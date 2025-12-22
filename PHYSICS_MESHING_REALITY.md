# Physics Meshing in OpenSim

## Myths, Realities, and Operational Tradeoffs

### Purpose of This Document

This document exists to correct a long-standing and frequently repeated misconception in the OpenSim ecosystem:

> “You must use ubODEMeshmerizer for correct physics.”

That statement is **false**, **misleading**, and **operationally harmful** when presented without context.

Physics behavior in OpenSim is not determined by a single component. It is the result of an **interaction chain** involving multiple subsystems, configuration choices, and content quality.

This document explains what actually matters, what does not, and why blanket recommendations are dangerous.

---

## The Simplified Myth

The claim usually appears in some variation of:

- “ubODEMeshmerizer is required”

- “Meshmerizer is broken”

- “Bullet physics only works correctly with ubODEMeshmerizer”

These statements are typically made:

- Without specifying the use case

- Without considering region scale

- Without considering content quality

- Without acknowledging configuration context

They are often repeated by users troubleshooting **unrelated problems**.

---

## Reality: Physics Is a System, Not a Toggle

Physics behavior in OpenSim is influenced by **at least** the following components:

1. Physics engine

2. Mesher choice

3. Build quality and collision design

4. Region and estate configuration

5. Simulator performance and load

6. Network stability

7. Viewer-side interpretation

Changing **one** of these in isolation rarely fixes systemic issues.

---

## What a Mesher Actually Does

A mesher is responsible for generating a **collision representation** from object geometry.

It does **not**:

- Define physics rules

- Control gravity

- Control avatar movement logic

- Fix poor content

- Compensate for overloaded simulators

Its job is limited and specific.

---

## ubODEMeshmerizer vs Meshmerizer

### ubODEMeshmerizer

**Strengths**

- Better handling of complex mesh shapes

- More consistent convex decomposition

- Reduced physics rebuild churn in some scenarios

**Costs**

- Higher CPU usage

- Higher memory usage

- Increased physics complexity

- Can amplify bad builds rather than fix them

### Meshmerizer

**Strengths**

- Faster

- Simpler collision models

- Lower server load

- More predictable behavior for simple builds

**Limitations**

- Less accurate collision on complex mesh

- Not suitable for all mesh-heavy scenes

Neither is universally “correct.”

---

## Why “Just Use ubODEMeshmerizer” Is Bad Advice

### 1. It masks root causes

Most reported “physics problems” come from:

- Non-optimized mesh

- Excessive physics shapes

- Incorrect prim physics flags

- Poor LODs

- Overloaded regions

Switching meshers may *change* the symptom without fixing the cause.

---

### 2. It increases server load silently

ubODEMeshmerizer increases:

- CPU cost

- Physics step complexity

- Memory pressure

On large estates or VAR regions, this can:

- Reduce region performance

- Increase lag

- Create cascading physics failures

Without warning.

---

### 3. It ignores use-case diversity

Different regions have different needs:

- Build regions

- Interior-only regions

- Low-gravity or lunar environments

- High-traffic social regions

- Scenic, non-interactive regions

A single mesher choice for all of these is irrational.

---

## The Real Physics Equation

Perceived “physics quality” is the result of:

`Mesher choice + Physics engine + Build discipline + Region scale + Server performance + Configuration sanity + Viewer stability`

Focusing on one variable while ignoring the others guarantees frustration.

---

## When ubODEMeshmerizer *does* make sense

ubODEMeshmerizer is appropriate when:

- Regions contain complex mesh geometry

- Accurate collision is required

- Server resources are sufficient

- Builds are optimized intentionally

- Physics tuning is deliberate

It is a **tool**, not a default.

---

## When Meshmerizer is preferable

Meshmerizer is often the better choice when:

- Regions are large-scale

- Content is mostly prim-based

- Physics simplicity is desirable

- Server load must be minimized

- Predictability matters more than precision

Again, this is a tradeoff, not a flaw.

---

## The Hidden Problem: Silent Misconfiguration

Many physics complaints originate from:

- Misconfigured physics settings

- Multiple conflicting configuration sources

- Hard-coded fallbacks overriding operator intent

- Lack of diagnostic logging

Changing meshers does nothing to fix these problems.

---

## Operational Recommendation

Instead of repeating myths, operators should:

1. Define the region’s purpose

2. Choose a physics engine accordingly

3. Select a mesher that matches the content

4. Validate build quality

5. Tune physics deliberately

6. Monitor performance and logs

Any advice that skips these steps is incomplete.

---

## Final Statement

There is no single “correct” mesher.

There is only:

- Correct understanding

- Appropriate configuration

- Honest diagnostics

- Clear intent

Blanket statements like  
“ZOMG YOU MUST USE ubODEMeshmerizer”  
are not expertise. They are shortcuts.

