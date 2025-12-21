using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OpenMetaverse;
using Nini.Config;
using OpenSim;
using OpenSim.Framework;
using OpenSim.Data;
using OpenSim.Region.CoreModules.World.WorldMap;
using OpenSim.Region.CoreModules.World.Warp3DMap;
using OpenSim.Services.GridService;
using OpenSim.Services.MapImageService;

namespace ConfigLoadHarness
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            bool useTestConfigs = false;
            bool testMapTiles = false;
            bool testHypergridMapTiles = false;
            List<string> filteredArgs = new List<string>();

            foreach (string arg in args)
            {
                if (string.Equals(arg, "--use-test-configs", StringComparison.OrdinalIgnoreCase))
                {
                    useTestConfigs = true;
                }
                else if (string.Equals(arg, "--test-map-tiles", StringComparison.OrdinalIgnoreCase))
                {
                    testMapTiles = true;
                }
                else if (string.Equals(arg, "--test-hg-map-tiles", StringComparison.OrdinalIgnoreCase))
                {
                    testHypergridMapTiles = true;
                }
                else
                {
                    filteredArgs.Add(arg);
                }
            }

            string repoRoot = Directory.GetCurrentDirectory();
            string binDir = Path.Combine(repoRoot, "bin");
            if (Directory.Exists(binDir))
                Directory.SetCurrentDirectory(binDir);

            ArgvConfigSource argvSource = new ArgvConfigSource(filteredArgs.ToArray());
            argvSource.AddSwitch("Startup", "inimaster", "m");
            argvSource.AddSwitch("Startup", "inifile", "i");
            argvSource.AddSwitch("Startup", "inidirectory", "d");

            IConfig startupConfig = argvSource.Configs["Startup"];
            if (startupConfig == null)
                startupConfig = argvSource.AddConfig("Startup");

            if (useTestConfigs)
            {
                string testDir = Path.Combine(repoRoot, "Tools", "ConfigLoadHarness", "test-configs");
                Directory.CreateDirectory(testDir);

                string templatePath = Path.Combine(testDir, "OpenSim.ini.template");
                string iniPath = Path.Combine(testDir, "OpenSim.ini");

                File.WriteAllText(templatePath, BuildTestTemplate());
                File.WriteAllText(iniPath, BuildTestIni(testMapTiles || testHypergridMapTiles));

                startupConfig.Set("inimaster", templatePath);
                startupConfig.Set("inifile", iniPath);
            }

            Console.WriteLine("[HARNESS]: Starting config load");
            Console.WriteLine($"[HARNESS]: Working directory: {Directory.GetCurrentDirectory()}");
            if (useTestConfigs)
                Console.WriteLine("[HARNESS]: Using test-configs (missing keys should prompt)");
            if (testMapTiles)
                Console.WriteLine("[HARNESS]: Map tile path test enabled");
            if (testHypergridMapTiles)
                Console.WriteLine("[HARNESS]: Hypergrid map tile path test enabled");

            ConfigurationLoader loader = new ConfigurationLoader();
            EnvConfigSource envConfigSource = new EnvConfigSource();
            ConfigSettings settings;
            NetworkServersInfo networkInfo;

            OpenSimConfigSource config = loader.LoadConfigSettings(argvSource, envConfigSource, out settings, out networkInfo);

            Console.WriteLine("[HARNESS]: Config load complete");
            IConfig loadedStartup = config.Source.Configs["Startup"];
            if (loadedStartup != null)
            {
                Console.WriteLine("[HARNESS]: Startup values:");
                DumpStartupValue(loadedStartup, "inimaster");
                DumpStartupValue(loadedStartup, "inifile");
                DumpStartupValue(loadedStartup, "ConfigDirectory");
                DumpStartupValue(loadedStartup, "RegistryLocation");
                DumpStartupValue(loadedStartup, "regionload_regionsdir");
                DumpStartupValue(loadedStartup, "physics");
                DumpStartupValue(loadedStartup, "meshing");
            }

            if (testMapTiles)
                RunMapTilePathTest(config.Source, repoRoot);
            if (testHypergridMapTiles)
                RunHypergridTilePathTest(config.Source);

            return 0;
        }

        private static void DumpStartupValue(IConfig config, string key)
        {
            string value = config.GetString(key, string.Empty);
            Console.WriteLine($"[HARNESS]: {key} = {value}");
        }

        private static string BuildTestTemplate()
        {
            return string.Join(Environment.NewLine, new[]
            {
                "; test template for ConfigLoadHarness",
                "[Startup]",
                "; inimaster = \"OpenSim.ini.template\"",
                "; inifile = \"OpenSim.ini\"",
                "; ConfigDirectory = \".\"",
                "; RegistryLocation = \".\"",
                "; regionload_regionsdir = \"Regions\"",
                "; physics = ubODE",
                "; meshing = ubODEMeshmerizer",
                "",
                "[Network]",
                "; intentionally empty for harness defaults",
                ""
            });
        }

        private static string BuildTestIni(bool includeMapTileSections)
        {
            List<string> lines = new List<string>
            {
                "; test ini for ConfigLoadHarness",
                "[Startup]",
                ""
            };

            if (includeMapTileSections)
            {
                lines.Add("[Map]");
                lines.Add("WorldMapModule = \"WorldMap\"");
                lines.Add("MapImageModule = \"Warp3DImageModule\"");
                lines.Add("TilesStoragePath = \"legacy-maptiles\"");
                lines.Add("");
                lines.Add("[MapImageService]");
                lines.Add("TilesStoragePath = \"canonical-maptiles\"");
                lines.Add("");
                lines.Add("[GridService]");
                lines.Add("HypergridLinker = true");
                lines.Add("MapTileDirectory = \"legacy-hg-maptiles\"");
                lines.Add("");
                lines.Add("[Hypergrid]");
                lines.Add("GatekeeperURI = \"http://localhost:8002/\"");
                lines.Add("HomeURI = \"http://localhost:8002/\"");
                lines.Add("");
            }

            return string.Join(Environment.NewLine, lines);
        }

        private static void RunMapTilePathTest(IConfigSource configSource, string repoRoot)
        {
            Console.WriteLine("[HARNESS]: Map tile path test");
            string iniPath = Path.Combine(repoRoot, "Tools", "ConfigLoadHarness", "test-configs", "OpenSim.ini");
            string templatePath = Path.Combine(repoRoot, "Tools", "ConfigLoadHarness", "test-configs", "OpenSim.ini.template");
            ConfigPrompt.SetConfigPaths(iniPath, templatePath);

            MapImageService mapService = new MapImageService(configSource);
            string mapServicePath = ReadPrivateStaticString(typeof(MapImageService), "m_TilesStoragePath");
            Console.WriteLine($"[HARNESS]: MapImageService TilesStoragePath = {mapServicePath}");

            WorldMapModule worldMap = new WorldMapModule();
            worldMap.Initialise(configSource);
            string worldMapPath = ReadPrivateInstanceString(worldMap, "m_mapTileCacheDirectory");
            if (worldMapPath == "<missing-field>")
                Console.WriteLine("[HARNESS]: WorldMapModule fields = " + GetPrivateInstanceFieldNames(worldMap));
            Console.WriteLine($"[HARNESS]: WorldMapModule cache directory = {worldMapPath}");

            Warp3DImageModule warp3D = new Warp3DImageModule();
            warp3D.Initialise(configSource);
            string warp3DPath = ReadPrivateInstanceString(warp3D, "m_mapTileCacheDirectory");
            if (warp3DPath == "<missing-field>")
                Console.WriteLine("[HARNESS]: Warp3DImageModule fields = " + GetPrivateInstanceFieldNames(warp3D));
            Console.WriteLine($"[HARNESS]: Warp3DImageModule cache directory = {warp3DPath}");

            IConfig mapImageConfig = configSource.Configs["MapImageService"];
            if (mapImageConfig != null)
            {
                string configuredPath = mapImageConfig.GetString("TilesStoragePath", string.Empty);
                Console.WriteLine("[HARNESS]: Config MapImageService.TilesStoragePath = " + configuredPath);
            }
        }

        private static string ReadPrivateStaticString(Type type, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
                return "<missing-field>";
            return field.GetValue(null) as string ?? "<null>";
        }

        private static string ReadPrivateInstanceString(object instance, string fieldName)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
                return "<missing-field>";
            return field.GetValue(instance) as string ?? "<null>";
        }

        private static void RunHypergridTilePathTest(IConfigSource configSource)
        {
            Console.WriteLine("[HARNESS]: Hypergrid map tile path test");
            DummyRegionData db = new DummyRegionData();
            HypergridLinker linker = new HypergridLinker(configSource, null, db);
            string path = ReadPrivateInstanceString(linker, "m_MapTileDirectory");
            Console.WriteLine("[HARNESS]: HypergridLinker MapTileDirectory = " + path);
        }

        private sealed class DummyRegionData : IRegionData
        {
            public RegionData Get(UUID regionID, UUID scopeID) => null;
            public List<RegionData> Get(string regionName, UUID scopeID) => null;
            public RegionData GetSpecific(string regionName, UUID scopeID) => null;
            public RegionData Get(int x, int y, UUID scopeID) => null;
            public List<RegionData> Get(int xStart, int yStart, int xEnd, int yEnd, UUID scopeID) => null;
            public bool Store(RegionData data) => false;
            public bool SetDataItem(UUID principalID, string item, string value) => false;
            public bool Delete(UUID regionID) => false;
            public List<RegionData> GetDefaultRegions(UUID scopeID) => null;
            public List<RegionData> GetDefaultHypergridRegions(UUID scopeID) => null;
            public List<RegionData> GetFallbackRegions(UUID scopeID) => null;
            public List<RegionData> GetHyperlinks(UUID scopeID) => null;
            public List<RegionData> GetOnlineRegions(UUID scopeID) => null;
        }

        private static string GetPrivateInstanceFieldNames(object instance)
        {
            FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            List<string> names = new List<string>();
            foreach (FieldInfo field in fields)
                names.Add(field.Name);
            return string.Join(", ", names);
        }
    }
}
