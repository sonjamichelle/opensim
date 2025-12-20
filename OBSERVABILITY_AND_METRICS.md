# OpenSim Observability and Metrics

## Practical Monitoring for Grid Stability and Early Warning

### Purpose of This Document

This document explains how to use OpenSim’s built-in monitoring and statistics
as part of a **real observability strategy**.

These metrics are NOT magic bullets.
They will NOT automatically fix lag, crashes, or bad content.

Used correctly, however, they can:

- Reveal long-term degradation
- Expose repeating failure patterns
- Provide early warning before users start pitchforking
- Reduce blind troubleshooting and guesswork

This document assumes:

- You are willing to collect data over time
- You understand that interpretation matters more than raw numbers
- You accept that monitoring is a supporting tool, not a solution by itself

---

## What OpenSim Metrics Actually Are

OpenSim exposes **low-level simulator signals**, not diagnoses.

Examples include:

- Frame timing irregularities
- Delayed simulator heartbeats
- Script execution pressure
- Physics load spikes
- Network throughput anomalies

These signals are:

- Noisy when viewed moment-to-moment
- Extremely useful when viewed as trends

Looking at a single spike usually means nothing.
Watching how spikes change over hours or days means everything.

---

## Why Most Admins Get This Wrong

Common mistakes include:

- Enabling monitoring and staring at numbers
- Expecting a red light that says “this is the problem”
- Treating metrics as a performance tweak instead of a diagnostic tool
- Turning things off or on without a baseline

Metrics do not answer questions.
They help you ask **better questions**.

---

## The Correct Mental Model

Think of metrics like medical vitals.

One high heart rate does not mean a diagnosis.
A rising resting heart rate over weeks absolutely does.

Your goal is to detect:

- Trends
- Repetition
- Correlation between events

Not instant explanations.

---

## OpenSim Metrics You Already Have

OpenSim provides several exposure points, including:

- Region monitoring watchdogs
- Web-based statistics endpoints
- Moving averages over frame execution

These provide:

- Timing information
- Liveness signals
- Coarse performance indicators

On their own, they are incomplete.
Their value comes from aggregation.

---

## Recommended Aggregation Stack

OpenSim does NOT provide dashboards or alerting.
You must bring your own tooling.

A proven, realistic stack is:

### Prometheus

- Pulls metrics on a schedule
- Stores time-series data efficiently
- Handles retention and downsampling

### Grafana

- Visualizes trends
- Compares regions, estates, or time windows
- Provides alerts based on thresholds and rate-of-change

Typical flow:

OpenSim
→ exposed stats or logs
→ Prometheus
→ Grafana dashboards and alerts

---

## What You Should Be Watching For

Do NOT obsess over absolute numbers.

Watch for:

- Gradual increases in frame delay
- Repeating stalls at the same time of day
- Regions that degrade faster than others
- Correlation between script load and physics load
- Spikes that precede region restarts or freezes

These patterns tell you:

- Where to look
- When to intervene
- Whether a problem is content, load, or infrastructure

---

## Example Early Warning Scenarios

### Slow Death Regions

A region runs fine for hours, then degrades until restart.
Metrics often show:

- Increasing frame variance
- Growing GC pauses
- Script execution backlogs

This points to:

- Script leaks
- Physics accumulation
- Content churn problems

### “Lag Complaints but No Crash”

Users complain but the region stays up.
Metrics may show:

- Network packet delays
- Interest management saturation
- Viewer update throttling

This shifts focus away from scripts and physics.

### Sudden Meltdowns

Everything dies at once.
Metrics often reveal:

- Host-level contention
- IO saturation
- Shared service failures

This prevents blaming the wrong region.

---

## Alerting Philosophy

Alerts should be:

- Based on trends, not spikes
- Rate-of-change oriented
- Tuned conservatively

Bad alerts create noise.
Good alerts buy you time.

Example:

- “Frame delay increasing steadily for 20 minutes”
  is actionable.
- “Frame delay exceeded threshold once”
  is usually useless.

---

## What This Will NOT Do

Monitoring will NOT:

- Fix bad scripts
- Fix broken physics meshes
- Replace profiling
- Prevent all outages

It will:

- Reduce blind panic
- Provide evidence during troubleshooting
- Help you justify changes
- Protect your sanity when users get loud

---

## Operator Reality Check

You can run a grid without observability.
Many do.

But when something goes wrong:

- You will guess
- You will restart blindly
- You will chase symptoms

Observability replaces guesswork with context.

---

## Final Advice

If you enable metrics:

- Commit to learning what they mean
- Collect data before you need it
- Build dashboards before users complain

These tools do not make you immune to failure.
They give you leverage when failure starts.

That alone is worth the effort.
