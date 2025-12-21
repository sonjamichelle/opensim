# Changelog

All notable changes to this project will be documented in this file.

## 2025-12-20 18:55:00

### Config Unification

- Enforced `OpenSim.ini` as the master-derived config and added interactive prompts for missing Startup keys, with write-back to `OpenSim.ini`.
- Centralized missing-key prompting in `ConfigPrompt` and reused it for map tile path configuration.
- Standardized map tile storage on `[MapImageService] TilesStoragePath` and removed legacy `MaptileDirectory` usage.
- Fixed Warp3D map module build ambiguity by qualifying `System.IO.Path`.
- Enforced Offline Message V2 as the only allowed `OfflineMessageModule` value.
- Added a config load harness for interactive validation and map tile path diagnostics.
- Generated Phase 1–4 progress reports and TODO references in repo root.

**Files touched**

- `OpenSim/Framework/ConfigPrompt.cs` (new)
- `OpenSim/Framework/OpenSim.Framework.csproj`
- `OpenSim/Region/Application/ConfigurationLoader.cs`
- `OpenSim/Tools/Configger/ConfigurationLoader.cs`
- `OpenSim/Region/CoreModules/World/WorldMap/WorldMapModule.cs`
- `OpenSim/Region/CoreModules/World/Warp3DMap/Warp3DImageModule.cs`
- `OpenSim/Services/MapImageService/MapImageService.cs`
- `OpenSim/Services/GridService/HypergridLinker.cs`
- `OpenSim/Region/CoreModules/Avatar/InstantMessage/OfflineMessageModule.cs`
- `bin/OpenSim.ini`
- `bin/OpenSim.ini.template`
- `OpenSim.ini.unifying.prompts.txt`
- `Tools/ConfigLoadHarness/*`
- `ini_phase1A_inventory.*`
- `ini_phase1B_coverage_matrix.*`
- `ini_phase1C_template_gap.*`
- `ini_phase1D_default_conflicts.*`
- `ini_phase3A_config_load_order_audit.*`
- `ini_phase3B_hard_coded_defaults.*`
- `ini_phase3B_hardcoded_overrides.*`
- `ini_phase3B_hardcoded_override_risks.*`
- `ini_phase3C_unused_config_keys.*`
- `ini_phase3C_unused_config_keys_refined.*`
- `ini_phase3C_unused_config_keys_hinted.*`
- `ini_phase3D_inconsistent_config_readers.*`
- `ini_phase3D_inconsistent_config_readers_sorted.*`
- `ini_phase3D_inconsistent_config_readers_opensim_ini_focus.*`
- `ini_phase4A_fix_candidates.*`
- `ini_phase4B_todo.txt`

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
