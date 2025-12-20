# NPC USE CASES AND OPERATIONAL GUIDANCE

## OpenSim Non-Player Characters (NPCs)

---

## 1. PURPOSE OF THIS DOCUMENT

This document exists to correct widespread misunderstanding of OpenSim NPCs and to provide **practical, operational guidance** for grid operators, estate managers, and developers.

NPCs are frequently:

- Disabled out of fear
- Enabled without limits
- Blamed for performance problems they did not cause
- Dismissed as griefing or stat-padding tools

All of the above are administrative failures, not NPC failures.

This document explains:

- What NPCs actually are
- What they cost
- When they are appropriate
- When they are not
- How to deploy them responsibly

---

## 2. WHAT NPCs ACTUALLY ARE

An NPC is a **server-side simulated avatar**.

NPCs:

- Exist entirely on the simulator
- Do not require a viewer connection
- Participate in physics
- Participate in interest management
- Generate and receive events
- Can move, sense, interact, and act

NPCs are **not bots** in the MMO sense.
They are **headless avatars**.

Operational consequence:

> Treat NPCs exactly like real avatars when planning capacity.

---

## 3. WHAT NPCs ARE NOT

NPCs are not:

- A griefing system by default
- A statistics manipulation trick
- A viewer exploit
- A lightweight background decoration
- A replacement for proper AI systems

NPC abuse comes from **scripts and permissions**, not the NPC subsystem.

---

## 4. PERFORMANCE REALITY CHECK

### 4.1 Cost Model

Rule of thumb:

| Entity Type            | Relative Cost |
| ---------------------- | ------------- |
| Real avatar            | 1.0           |
| NPC                    | 0.8–1.0       |
| Simple scripted object | 0.05–0.2      |

NPCs:

- Consume CPU
- Consume physics cycles
- Increase update traffic
- Increase script scheduling pressure

NPCs are **not cheap**.

---

### 4.2 What Happens If You Overuse NPCs

Excessive NPCs will cause:

- Increased frame time
- Delayed script execution
- Physics instability
- Avatar movement jitter
- Misattributed “lag” complaints

Symptoms will often be blamed on:

- Physics engine
- Viewers
- Network
- “OpenSim is slow”

The real cause will be **population mismanagement**.

---

## 5. LEGITIMATE USE CASES

### 5.1 Training and Simulation

Proven deployments include:

- Emergency response training
- Military and defense simulations
- Law enforcement scenario rehearsal
- Crowd behavior modeling
- Stress exposure training

NPCs are used to:

- Create human presence
- Trigger reactions
- Force decision making
- Simulate unpredictability

MOSES demonstrated this at scale.

---

### 5.2 Education

NPCs can function as:

- Guided tour hosts
- Instructors
- Assistants
- Demonstration actors

Use cases:

- Museums
- Universities
- Remote learning environments
- Technical training labs

---

### 5.3 Immersive World Building

NPCs can populate:

- Cities
- Shops
- Stations
- Venues
- Staffed environments

NPCs make regions feel:

- Lived in
- Purposeful
- Less empty during off hours

This improves **first impression retention**.

---

### 5.4 Load and Behavior Testing

NPCs are ideal for:

- Stress testing regions
- Testing interest management behavior
- Script load testing
- Physics tuning validation

NPCs allow controlled testing **without recruiting humans**.

---

## 6. HIGH-RISK USE CASES

NPCs become dangerous when:

- Unlimited NPC creation is allowed
- Ownership rules are lax
- NPC cloning is unrestricted
- NPCs are indistinguishable from users
- No per-scene limits exist

These scenarios lead to:

- Abuse
- Confusion
- Impersonation
- Performance collapse

NPCs require **policy**, not blind enablement.

---

## 7. SECURITY AND POLICY CONSIDERATIONS

### 7.1 Ownership

Operators must decide:

- Who can create NPCs
- Who owns them
- Who can delete them

Best practice:

- Restrict NPC creation to trusted roles
- Avoid public NPC creation permissions

---

### 7.2 Cloning

NPC cloning of avatars is powerful but dangerous.

Risks:

- Impersonation
- User confusion
- Trust erosion

Recommendation:

- Disable cloning unless required
- Clearly mark NPCs visually if enabled

---

### 7.3 Identification

NPCs should be identifiable:

- Group titles
- Name conventions
- Visual indicators

Hidden NPCs are acceptable **only** in controlled simulations.

---

## 8. OPERATIONAL BEST PRACTICES

### 8.1 Limits

Always set:

- Max NPCs per scene
- Script rate limits
- Ownership controls

Never:

- Allow unlimited NPCs on production regions

---

### 8.2 Monitoring

NPC usage should be monitored alongside:

- Avatar count
- Script time
- Physics FPS
- Frame time

NPCs are not “fire and forget”.

---

### 8.3 Documentation

If NPCs are enabled:

- Document why
- Document who controls them
- Document expected behavior

Admins rotating in later must understand intent.

---

## 9. COMMON FAILURE PATTERNS

