# Draw Distance Clamp Audit (TODO)

## Summary
OpenSim currently clamps draw distance and region view distance in code, which can override the values configured in `OpenSim.ini`. This file documents where the clamps occur, when they were introduced, and what needs to be undone or made configurable for VergeGrid.

## Current Clamp Points (Code)

1) **Per-avatar draw distance clamp**
- File: `OpenSim/Region/Framework/Scenes/ScenePresence.cs`
- Location: `ScenePresence.DrawDistance` setter
- Behavior: clamps to `32f .. m_scene.MaxDrawDistance`
  - Current code: `m_drawDistance = Utils.Clamp(value, 32f, m_scene.MaxDrawDistance);`

2) **Region view distance clamp**
- File: `OpenSim/Region/Framework/Scenes/ScenePresence.cs`
- Location: `ScenePresence.RegionViewDistance` getter
- Behavior: clamps view distance to min/max region view limits
  - Current code: `Utils.Clamp(m_drawDistance + 64f, m_scene.MinRegionViewDistance, m_scene.MaxRegionViewDistance);`
  - NOTE: Earlier versions used `Util.Clamp(m_drawDistance, 32f, m_scene.MaxRegionViewDistance)` (see commit history below).

3) **Config-time clamps after reading [Startup] values**
- File: `OpenSim/Region/Framework/Scenes/Scene.cs`
- Location: config load for draw distance values
- Behavior:
  - `DefaultDrawDistance` forced to `MaxDrawDistance` if higher.
  - `MaxRegionsViewDistance` forced down to `MaxDrawDistance` if higher.
  - `MinRegionsViewDistance` forced up to `96f` if lower.
  - `MinRegionsViewDistance` forced down to `MaxRegionsViewDistance` if higher.

## When the Clamps Were Introduced (Git History)

### Draw distance clamp (per-avatar)
- Commit: `5b7a3c703d0` (2015-09-30) - "clamp all draw distance changes within region limits"
- File: `OpenSim/Region/Framework/Scenes/ScenePresence.cs`
- Change: `DrawDistance` setter now clamps to `32f .. MaxDrawDistance`.

### Region view distance clamp
- Commit: `109723dc2df` (2016-01-26) - "add option MaxRegionsViewDistance"
- File: `OpenSim/Region/Framework/Scenes/ScenePresence.cs`
- Change: added `RegionViewDistance` with a clamp to `MaxRegionViewDistance`.

### DefaultDrawDistance forced down to MaxDrawDistance
- Commit: `3a0137cb45f` (2015-09-06) - "fix odd drawdistance control initialization"
- File: `OpenSim/Region/Framework/Scenes/Scene.cs`
- Change: enforce `DefaultDrawDistance <= MaxDrawDistance` during config load.

### MaxRegionsViewDistance forced down to MaxDrawDistance
- Commit: `668ff1e12ca` (2016-01-26) - "make sure MaxRegionsViewDistance is lower than MaxDrawDistance"
- File: `OpenSim/Region/Framework/Scenes/Scene.cs`
- Change: enforce `MaxRegionsViewDistance <= MaxDrawDistance`.

### MinRegionsViewDistance forced up to 96
- Commit: `63321f9ccc1` (2019-04-05) - "add option RegionViewDistance"
- File: `OpenSim/Region/Framework/Scenes/Scene.cs`
- Change: enforce `MinRegionsViewDistance >= 96f`.

### Clamp refactor (no behavior change)
- Commit: `acf02563f2b` (2021-09-14) - "some cleanup"
- File: `OpenSim/Region/Framework/Scenes/ScenePresence.cs`
- Change: `Util.Clamp` -> `Utils.Clamp`.

## TODO (VergeGrid)

1) Decide desired server behavior:
- Remove or relax server-side clamps, or make them configurable.
- Decide whether to keep a safety minimum (e.g., 32m) at all.

2) Implement code changes:
- `ScenePresence.DrawDistance` setter: remove or make clamp optional.
- `ScenePresence.RegionViewDistance` getter: remove/minimize clamp, or tie to explicit config.
- `Scene.cs` config load clamps: remove hard limits or gate them behind a config flag.

3) Update config documentation:
- `[Startup] DefaultDrawDistance / MaxDrawDistance / MaxRegionsViewDistance / MinRegionsViewDistance`
- Document viewer-side clamping expectations vs server behavior.

## How to Locate the Changes

Commands used to trace history:

- `git log -L "/m_drawDistance = .*Clamp/,+1:OpenSim/Region/Framework/Scenes/ScenePresence.cs"`
- `git log -L "/m_defaultDrawDistance > m_maxDrawDistance/,+1:OpenSim/Region/Framework/Scenes/Scene.cs"`
- `git log -L "/m_maxRegionViewDistance > m_maxDrawDistance/,+1:OpenSim/Region/Framework/Scenes/Scene.cs"`
- `git log -L "/m_minRegionViewDistance < 96f/,+1:OpenSim/Region/Framework/Scenes/Scene.cs"`
