# VergeGrid Endless Land and Seas

## Technical Roadmap

**Purpose:** Provide a durable, drop-in reference for implementing an "Endless Land and Seas" system for OpenSimulator within VergeGrid: a finite pool of regions recycled behind the scenes, with deterministic regeneration for seamless sailing and flight.

**Last updated:** 2025-12-19 (America/Chicago)

---

## Table of Contents

1. [Project Definition](#project-definition)
2. [Architectural Pillars](#architectural-pillars)
3. [System Components](#system-components)
4. [Coordinate and Seed Model](#coordinate-and-seed-model)
5. [Region Pool and Recycling](#region-pool-and-recycling)
6. [Phased Roadmap](#phased-roadmap)
7. [OpenSim Integration Points](#opensim-integration-points)
8. [Tooling and Debugging](#tooling-and-debugging)
9. [Risks and Mitigations](#risks-and-mitigations)
10. [Future Extensions](#future-extensions)
11. [Status](#status)

---

## Project Definition

### Goal

Create the **illusion** of an endless, contiguous world for **sailing** and **flight** using a **finite pool of pre-registered OpenSim regions**, recycled and regenerated deterministically as users traverse the world.

### Non-Goals

- True infinite region creation (dynamic GridService registration at runtime).
- Viewer hacks or custom viewer requirements.
- Regions appearing/disappearing while avatars are inside them in ways that violate core OpenSim assumptions.
- A full simulation of population, traffic, or economy.

### Success Criteria

- Users can travel in a straight line for hours with normal region crossings.
- No visible seams in terrain/water across region borders.
- Deterministic regeneration: the same world tile always produces the same results.
- Stable performance under sustained traversal.

---

## Architectural Pillars

1. **Deterministic world generation**
   
   - Every tile is generated from a stable seed derived from world coordinates.

2. **Finite pool of regions**
   
   - All regions in the pool exist permanently in the grid database.
   - Region UUIDs remain constant.

3. **Recycling, not creation**
   
   - Regions behind users are reset and regenerated for new world tiles.

4. **Seam continuity**
   
   - Terrain and features must match at borders via neighbor-aware sampling.

5. **Normal viewer behavior**
   
   - Standard OpenSim region crossings.
   - No viewer modifications.

---

## System Components

Suggested namespace layout:

```
VergeGrid.EndlessWorld
├── Services
│   ├── WorldSeedService
│   ├── TileCoordinateSystem
│   ├── PersistenceService
│   └── TelemetryService
├── Region
│   ├── RegionRecycleManager
│   ├── RegionStateTracker
│   └── RegenerationQueue
├── Generation
│   ├── TerrainGenerator
│   ├── WaterController
│   ├── BiomeSelector
│   └── CityGenerator (OpenSimCities-derived)
└── Debug
    ├── ConsoleCommands
    └── InWorldOverlays
```

Component responsibilities:

- **WorldSeedService**
  
  - Holds global seed.
  - Derives per-tile seeds from tile coordinates.

- **TileCoordinateSystem**
  
  - Converts between OpenSim region coords, pool coords, and world tile coords.

- **RegionRecycleManager**
  
  - Assigns world tiles to pool regions.
  - Decides when a region should be recycled.
  - Orchestrates clearing and regeneration.

- **TerrainGenerator**
  
  - Deterministic terrain for each world tile.
  - Edge-matched continuity.

- **WaterController**
  
  - Ocean height, shallow seas, rivers (later).
  - Optional wave/wind presets.

- **BiomeSelector**
  
  - Determines tile biome from seed + macro-noise.

- **CityGenerator**
  
  - Deterministic procedural city scaffolding.
  - Must support edge-aware roads and constraints.

- **PersistenceService**
  
  - Keeps player/anchored changes across recycling events.

- **TelemetryService**
  
  - Measures regeneration time, memory, object count, failures.

---

## Coordinate and Seed Model

### Global Coordinate Space

Define a world coordinate space independent of the pool.

- One OpenSim region = **256m x 256m** (standard).
- One world tile = **256m x 256m**.

For a global position `(globalX, globalY)`:

```
worldTileX = floor(globalX / 256)
worldTileY = floor(globalY / 256)
```

### Seed Derivation

A stable tile seed derived from a global seed plus tile coords:

```
tileSeed = Hash(globalSeed, worldTileX, worldTileY)
```

**Rule:** No RNG without a seed.

The tileSeed feeds:

- terrain height sampling
- biome choice
- road/city placement
- vegetation/props placement (future)

---

## Region Pool and Recycling

### Fixed Region Pool (Example)

Pre-register a pool of contiguous regions, e.g.:

- 9x9 (81) for walking tests
- 11x11 (121) for sailing
- 13x13 (169) for flight stress tests

These regions exist permanently. The system maps them to world tiles dynamically.

### Recycling Model (Ring Buffer)

- Track avatar(s) active area in world tiles.
- Maintain a radius of loaded/assigned tiles around the active center.
- When a pool region falls outside the active radius:
  1. mark it recyclable
  2. clear disposable content
  3. assign it a new world tile
  4. regenerate deterministically

**Key rule:** never recycle a region that still contains an avatar or active vehicle.

---

## Phased Roadmap

### Phase 1: Endless Ocean (Proof of Concept)

**Objective:** Validate seamless recycling with minimal complexity.

**Features**

- Flat ocean (fixed water height)
- No terrain variance
- No objects

**Tasks**

- Implement TileCoordinateSystem
- Implement RegionStateTracker
- Implement RegionRecycleManager (basic)
- Hard-code water height
- Logging for tile assignments and recycling events

**Exit Criteria**

- Sail indefinitely in one direction
- No visible seams
- No region crossing failures

---

### Phase 2: Deterministic Terrain Tiles

**Objective:** Add landmasses with guaranteed continuity.

**Features**

- Heightmap generation per tile
- Coastlines/islands
- Border seam continuity

**Tasks**

- TerrainGenerator module
- Neighbor-aware edge sampling
- Terrain baking to internal terrain format
- Optional caching for generated tiles

**Exit Criteria**

- No terrain steps at borders
- Same tile always regenerates identically

---

### Phase 3: Biomes and Water Features

**Objective:** Add variation while keeping navigation viable.

**Features**

- Biome selection per tile (seeded)
- Bays, shallow seas
- Rivers (if feasible without seam pain)
- Wind presets per biome

**Tasks**

- BiomeSelector
- WaterController enhancements
- Wind preset mapping (per biome)

**Exit Criteria**

- Biomes are stable and repeatable
- Water remains navigable for boats

---

### Phase 4: Cities and Infrastructure (OpenSimCities Integration)

**Objective:** Introduce sparse civilization without breaking seams.

**Features**

- Ports and coastal towns
- Sparse inland cities
- Road networks that align at borders

**Tasks**

- Fork OpenSimCities as CityGenerator
- Add deterministic seed input (no unseeded randomness)
- Add border-aware road stitching
- Add biome rules (only generate cities in appropriate tiles)
- Add placement constraints (slope, elevation, coast distance)

**Exit Criteria**

- Roads can cross region borders without discontinuity
- City placement is stable and does not overlap itself across cycles

---

### Phase 5: Persistence Layer

**Objective:** Allow player impact without breaking regeneration.

**Features**

- Anchored builds (ports, lighthouses, player bases)
- Claims (tile-level, parcel-like, or region-like)
- Optional locking of specific tiles

**Tasks**

- PersistenceService
- Identify persistent vs disposable objects
- Store deltas per tile (not per pool region)
- Restore deltas after regeneration

**Exit Criteria**

- Player changes survive recycling
- Disposable world content remains disposable

---

### Phase 6: Performance, Stability, and Recovery

**Objective:** Production readiness and resilience.

**Features**

- Throttled regeneration
- Regeneration queue with prioritization
- Failure recovery and safe fallback

**Tasks**

- TelemetryService with metrics
- Memory/object cleanup audit
- Regen concurrency controls
- Watchdog for stuck regeneration

**Exit Criteria**

- Multi-hour traversal stable under load
- Predictable regen times
- No runaway memory growth

---

## OpenSim Integration Points

### Module Type

- Start with **INonSharedRegionModule** (region-local control).
- Optionally add a shared service (singleton) for coordination if needed.

### Likely Hooks

- Region lifecycle: initialization, post-init, shutdown
- Scene and entity management: object delete/create APIs
- Terrain module hooks: setting heightmap
- Presence tracking: ensure regions are not recycled while occupied
- Vehicle edge cases: crossings while seated

---

## Tooling and Debugging

### Console Commands (Recommended)

- `ew status`         - show pool size, active tiles, recycle queue
- `ew tile`           - show current world tile mapping for a region
- `ew regen <region>` - force regeneration for a pool region
- `ew lock <tile>`    - lock a world tile (no recycle)
- `ew seed`           - show global seed and current tile seed

### Visual Overlays (Optional)

- Tile boundary display
- WorldTileX/Y labels at region corners
- Debug objects showing seam checks

### Logging

- Log every tile assignment, recycling event, and regeneration duration.
- Log failures with enough state to reproduce tileSeed and tile coords.

---

## Risks and Mitigations

| Risk                        | Impact                         | Mitigation                                                          |
| --------------------------- | ------------------------------ | ------------------------------------------------------------------- |
| Terrain seams               | Visible border steps           | Neighbor-aware sampling and edge constraints                        |
| Viewer caching oddities     | Visual mismatch on fast travel | Keep region UUIDs fixed; avoid rapid morphing while avatars present |
| Physics spikes during regen | Lag / stalls                   | Regen throttling + queues; avoid regen near active crossings        |
| DB bloat                    | Slow restores / backups        | Store deltas per tile; keep disposable content out of DB            |
| Vehicle crossing edge cases | Avatar stuck or desync         | Conservative recycle rules; crossing-aware scheduling               |

---

## Future Extensions

- Shipping lanes and buoy markers
- Deterministic landmark system (rare islands, ruins, stations)
- Procedural lore points (discoverables)
- Map tile renderer that reflects the generated world
- Export tooling: “exploration charts” per seed

---

## Status

**State:** Concept approved  
**Next step:** Implement **Phase 1 (Endless Ocean)** as a proof of concept.

**Suggested first repo/module name:** `VergeGrid.EndlessWorld`
