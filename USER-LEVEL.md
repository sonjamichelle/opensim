;; ------------------------------------------------------------------
;; USERLEVEL CONVENTIONS  (GRID AUTHORITY AND TRUST MODEL)
;; ------------------------------------------------------------------
;;
;; UserLevel is a FIRST-CLASS database field.
;; It exists to represent trust, authority, and role within the grid.
;;
;; It is NOT legacy.
;; It is NOT deprecated.
;; It is UNDERUTILIZED.
;;
;; OpenSim provides the storage and comparison mechanics.
;; Meaning and policy are the responsibility of the grid.
;;
;; VergeGrid explicitly adopts UserLevel as a core governance primitive.
;;

;; --------------------------------------------------
;; USERLEVEL RANGES (VERGEGRID STANDARD)
;; --------------------------------------------------
;;
;;   0
;;   ── Normal Users
;;      • Default residents
;;      • Full creative participation (subject to policy)
;;
;;   1–49
;;   ── New / Probationary Users
;;      • Optional restrictions
;;      • Anti-spam / anti-grief window
;;      • Time-on-grid or trust-building phase
;;
;;   50–99
;;   ── Established / Trusted Users
;;      • Long-term residents
;;      • Reduced restrictions
;;
;;   100–149
;;   ── Mentors / Guides
;;      • User-facing help roles
;;      • NO god powers
;;      • NO infrastructure authority
;;
;;   150–199
;;   ── Staff / Moderators
;;      • Limited administrative capabilities
;;      • Enforcement and support roles
;;      • Still NOT gods
;;
;;   200–239
;;   ── Senior Staff / Operators
;;      • Elevated operational trust
;;      • Still explicitly BELOW god threshold
;;      • Useful for “almost admin” roles
;;
;;   240–249
;;   ── Grid Administrators (GODS)
;;      • Eligible for god powers
;;      • Subject to allow_grid_gods
;;      • Infrastructure and region authority
;;
;;   250
;;   ── Founders / Root Authority
;;      • Absolute grid authority
;;      • Extremely limited assignment
;;      • Reserved for grid ownership
;;
;; --------------------------------------------------
;; GOD THRESHOLD POLICY
;; --------------------------------------------------
;;
;; VergeGrid defines GOD eligibility as:
;;
;;   UserLevel >= 240
;;
;; This decouples:
;;   • High trust roles
;;   • Operational staff
;;   • True god authority
;;
;; and prevents casual privilege escalation.
;;

;; --------------------------------------------------
;; DESIGN PHILOSOPHY
;; --------------------------------------------------
;;
;; UserLevel SHOULD be used to:
;;   • Gate sensitive capabilities
;;   • Represent earned trust
;;   • Encode long-term roles
;;
;; UserLevel SHOULD NOT be used as:
;;   • A temporary hack
;;   • An afterthought
;;   • Something reimplemented externally
;;
;; Recreating this system outside the DB duplicates effort
;; and increases complexity without adding capability.
;;


;; ------------------------------------------------------------------
;; USERLEVEL INTEGRATION  (WEBUI / EXTERNAL SERVICES)
;; ------------------------------------------------------------------
;;
;; UserLevel is intended to be consumed by EXTERNAL SERVICES,
;; including the VergeGrid WebUI.
;;
;; VERIFIED:
;;   • UserLevel is stored in the database
;;   • UserLevel is reliably queryable
;;   • UserLevel can be used for access decisions
;;
;; VergeGrid explicitly treats UserLevel as:
;;   • An AUTHORIZATION primitive
;;   • A TRUST indicator
;;   • A ROLE boundary
;;
;; --------------------------------------------------
;; WEBUI INTENDED USAGE
;; --------------------------------------------------
;;
;; UserLevel will be used to control access to:
;;   • Administrative dashboards
;;   • Moderation tools
;;   • Estate / region management
;;   • Grid-wide control features
;;
;; EXAMPLES:
;;   • UserLevel >= 100  -> Mentor tools visible
;;   • UserLevel >= 150  -> Moderator panels enabled
;;   • UserLevel >= 200  -> Operational admin features
;;   • UserLevel >= 240  -> God-level controls
;;
;; ACCESS CONTROL MODEL:
;;   • Deny-by-default
;;   • Explicit UserLevel thresholds per feature
;;   • No UI exposure without sufficient authority
;;
;; --------------------------------------------------
;; DESIGN GOAL
;; --------------------------------------------------
;;
;; Use ONE shared authority field (UserLevel)
;; across:
;;   • Simulator
;;   • Services
;;   • WebUI
;;
;; DO NOT:
;;   • Re-encode roles separately per system
;;   • Duplicate permission logic in parallel tables
;;   • Create shadow role systems
;;
;; Single source of truth reduces complexity and errors.
;;
;; SECURITY NOTE:
;; UserLevel-based access MUST be enforced server-side.
;; UI-only restrictions are insufficient.
