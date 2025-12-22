# Physics Meshing in OpenSim

## Compatibility, Tradeoffs, and Operational Reality

---

## Purpose of This Document

This document clarifies persistent misconceptions regarding physics meshers in OpenSim, particularly the repeated claim that **ubODEMeshmerizer is required** for ubODE physics or that alternative meshers are incompatible.

The intent is to replace assumption-based guidance with **verifiable, code-backed facts**, and to document how physics behavior in OpenSim actually emerges from system interactions rather than single configuration choices.

This document does not prescribe a specific configuration.  
It describes observable behavior, architectural separation, and operational tradeoffs.

---

## Summary of Findings

- ubODE physics does **not** require ubODEMeshmerizer

- Meshmerizer is **compatible** with ubODE physics

- No code-level enforcement exists requiring a specific mesher

- Claims of incompatibility originate from documentation drift, not behavior

- Mesher choice affects collision fidelity and performance, not engine viability

---

## Physics Is a System, Not a Toggle

Physics behavior in OpenSim is the result of interactions between multiple components, including:

1. Physics engine

2. Mesher choice

3. Build quality and collision design

4. Region and estate configuration

5. Simulator performance and load

6. Network stability

7. Viewer-side interpretation

Changing any single component in isolation rarely resolves systemic issues.

---

## Architectural Separation of Concerns

### Physics Engine

The physics engine (e.g. ubODE) is responsible for:

- Simulation rules

- Constraint solving

- Gravity and motion

- Avatar and vehicle behavior

### Mesher

The mesher is responsible for:

- Generating collision shapes

- Translating geometry into physics representations

These components interact through defined interfaces.  
They are **not mutually dependent**.

If a mesher were required for engine operation, that dependency would be explicitly enforced in code. No such enforcement exists.

---

## Code-Level Evidence

- ubODE initializes independently of mesher selection

- Mesher implementations conform to a shared interface

- No runtime assertion or guard enforces ubODEMeshmerizer usage

- Meshmerizer operates successfully with ubODE across multiple OpenSim versions

From an architectural standpoint, this conclusively demonstrates compatibility.

If Meshmerizer were incompatible:

- ubODE would fail to initialize, or

- Physics would fail deterministically at startup

Neither behavior occurs.

---

## Documentation and Maintainer Record

Official OpenSim documentation describes meshers as **pluggable components**, not mandatory dependencies.

Statements describing ubODEMeshmerizer as “required” do not appear in:

- Code

- Enforced configuration logic

- Authoritative documentation

Such language appears primarily in:

- Legacy comments

- Forum posts

- Repeated second-hand advice

This indicates documentation drift rather than a behavioral change.

---

## Operational Observations

Across long-running OpenSim deployments:

- ubODE has been operated successfully with multiple meshers

- Mixed mesher usage across regions is common

- Mesher selection affects performance and collision fidelity, not correctness

Operational issues attributed to “wrong mesher choice” are frequently traced to:

- Poor build quality

- Physics misconfiguration

- Region load and scaling issues

- Simulator resource saturation

- Viewer-side misinterpretation

Changing meshers may alter symptoms without addressing root causes.

---

## What a Mesher Actually Does

A mesher generates a **collision representation** from object geometry.

It does **not**:

- Define physics rules

- Control gravity

- Control avatar movement logic

- Fix poor content

- Compensate for overloaded simulators

Its role is limited and specific.

---

## ubODEMeshmerizer vs Meshmerizer

### ubODEMeshmerizer

**Characteristics**

- More accurate collision meshes for complex geometry

- More consistent convex decomposition

- Reduced physics rebuild churn in some scenarios

**Tradeoffs**

- Higher CPU usage

- Higher memory usage

- Increased physics complexity

- Can amplify poor build practices rather than correct them

---

### Meshmerizer

**Characteristics**

- Simpler collision meshes

- Lower computational overhead

- Predictable behavior for prim-based or simple builds

**Tradeoffs**

- Less accurate collision for complex mesh

- Not appropriate for all mesh-heavy scenes

Neither mesher is universally correct.

---

## Why Blanket Mesher Advice Is Harmful

### Masks Root Causes

Most reported physics problems originate from:

- Non-optimized mesh

- Excessive physics shapes

- Incorrect physics flags

- Poor LOD design

- Overloaded regions

Switching meshers may change behavior without resolving the underlying issue.

---

### Increases Load Silently

ubODEMeshmerizer increases:

- CPU usage

- Physics step complexity

- Memory pressure

On large estates or VAR regions, this can degrade stability without clear indicators.

---

### Ignores Use-Case Diversity

Different region types have fundamentally different needs:

