# OpenSim INI Unification Roadmap

## Goals
- Make `OpenSim.ini.template` the canonical schema and prevent silent fallbacks to other config sources.
- Ensure runtime settings resolve through the derived `OpenSim.ini` with clear, user-driven recovery prompts.
- Eliminate duplicate/competing config keys that cause nondeterministic behavior.
- Identify and fix hard-coded overrides that bypass config.
- Provide repeatable test harnesses to validate config load order and key usage.

## Guiding Principles
- Preserve user intent and comments in config templates.
- Avoid silent fallbacks; require explicit user acknowledgement when needed.
- Keep fixes minimal and scoped; no refactors unless explicitly authorized.

## Completed (This Branch)
- Phase 1A?1D: Inventory, coverage matrix, template gap report, conflict report.
- Phase 2A: Template completeness check.
- Phase 2B: Template augmentation (commented only) with source annotations.
- Phase 3A: Config load order audit with prompt behavior updates.
- Phase 3B: Hard-coded default audit + override risks.
- Phase 3C: Unused config keys report + template annotations.
- Phase 3D: Inconsistent readers report + OpenSim.ini focus tagging.
- Phase 4A: Fix candidate list (all items marked SOLVED).
- Phase 4B (Item 1): Offline message module clean break to V2 only.
- Map tile canonicalization to `MapImageService.TilesStoragePath`.
- New config prompt helper and harness to validate behavior.

## In Progress / Pending
- Phase 4B: Remaining inconsistent readers (non-Startup, non-Phase4A items).
- Validate non-map settings that still use legacy keys or sections.
- Decide on any remaining backward-compatibility exceptions.

## Next Phases
- Phase 4B (cont.): One-setting fixes for each remaining inconsistent reader.
- Phase 4C: Unify remaining duplicated keys (no behavior drift).
- Phase 4D: Consolidate map tile module reads to a single canonical source.
- Phase 5A: Final reconciliation of template comments and key placement.
- Phase 5B: Regression checklist for config load order and prompts.
- Phase 6: Documentation updates for deployment and migration.

## Validation Plan
- Build full solution after each milestone.
- Run ConfigLoadHarness:
  - Startup key prompt flow (Abort/Master/Enter/Retry).
  - Map tile path reads (MapImageService/WorldMap/Warp3D/Hypergrid).
- Run OpenSim with:
  - `OpenSim.ini` present but missing key.
  - `OpenSim.ini` missing entirely.
  - Explicit `-inifile` and `-inidirectory` overrides.

## Artifacts
- Phase reports: `ini_phase1A_*` through `ini_phase4A_*`.
- Prompt tracking: `OpenSim.ini.unifying.prompts.txt`.
- Harness: `Tools/ConfigLoadHarness`.

## Open Questions
- Which remaining inconsistent keys should be retired vs. merged?
- Are any legacy keys required for specific deployments?
- What is the expected migration path for grids with custom overrides?