This document exists to end that myth.



# Physics Decision Matrix

## Region Type vs Physics Engine vs Mesher

This matrix exists to replace blanket advice with **intent-driven selection**.

### Key Principle

There is **no universal best choice**.  
There is only a **best choice for a given operational purpose**.

---

## Decision Matrix

| Region Type                    | Primary Goal          | Physics Engine | Mesher                          | Rationale                                              |
| ------------------------------ | --------------------- | -------------- | ------------------------------- | ------------------------------------------------------ |
| **General-purpose region**     | Balanced interaction  | Bullet / ubODE | Meshmerizer                     | Predictable, lower overhead, tolerant of mixed content |
| **High-detail mesh build**     | Accurate collision    | Bullet / ubODE | ubODEMeshmerizer                | Better convex decomposition for complex mesh           |
| **Large VAR region (3×3+)**    | Performance stability | Bullet / ubODE | Meshmerizer                     | Physics cost scales badly with mesh complexity         |
| **Interior-only build**        | Predictability        | Bullet / ubODE | Meshmerizer                     | Wind and fine collision detail usually unnecessary     |
| **Low-gravity / Lunar region** | Physical realism      | Bullet / ubODE | Meshmerizer or ubODEMeshmerizer | Mesher choice secondary to gravity and timestep tuning |
| **Sandbox / Dev region**       | Build testing         | Bullet / ubODE | ubODEMeshmerizer                | Reveals bad collision early                            |
| **Scenic / non-interactive**   | Visual fidelity       | Bullet / ubODE | Meshmerizer                     | Physics accuracy irrelevant                            |
| **High-avatar concurrency**    | Stability             | Bullet / ubODE | Meshmerizer                     | Reduces physics churn under load                       |
| **Legacy prim-based builds**   | Compatibility         | Bullet / ubODE | Meshmerizer                     | Mesh precision provides no benefit                     |

---

## Notes That Matter

- Mesher choice **does not fix bad builds**

- Physics engine choice **does not fix bad configuration**

- Complex mesh + ubODEMeshmerizer on large regions can **destroy performance**

- Physics accuracy is meaningless if the simulator cannot keep up

---

# Physics Tuning Checklist

## What to Validate Before Blaming the Mesher

This checklist exists to prevent **cargo-cult debugging**.

Run it **before** changing physics engines or meshers.

---

## 1. Define the Region’s Purpose

Answer these explicitly:

- Is this region primarily interactive?

- Is accurate collision required?

- Is performance more important than precision?

- Is this a build, sandbox, or production region?

If you cannot answer these, you are guessing.

---

## 2. Validate Build Quality

Most “physics bugs” live here.

Check for:

- Excessive physics shapes

- Non-convex mesh set to “Prim”

- Overlapping physics geometry

- Bad LODs

- Decorative mesh incorrectly marked physical

No mesher fixes these.

---

## 3. Verify Physics Flags

Confirm:

- Objects intended to collide are actually physical

- Objects intended to be decorative are not

- Sculpties and mesh are used intentionally

- Vehicles and scripted objects are not abusing physics

---

## 4. Confirm Configuration Authority

Ensure:

- Only one authoritative OpenSim.ini is in use

- No conflicting includes override physics values

- No silent fallback is occurring

- Paths and values resolve as expected at startup

If config is ambiguous, behavior will be too.

---

## 5. Review Physics Engine Settings

Focus on:

- Physics timestep

- Max substeps

- Gravity

- Avatar movement parameters

- Vehicle parameters

Do **not** change everything at once.

---

## 6. Evaluate Simulator Health

Physics degrades when the simulator is unhealthy.

Check:

- CPU saturation

- Thread starvation

- Memory pressure

- GC churn

- Network jitter

A struggling simulator produces bad physics regardless of settings.

---

## 7. Only Then Evaluate Mesher Choice

Now and only now ask:

- Does collision accuracy actually need improvement?

- Is server capacity sufficient?

- Are we solving a real problem or chasing perception?

If the answer is unclear, do not switch.

---

## Red Flags That Indicate Misdiagnosis

- “Walking through walls” in laggy regions

- “Falling through floors” during high load

- “Vehicles feel wrong” with unstable FPS

- “Physics breaks randomly”

These are **systemic issues**, not mesher problems.

---

## Operational Rule

If switching meshers appears to “fix” the issue:

- Identify *why*

- Document the tradeoff

- Measure performance impact

- Do not assume correctness

Otherwise, you are just moving the failure.

---

## Closing Statement

Physics in OpenSim is an **engineering discipline**, not a checkbox.

The correct workflow is:

1. Intent

2. Configuration

3. Validation

4. Measurement

5. Adjustment

Anything else is superstition.