- Build regions

- Interior-only regions

- Low-gravity or lunar environments

- High-traffic social spaces

- Scenic, non-interactive regions

A single mesher choice for all cases is irrational.

---

## Correct Interpretation

- ubODEMeshmerizer provides higher-fidelity collision for complex geometry

- Meshmerizer provides simpler, lower-overhead collision

- Both are valid tools with explicit tradeoffs

- Neither is universally required

Mesher selection is an engineering decision, not a rule.

---

## Physics Decision Matrix

### Region Type vs Physics Engine vs Mesher

| Region Type                | Primary Goal          | Physics Engine | Mesher           | Rationale                           |
| -------------------------- | --------------------- | -------------- | ---------------- | ----------------------------------- |
| General-purpose region     | Balanced interaction  | Bullet / ubODE | Meshmerizer      | Predictable, lower overhead         |
| High-detail mesh build     | Accurate collision    | Bullet / ubODE | ubODEMeshmerizer | Better convex decomposition         |
| Large VAR region (3×3+)    | Performance stability | Bullet / ubODE | Meshmerizer      | Physics cost scales poorly          |
| Interior-only build        | Predictability        | Bullet / ubODE | Meshmerizer      | Wind and fine collision unnecessary |
| Low-gravity / Lunar region | Physical realism      | Bullet / ubODE | Either           | Mesher secondary to gravity tuning  |
| Sandbox / Dev region       | Build testing         | Bullet / ubODE | ubODEMeshmerizer | Exposes collision issues early      |
| Scenic / non-interactive   | Visual fidelity       | Bullet / ubODE | Meshmerizer      | Physics accuracy irrelevant         |
| High-avatar concurrency    | Stability             | Bullet / ubODE | Meshmerizer      | Reduces physics churn               |
| Legacy prim builds         | Compatibility         | Bullet / ubODE | Meshmerizer      | Mesh precision unnecessary          |

---

## Physics Tuning Checklist

### What to Validate Before Changing Meshers

1. **Define region purpose**  
   Interaction level, precision requirements, performance constraints.

2. **Validate build quality**  
   Physics shape usage, convexity, LODs, overlapping geometry.

3. **Verify physics flags**  
   Physical vs decorative intent, sculpt and mesh usage.

4. **Confirm configuration authority**  
   Single authoritative INI, no silent overrides, no fallback paths.

5. **Review physics engine settings**  
   Timestep, substeps, gravity, avatar and vehicle parameters.

6. **Evaluate simulator health**  
   CPU, memory, threading, GC behavior, network stability.

7. **Only then evaluate mesher choice**  
   Determine whether collision accuracy actually limits the experience.

---

## Red Flags of Misdiagnosis

- Walking through walls under load

- Falling through floors during lag

- Vehicles behaving inconsistently

- Intermittent or non-deterministic failures

These are systemic issues, not mesher failures.

---

## Documentation Correction Rationale

Updating comments and documentation to reflect actual behavior is corrective, not disruptive.

Such changes:

- Align documentation with code

- Reduce operator confusion

- Prevent cargo-cult configuration

- Improve long-term maintainability

---

## Conclusion

Physics mesher selection in OpenSim is a matter of **tradeoffs**, not compatibility constraints.

Any statement asserting a single required mesher is unsupported by:

- Code

- Documentation

- Operational evidence

Accurate documentation is essential to prevent misconfiguration and repeated troubleshooting driven by misinformation.

This document replaces assumption with verification.

## Observed in Production

### Real-World Validation of Mixed Mesher Usage

The configuration model described in this document is not theoretical.  
It reflects **established operational practice** on multiple long-running OpenSim grids that have deployed ubODE physics with **different meshers selected based on region purpose and performance constraints**.

The following grids provide publicly observable validation of this approach.

---

### **OSGrid**

OSGrid is the largest and longest-running public OpenSim grid, hosting a wide variety of content types, region sizes, and usage patterns.

Operational characteristics relevant to this document:

- ubODE physics has been deployed at scale

- Meshmerizer has been used successfully alongside ubODE

- Mesher choice varies based on region role and performance requirements

- No grid-wide requirement exists enforcing ubODEMeshmerizer usage

If Meshmerizer were incompatible with ubODE, OSGrid would exhibit systemic physics failure. It does not.

---

### **Metropolis**

Metropolis was an early adopter of Bullet-based physics and has historically operated regions with:

- Mixed content complexity

- Performance-sensitive deployments

- Mesher selection driven by practical constraints rather than blanket rules

This aligns directly with the decision-matrix approach documented here.

---

### **DigiWorldz**

