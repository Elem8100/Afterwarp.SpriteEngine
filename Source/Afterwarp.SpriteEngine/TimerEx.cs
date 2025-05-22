using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Afterwarp.SpriteEngine;

public class TimerEx
{
    long frequency = Stopwatch.Frequency;
    long previousTicks = Stopwatch.GetTimestamp();
    public void OnTimer(int Interval, Action Action)
    {
        long currentTicks = Stopwatch.GetTimestamp();
        long elapsedTicks = currentTicks - previousTicks;
        double elapsedMs = (elapsedTicks * 1000.0) / frequency;
        if (elapsedMs >= Interval)
        {
            Action();
            previousTicks = currentTicks;
        }
    }

}
