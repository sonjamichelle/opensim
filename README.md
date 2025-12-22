
# IMPORTANT: About This Fork

This repository is a **purpose-driven fork** created to support the practical operation of a specific OpenSim grid and to reduce long-standing administrative and cognitive overhead encountered during real-world use.

This fork does **not** exist to replace upstream OpenSim, redefine best practices for everyone, or mandate changes outside its intended scope.

It exists to make OpenSim **operable, predictable, and maintainable** for the environments in which it is actually run here.

---

## Why This Fork Exists

This fork was created after repeated encounters with:

- Ambiguous or misleading configuration behavior
- Documentation that does not reflect actual code behavior
- Silent fallbacks that hide misconfiguration
- Legacy comments and defaults that contradict runtime reality
- Administrative friction that consumes time and attention unnecessarily

While upstream OpenSim functions, its configuration surface and documentation often rely on historical convention, assumption, or repetition rather than explicit intent and validation.

For operators running persistent grids, this ambiguity becomes a recurring operational burden.

Forking was chosen as a **last resort**, not a preference.

---

## Scope and Intent

Changes in this repository are made with the following intent:

- Reduce ambiguity in configuration and behavior
- Make documentation match actual runtime behavior
- Treat configuration and logging as operator-facing contracts
- Favor explicit validation over silent fallback
- Improve maintainability for long-running grid operation

These changes are **targeted** to the needs of this deployment.

They are not designed to satisfy every possible use case.

---

## Non-Goals

This fork explicitly does **not** aim to:

- Enforce a universal configuration model
- Dictate how other grids must operate
- Replace upstream governance or design decisions
- Maintain strict compatibility with all third-party assumptions
- Serve as a drop-in replacement for all OpenSim deployments

Upstream OpenSim remains the correct choice for many users.

This fork exists because those choices were insufficient for this environment.

---

## About Public Availability

This repository is public for transparency and inspection.

If others find the changes useful and choose to adopt or adapt them, that is welcome but incidental. No obligation of support, endorsement, or long-term compatibility is implied.

This fork primarily serves its original purpose:  
to reduce operational friction and ambiguity for its maintainers.

---

## How to Read This Repository

When evaluating changes in this fork, assume:

- Changes are intentional and scoped
- Documentation reflects observed behavior
- Configuration examples prioritize clarity over legacy compatibility
- Comments and defaults may differ from upstream to reduce ambiguity

If a change appears opinionated, it is because it resolves a specific operational problem encountered here.

---

If you are looking for the upstream project, please refer to the original OpenSim repository.

<<<<<<<<<  Original README follows >>>>>>>>>>>




Welcome to OpenSimulator (OpenSim for short)!

# Overview

OpenSim is a BSD Licensed Open Source project to develop a functioning
virtual worlds server platform capable of supporting multiple clients
and servers in a heterogeneous grid structure. OpenSim is written in
C#, and can run under Mono or the Microsoft .NET runtimes.

This is considered an alpha release.  Some stuff works, a lot doesn't.
If it breaks, you get to keep *both* pieces.

# Compiling OpenSim

Please see BUILDING.md

# Running OpenSim on Windows

You will need dotnet 8.0 runtime (https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

To run OpenSim from a command prompt

* cd to the bin/ directory where you unpacked OpenSim
* review and change configuration files (.ini) for your needs. see the "Configuring OpenSim" section
* run OpenSim.exe

# Running OpenSim on Linux/Mac

You will need

* [dotnet 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

* libgdiplus 
  
  if you have mono 6.x complete, you already have libgdiplus, otherwise you need to install it
  using a package manager for your operating system, like apt, brew, macports, etc
  for example on debian:
  
  `apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev`

To run OpenSim, from the unpacked distribution type:

* cd bin
* review and change configuration files (.ini) for your needs. see the "Configuring OpenSim" section
* run ./opensim.sh

# Configuring OpenSim

When OpenSim starts for the first time, you will be prompted with a
series of questions that look something like:

    [09-17 03:54:40] DEFAULT REGION CONFIG: Simulator Name [OpenSim Test]:

For all the options except simulator name, you can safely hit enter to accept
the default if you want to connect using a client on the same machine or over
your local network.

You will then be asked "Do you wish to join an existing estate?".  If you're
starting OpenSim for the first time then answer no (which is the default) and
provide an estate name.

Shortly afterwards, you will then be asked to enter an estate owner first name,
last name, password and e-mail (which can be left blank).  Do not forget these
details, since initially only this account will be able to manage your region
in-world.  You can also use these details to perform your first login.

Once you are presented with a prompt that looks like:

    Region (My region name) #

You have successfully started OpenSim.

If you want to create another user account to login rather than the estate
account, then type "create user" on the OpenSim console and follow the prompts.

Helpful resources:

* http://opensimulator.org/wiki/Configuration
* http://opensimulator.org/wiki/Configuring_Regions

# Connecting to your OpenSim

By default your sim will be available for login on port 9000.  You can login by
adding -loginuri http://127.0.0.1:9000 to the command that starts Second Life
(e.g. in the Target: box of the client icon properties on Windows).  You can
also login using the network IP address of the machine running OpenSim (e.g.
http://192.168.1.2:9000)

To login, use the avatar details that you gave for your estate ownership or the
one you set up using the "create user" command.

# Bug reports

In the very likely event of bugs biting you (err, your OpenSim) we
encourage you to see whether the problem has already been reported on
the [OpenSim mantis system](http://opensimulator.org/mantis/main_page.php).

If your bug has already been reported, you might want to add to the
bug description and supply additional information.

If your bug has not been reported yet, file a bug report ("opening a
mantis"). Useful information to include:

* description of what went wrong
* stack trace
* OpenSim.log (attach as file)
* OpenSim.ini (attach as file)

# More Information on OpenSim

More extensive information on building, running, and configuring
OpenSim, as well as how to report bugs, and participate in the OpenSim
project can always be found at http://opensimulator.org.

Thanks for trying OpenSim, we hope it is a pleasant experience.
