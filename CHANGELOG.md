# Changelog

All notable changes to this project will be documented in this file.

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
