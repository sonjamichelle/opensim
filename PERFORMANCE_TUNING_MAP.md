# PERFORMANCE_TUNING_MAP.md

## A Practical Mental Model for OpenSim Performance

> “Lag” is not a single thing.
> 
> In OpenSim, performance problems are almost always **misattributed** because
> multiple subsystems fail *silently* and present similar user-visible symptoms.

This document maps **symptoms → subsystems → tuning areas** so administrators
can diagnose *correctly* before changing configuration.

---

## 1. THE CORE LIE: “LAG”

Users say “lag”.
Admins chase CPU.
The real cause is usually **one of five pipelines**.

OpenSim performance issues almost always originate in:

1. **Network Delivery**
2. **Interest Management**
3. **Physics Simulation**
4. **Scripting Execution**
5. **Viewer Update Saturation**

Each has **distinct failure signatures** that masquerade as each other.

---

## 2. PERFORMANCE PIPELINES (HIGH LEVEL)

┌───────────┐
│ Viewer │
└─────┬─────┘
│
▼
┌──────────────────┐
│ Interest Mgmt │ ← What gets sent, when, and to whom
└─────┬────────────┘
│
▼
┌──────────────────┐
│ Network (UDP) │ ← How updates are delivered
└─────┬────────────┘
│
▼
┌──────────────────┐
│ Simulator Core │
│ ├ Physics │
│ ├ Scripts │
│ ├ Scene Graph │
└──────────────────┘

yaml
Copy code

Breakage anywhere **upstream** looks like “lag” downstream.

---

## 3. SYMPTOM → SUBSYSTEM MAP

### A. Avatars rubber-band, snap, or jitter

**Likely cause:**

- InterestManagement
- Network throttling
- Physics timestep mismatch

**DO NOT START WITH:**

- Script limits
- Viewer draw distance

**Relevant sections:**

- `[InterestManagement]`
- `[ClientStack.LindenUDP]`
- Physics engine step size

---

### B. Objects rez slowly or inconsistently

**Likely cause:**

- Interest prioritization
- Packet saturation
- Asset delivery caps

**Relevant sections:**

- `[InterestManagement]`
- `[ClientStack.LindenUDP]`
- `[ClientStack.LindenCaps]`

---

### C. Terrain edits stall or viewer freezes while terraforming

**Likely cause:**

- Parcel layer flooding

**Relevant sections:**

- `[LandManagement]`

**Classic admin mistake:**

> “Physics lag”

This is **almost never physics**.

---

### D. Scripts “feel slow” but CPU is idle

**Likely cause:**

- Script engine worker starvation
- Timer clamping
- Excessive script yielding

**Relevant sections:**

- `[YEngine]`

---

### E. Random viewer disconnects, phantom failures, ghost bugs

**Likely cause:**

- Packet pooling misconfiguration
- UDP corruption masking as viewer bugs

**Relevant sections:**

- `[PacketPool]`

---

## 4. INTEREST MANAGEMENT (THE MOST MISUNDERSTOOD SYSTEM)

### What it actually does

Interest Management decides:

- **WHAT** objects update
- **WHEN** they update
- **IN WHAT ORDER**
- **AT WHAT PRIORITY**

It does **not**:

- Manage user “interest”
- Track engagement
- Control permissions

The name is historical and misleading.

### Why it matters

Every viewer has a **finite bandwidth budget**.
Interest Management decides where that budget is spent.

Bad prioritization:

- Makes nearby avatars stutter
- Makes distant junk update first
- Creates “lag” with idle CPU

### Key sections

- `[InterestManagement]`
- `[ClientStack.LindenUDP]`

---

## 5. NETWORK DELIVERY ≠ BANDWIDTH

More bandwidth does **not** fix:

- Packet flooding
- Update starvation
- Poor prioritization

### What actually matters

- Update frequency
- Packet reuse
- Burst behavior
- Terse update periods

### Dangerous knobs

- PacketPool recycling
- UDP throttles
- Terse update suppression

These are **loaded firearms**.
Defaults exist for a reason.

---

## 6. PHYSICS ≠ PERFORMANCE (UNTIL IT IS)

Physics is often blamed **last**, correctly.

But:

- Wrong timestep
- Wrong solver iteration count
- Wrong avatar collision tuning

can silently cascade into:

- Interest overload
- Network saturation
- Script backpressure

Physics problems often **present as network lag**.

---

## 7. SCRIPT ENGINE MISDIAGNOSIS

Script lag almost never looks like:

- “Scripts are slow”

It looks like:

- Delayed interactions
- Unresponsive HUDs
- Timers firing inconsistently

CPU may remain idle while scripts starve.

---

## 8. OBSERVABILITY IS NOT OPTIONAL

You cannot tune what you cannot see.

OpenSim exposes:

- Frame timing
- Update cadence
- Script latency
- Physics pacing

But **does not explain them**.

### Minimum viable observability stack

- Region console monitoring
- `/SStats` endpoint
- External aggregation (Prometheus, Grafana)

This is **not optional** on a production grid.

---

## 9. THE GOLDEN RULE

> **Never tune in isolation.**

Changing:

- Interest settings affects network
- Network affects script perception
- Physics affects interest pressure

**Everything couples.**

---

## 10. SAFE TUNING ORDER (DO NOT SKIP)

1. Observe (logs, stats, trends)
2. Identify dominant symptom
3. Map symptom to subsystem
4. Adjust **one** setting
5. Re-observe
6. Roll back if needed

---

## 11. WHAT THIS DOCUMENT IS NOT

- Not a magic config
- Not a performance recipe
- Not a guarantee

It is a **map**, not a destination.

---

## 12. FUTURE EXTENSIONS (TODO)

- Varregion-specific profiles
- Event-region presets
- Education vs roleplay tuning
- WebUI-driven performance states
- Automated anomaly detection

---

## FINAL NOTE

If you are “fixing lag” by:

- Randomly touching UDP values
- Copying forum configs
- Cranking limits upward

You are not tuning.
You are gambling.

This document exists to stop that.


