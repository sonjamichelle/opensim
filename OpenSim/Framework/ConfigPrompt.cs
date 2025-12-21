/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Nini.Config;

namespace OpenSim.Framework
{
    public static class ConfigPrompt
    {
        private static string s_primaryIniPath = string.Empty;
        private static string s_templateIniPath = string.Empty;

        public static void SetConfigPaths(string primaryIniPath, string templateIniPath)
        {
            s_primaryIniPath = primaryIniPath ?? string.Empty;
            s_templateIniPath = templateIniPath ?? string.Empty;
        }

        public static string RequireSetting(IConfigSource configSource, string section, string key,
            string defaultValue, bool forbidNone)
        {
            if (configSource == null)
                return defaultValue ?? string.Empty;

            IConfig config = configSource.Configs[section] ?? configSource.AddConfig(section);
            string current = config.GetString(key, string.Empty);
            if (!String.IsNullOrWhiteSpace(current))
            {
                if (!forbidNone || !String.Equals(current.Trim(), "none", StringComparison.OrdinalIgnoreCase))
                    return current;
            }

            if (forbidNone && String.Equals(current.Trim(), "none", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine($"[CONFIG]: {section}.{key}=none is not allowed; prompting for replacement");

            while (true)
            {
                Console.WriteLine($"[CONFIG]: Missing required setting [{section}] {key}");
                Console.WriteLine("[CONFIG]: Choose: (A)bort, (M)aster Template, (E)nter Value, (R)etry");
                Console.Write("[CONFIG]: Enter choice: ");
                string choice = Console.ReadLine();
                if (choice == null)
                    continue;

                switch (choice.Trim().ToUpperInvariant())
                {
                    case "A":
                        return null;
                    case "M":
                    {
                        if (TryGetIniSetting(s_templateIniPath, section, key, true, out string fromTemplate))
                        {
                            config.Set(key, fromTemplate);
                            return fromTemplate;
                        }
                        if (!String.IsNullOrEmpty(defaultValue))
                        {
                            config.Set(key, defaultValue);
                            return defaultValue;
                        }
                        Console.WriteLine("[CONFIG]: Master Template value not found for this key.");
                        break;
                    }
                    case "E":
                    {
                        Console.Write("[CONFIG]: Enter value: ");
                        string input = Console.ReadLine();
                        if (String.IsNullOrWhiteSpace(input))
                        {
                            Console.WriteLine("[CONFIG]: Value cannot be empty.");
                            break;
                        }
                        string value = input.Trim();
                        config.Set(key, value);
                        if (!String.IsNullOrEmpty(s_primaryIniPath) && File.Exists(s_primaryIniPath))
                            UpdateIniSetting(s_primaryIniPath, section, key, value);
                        return value;
                    }
                    case "R":
                    {
                        if (TryGetIniSetting(s_primaryIniPath, section, key, false, out string fromPrimary))
                        {
                            config.Set(key, fromPrimary);
                            return fromPrimary;
                        }
                        Console.WriteLine("[CONFIG]: Key still missing in primary ini.");
                        break;
                    }
                    default:
                        Console.WriteLine("[CONFIG]: Invalid choice.");
                        break;
                }
            }
        }

        private static bool TryGetIniSetting(string filePath, string section, string key, bool allowCommented, out string value)
        {
            value = string.Empty;
            if (String.IsNullOrWhiteSpace(filePath))
                return false;
            if (!File.Exists(filePath))
                return false;

            string currentSection = string.Empty;
            foreach (string rawLine in File.ReadLines(filePath))
            {
                if (rawLine == null)
                    continue;

                string line = rawLine.Trim();
                if (line.Length == 0)
                    continue;

                if (line.StartsWith("[") && line.Contains("]"))
                {
                    int endIndex = line.IndexOf(']');
                    currentSection = line.Substring(1, endIndex - 1).Trim();
                    continue;
                }

                if (!String.Equals(currentSection, section, StringComparison.Ordinal))
                    continue;

                bool isComment = line.StartsWith(";") || line.StartsWith("#");
                string candidate = line;
                if (isComment)
                {
                    if (!allowCommented)
                        continue;
                    candidate = line.TrimStart(';', '#').TrimStart();
                    if (candidate.Length == 0)
                        continue;
                }

                int equalsIndex = candidate.IndexOf('=');
                if (equalsIndex <= 0)
                    continue;

                string candidateKey = candidate.Substring(0, equalsIndex).Trim();
                if (!String.Equals(candidateKey, key, StringComparison.Ordinal))
                    continue;

                string candidateValue = candidate.Substring(equalsIndex + 1);
                value = TrimInlineComment(candidateValue);
                return true;
            }

            return false;
        }

        private static string TrimInlineComment(string value)
        {
            if (String.IsNullOrEmpty(value))
                return string.Empty;

            bool inQuotes = false;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (c == '"')
                    inQuotes = !inQuotes;
                if (!inQuotes && c == ';')
                    return value.Substring(0, i).Trim();
            }

            return value.Trim();
        }

        private static void UpdateIniSetting(string filePath, string section, string key, string value)
        {
            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            int sectionStart = -1;
            int sectionEnd = lines.Count;
            string sectionHeader = $"[{section}]";
            string indent = string.Empty;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.Contains("]"))
                {
                    int endIndex = trimmed.IndexOf(']');
                    string currentSection = trimmed.Substring(1, endIndex - 1).Trim();
                    if (sectionStart >= 0)
                    {
                        sectionEnd = i;
                        break;
                    }
                    if (String.Equals(currentSection, section, StringComparison.Ordinal))
                        sectionStart = i;
                }
                else if (sectionStart >= 0 && indent.Length == 0)
                {
                    string leading = line.Substring(0, line.Length - line.TrimStart().Length);
                    if (leading.Length > 0)
                        indent = leading;
                }
            }

            if (sectionStart < 0)
            {
                if (lines.Count > 0 && !String.IsNullOrWhiteSpace(lines[^1]))
                    lines.Add(string.Empty);
                lines.Add(sectionHeader);
                lines.Add($"{key} = {value}");
                File.WriteAllLines(filePath, lines);
                return;
            }

            for (int i = sectionStart + 1; i < sectionEnd; i++)
            {
                string line = lines[i];
                string trimmed = line.TrimStart();
                bool isComment = trimmed.StartsWith(";") || trimmed.StartsWith("#");
                string candidate = trimmed;
                if (isComment)
                    candidate = trimmed.TrimStart(';', '#').TrimStart();

                int equalsIndex = candidate.IndexOf('=');
                if (equalsIndex <= 0)
                    continue;

                string candidateKey = candidate.Substring(0, equalsIndex).Trim();
                if (!String.Equals(candidateKey, key, StringComparison.Ordinal))
                    continue;

                string leading = line.Substring(0, line.Length - line.TrimStart().Length);
                lines[i] = $"{leading}{key} = {value}";
                File.WriteAllLines(filePath, lines);
                return;
            }

            string insertIndent = indent;
            lines.Insert(sectionEnd, $"{insertIndent}{key} = {value}");
            File.WriteAllLines(filePath, lines);
        }
    }
}