| Failure              | Cause                          |
| -------------------- | ------------------------------ |
| NPC blamed for lag   | No population limits           |
| NPC impersonation    | Cloning enabled without policy |
| Empty regions        | NPCs disabled unnecessarily    |
| User confusion       | NPCs not identifiable          |
| Performance collapse | NPCs treated as lightweight    |

---

## 10. SUMMARY

NPCs are:

- Powerful
- Proven
- Expensive
- Dangerous if misused

NPCs are not:

- Toys
- Decorations
- Grief tools by default

NPCs require:

- Intent
- Policy
- Limits
- Monitoring

Used correctly, NPCs enable experiences **no other OpenSim feature can provide**.

Used incorrectly, NPCs become an easy scapegoat for administrative failure.

---

## 11. FINAL OPERATOR GUIDANCE

If you do not understand NPCs:

- Do not disable them out of fear
- Do not enable them blindly

Learn first.
Plan second.
Deploy intentionally.

NPCs are a scalpel, not a hammer.

## 12. NPCs AND THE EVOLUTION OF AI-DRIVEN INTERACTION

### 12.1 HISTORICAL LIMITATION

Historically, NPCs in OpenSim have been perceived as:

- Scripted mannequins
- Path-following actors
- Trigger-based responders
- Finite-state machines

This limitation was not architectural.
It was a function of:

- Script complexity limits
- Lack of external intelligence integration
- Event-driven scripting constraints
- No practical conversational backend

NPCs were never the limiting factor.
The tooling was.

### 12.2 MODERN REALITY

With the maturation of:

- External AI services
- Natural language processing APIs
- State-aware conversational models
- Context retention systems

NPCs can now evolve into **fully interactive, adaptive characters**.

NPC behavior is no longer limited to:

- Hardcoded dialog trees
- Static responses
- Predefined animation loops

NPCs can now:

- Interpret free-form user input
- Maintain conversational context
- React dynamically to environment state
- Adapt behavior based on role, location, or scenario
- Serve as intelligent intermediaries between users and systems

---

### 12.3 WHAT ENABLES THIS SHIFT

AI-augmented NPCs require **no core OpenSim changes**.

They are enabled by:

- Scripted API access (HTTP/HTTPS)
- External AI inference services
- Proper rate limiting and caching
- Intentional dialogue and behavior design

The NPC remains:

- A server-side avatar
- A physics participant
- A script-driven entity

The intelligence is external.
The embodiment is OpenSim-native.

---

### 12.4 PRACTICAL USE CASE EXPANSION

AI-integrated NPCs unlock new categories of use:

- Interactive instructors and tutors
- Adaptive training role-players
- Dynamic quest givers and narrative agents
- Customer support or concierge avatars
- Simulation actors with evolving behavior
- Information brokers and world guides
- Accessibility assistants

The NPC becomes:

> An interface, not just an object.

---

### 12.5 OPERATIONAL CONSTRAINTS (READ THIS)

AI-driven NPCs introduce **new responsibilities**.

They require:

- Network reliability
- Latency tolerance
- API key management
- Cost controls
- Rate limiting
- Failure handling
- Privacy consideration

Failure modes include:

- Silent NPCs when APIs are unreachable
- Delayed responses under load
- Increased script execution time
- External dependency outages

AI integration does **not** reduce operational complexity.
It shifts it.

---

### 12.6 PERFORMANCE AND COST REALITY

AI does not replace NPC cost.
It adds to it.

Consider:

- NPC baseline cost (avatar-equivalent)
- Script scheduling overhead
- Network round-trips
- External API billing
- Token or request limits

AI-enhanced NPCs must be:

- Fewer
- Purpose-driven
- Carefully scoped

They are **not background decoration**.

---

### 12.7 SECURITY AND POLICY CONSIDERATIONS

AI-driven NPCs may:

- Receive user-generated input
- Generate unvetted output
- Access external systems

Operators must consider:

- Content moderation
- Prompt injection risks
- Data leakage
- Logging and auditing
- Compliance with regional policy

NPC intelligence is only as safe as the systems behind it.

---

### 12.8 IMPORTANT CLARIFICATION

AI does not make NPCs mandatory.
AI does not make NPCs universally better.

Many environments benefit from:

- Static NPCs
- Scripted behavior
- Predictable interaction

AI is an **option**, not an obligation.

---

### 12.9 SUMMARY

Modern AI does not change what NPCs are.
It expands what they can become.

NPCs are no longer limited by:

- Script language
- Dialogue size
- Predefined responses

They are limited by:

- Creator intent
- Script design
- Infrastructure
- Network access
- AI model choice
- Cost tolerance

The NPC subsystem is ready.
The rest is an engineering decision.

---

TODO:



- `AI-NPC-ARCHITECTURE.md`

- `AI-NPC-FAILURE-MODES.md`

- `AI-NPC-COST-AND-RATE-LIMITING.md`

- `AI-NPC-POLICY-AND-SAFETY.md`
