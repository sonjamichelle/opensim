// ============================================================================
// Animesh AI Greeter Base (Modern OpenSim)
// Derived from v7D_Advanced_Visitor_Greeter by Void Singer
//
// Original work:
//   (C)2009 CC-BY Void Singer
//   https://wiki.secondlife.com/wiki/User:Void_Singer
//
// Modified for OpenSim, Mono, and Animesh use
// (C)2025 Sonja & Nikki
//
// ============================================================================
// PURPOSE:
//   Base greeter framework with avatar detection, familiarity tracking,
//   grace-period memory, and decay logic.
//
//   This script is intentionally NON-AI.
//   AI integration will be layered on top via llHTTPRequest.
//
// ============================================================================

integer SENSOR_RANGE = 96;
float   SENSOR_ARC   = PI;
float   SENSOR_RATE  = 30.0;

// Familiarity decay
integer GRACE_PERIOD = 14400;   // 4 hours (short return grace)
integer DECAY_PERIOD = 172800;  // 48 hours (forget details)

// Internal tracking
list gAvatars;     // avatar UUIDs
list gLastSeen;    // unix timestamps
integer gMaxStored = 512;

// Utility: find avatar index
integer indexOfAvatar(key av)
{
    return llListFindList(gAvatars, [av]);
}

// Utility: greet avatar (placeholder for AI later)
greetAvatar(key av)
{
    string name = llKey2Name(av);
    llRegionSayTo(av, 0, "Oh! Hello there, " + name + ".");
}

// Cull expired entries
cullAvatars()
{
    integer now = llGetUnixTime();
    integer len = llGetListLength(gAvatars);
    integer i;

    for (i = len - 1; i >= 0; --i)
    {
        if (now - llList2Integer(gLastSeen, i) > DECAY_PERIOD)
        {
            gAvatars  = llDeleteSubList(gAvatars, i, i);
            gLastSeen = llDeleteSubList(gLastSeen, i, i);
        }
    }
}

default
{
    state_entry()
    {
        llSensor("", NULL_KEY, AGENT, SENSOR_RANGE, SENSOR_ARC);
        llSetTimerEvent(SENSOR_RATE);
    }

    timer()
    {
        llSensor("", NULL_KEY, AGENT, SENSOR_RANGE, SENSOR_ARC);
    }

    sensor(integer total)
    {
        integer now = llGetUnixTime();
        integer i;

        for (i = 0; i < total; ++i)
        {
            key av = llDetectedKey(i);
            integer idx = indexOfAvatar(av);

            if (idx == -1)
            {
                // First time seen
                greetAvatar(av);

                gAvatars  = [av] + gAvatars;
                gLastSeen = [now] + gLastSeen;

                // Enforce max storage
                if (llGetListLength(gAvatars) > gMaxStored)
                {
                    gAvatars  = llDeleteSubList(gAvatars, gMaxStored, -1);
                    gLastSeen = llDeleteSubList(gLastSeen, gMaxStored, -1);
                }
            }
            else
            {
                // Update last-seen timestamp
                gLastSeen = llListReplaceList(gLastSeen, [now], idx, idx);
            }
        }

        cullAvatars();
    }
}
