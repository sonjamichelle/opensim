# Changelog

All notable changes to this project will be documented in this file.

## 2025-12-19 11:40:00

### Tooling

- Converted `Prebuild/src/Prebuild.csproj` to SDK-style for C# Dev Kit compatibility on .NET Framework (`net48`).
- Preserved strong-name signing (`Prebuild.snk`), `StartupObject`, `ApplicationIcon`, and existing warning settings.
- Kept legacy `NET46` compile constant and output paths (`bin\Debug\`, `bin\Release\`) to avoid build artifact changes.
- Carried forward embedded resources (`App.ico`, `data\prebuild-1.10.xsd`, `data\autotools.xml`) and `app.config` inclusion.
- Disabled SDK auto-generated assembly info so `Properties/AssemblyInfo.cs` remains authoritative.

**Files touched**

- `Prebuild/src/Prebuild.csproj`

## 2025-12-19 10:53:54

### Map Rendering

- Added `[Map] SuppressJ2KWarnings` to silence CSJ2K console warnings during Warp3D map generation.
- Introduced `J2kDecoderLogSilencer` to temporarily suppress `Console.Out`/`Console.Error` while decoding.
- Wrapped CSJ2K decode calls for terrain splat textures, sculpt/mesh decode, prim textures, and average-color sampling.
- Updated `TerrainSplat.Splat` to accept a suppression flag and wired it from `Warp3DImageModule`.
- Documented the new ini flag in `OpenSim.ini.example` with a short description.

**Files touched**

- `OpenSim/Region/CoreModules/World/Warp3DMap/J2kDecoderLogSilencer.cs` (new)
- `OpenSim/Region/CoreModules/World/Warp3DMap/TerrainSplat.cs`
- `OpenSim/Region/CoreModules/World/Warp3DMap/Warp3DImageModule.cs`
- `bin/OpenSim.ini.example`