DigiWorldz has emphasized large regions, stability, and performance predictability.

Operational patterns include:

- Avoidance of unnecessary physics overhead on large or scenic regions

- Selection of lighter collision models where precision is not required

- No enforced global mesher requirement tied to ubODE

Again, this reflects intentional tradeoff management rather than incompatibility.

---

## Significance of These Observations

These grids demonstrate that:

- ubODEMeshmerizer is **not required** for ubODE physics to function correctly

- Meshmerizer operates compatibly with ubODE in production

- Mesher choice is routinely treated as an **engineering decision**, not a rule

- Mixed mesher usage across regions is a normal and accepted practice

These deployments are:

- Long-running

- Publicly accessible

- Observable firsthand

They are not hypothetical examples or experimental configurations.

---

## Addressing Common Objections

**“That’s just your opinion.”**  
The configurations described here are observable on multiple active grids.

**“That setup isn’t used anywhere else.”**  
It is used on OSGrid, Metropolis, DigiWorldz, and numerous private grids.

**“If it worked, everyone would do it.”**  
Operational success does not generate forum posts. Failure does.

---

## Conclusion

The physics and meshing model described in this document reflects **real-world operational practice**, not a novel or grid-specific interpretation.

Documenting this reality aligns OpenSim configuration guidance with:

- Code behavior

- Production deployments

- Long-established operational experience

This subsection exists to ground the discussion in **verifiable facts**, not tradition or repetition.


### Verification Note

The operational observations listed above reflect publicly observable
grid behavior as of **March 2025**.

Grid configurations evolve over time. The purpose of this section is not
to freeze a specific setup indefinitely, but to document that mixed
mesher usage with ubODE physics has been deployed successfully in
production for extended periods without enforced incompatibility.

If future changes alter these practices, they should be evaluated as
new operational decisions rather than retroactive validation of earlier
misconceptions.



## Evidence Scope and Validation Boundaries

### What Is Proven, What Is Observable, and What Is Not Claimed

This document is based on **verifiable evidence**, not opinion or preference.  
To avoid misinterpretation, the scope of validation is stated explicitly below.

---

### Code-Level Validation (Definitive)

The strongest form of validation is source code behavior.

- ubODE physics does **not** enforce the use of ubODEMeshmerizer

- No runtime checks, assertions, or guards bind ubODE to a specific mesher

- Meshmerizer and ubODEMeshmerizer both implement the same meshing interface

- ubODE initializes and operates independently of mesher selection

If ubODEMeshmerizer were required or if Meshmerizer were incompatible, this dependency would be enforced in code. It is not.

This alone conclusively disproves claims of requirement or incompatibility.

---

### Operational Validation (Publicly Observable)

The mesher and physics combinations described in this document are not theoretical.

They have been deployed successfully on long-running, publicly accessible OpenSim grids, including:

- OSGrid

- Metropolis

- DigiWorldz

These grids have operated ubODE physics with mixed mesher usage based on region purpose and performance constraints.

If Meshmerizer were incompatible with ubODE:

- Regions would fail deterministically

- Physics initialization would error

- Systemic failures would be immediately visible at scale

Such failures have not occurred.

These deployments are observable firsthand and constitute real-world validation.

---

### Operational Reality (Why This Is Not Always Visible)

Stable configurations rarely generate public discussion.

Most public advice and forum content originates from troubleshooting scenarios, not from long-term stable operation. As a result:

- Successful mixed-mesher deployments are underreported

- Failure narratives are disproportionately visible

- Repeated advice drifts toward oversimplification

This survivorship bias explains how “recommended for some cases” gradually became misrepresented as “required”.

---

### Explicit Non-Claims

For clarity, this document does **not** claim that:

- All grids use the same mesher configuration

- ubODEMeshmerizer is inferior or discouraged

- Meshmerizer is universally preferable

- Production grids maintain static configurations indefinitely

- One configuration is correct for all use cases

The only claim made is narrow, specific, and verifiable:

> ubODEMeshmerizer is not required for ubODE physics, and Meshmerizer is compatible and has been used successfully in production.

---

### Configuration Evolution Disclaimer

Grid configurations evolve over time.

The examples referenced here demonstrate **compatibility and viability**, not permanence.  
Future configuration changes should be evaluated as new operational decisions, not as retroactive validation or invalidation of earlier guidance.

---

### Summary

- Claims of mandatory mesher requirements are unsupported by code

- Claims of incompatibility are contradicted by production operation

- Historical repetition does not override verifiable behavior

- This document reflects observed reality, not tradition

This section exists to ensure the documentation remains grounded in **evidence**, **scope clarity**, and **intellectual honesty**.
