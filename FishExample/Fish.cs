using System;
using System.Collections.Generic;
using System.Text;

namespace FishMonitoringConsole
{
    public abstract class Fish
    {
        public string name;
        public Quality quality;

        public Fish(Quality q)
        {
            quality = q;
        }
        public abstract bool isValid();
    }

    public class FrozenFish : Fish
    {
        public double maxStoreTemp;
        public TimeSpan deathTime;

        public FrozenFish(string n, Quality q, double t, TimeSpan d) : base(q)
        {
            name = n;
            maxStoreTemp = t;
            deathTime = d;
        }

        public override bool isValid()
        {
            return !((quality as TempQuality).GetTempUpperTime(maxStoreTemp) > deathTime);
        }
    }
    public class ChilledFish : Fish
    {
        public double minStoreTemp;
        public double maxStoreTemp;
        public TimeSpan minDeathTime;
        public TimeSpan maxDeathTime;

        public ChilledFish(string name, Quality q, double t, TimeSpan d) : base(q)
        { }

        public override bool isValid()
        {
            return !((quality as TempQuality).GetTempUpperTime(maxStoreTemp) > maxDeathTime
            || (quality as TempQuality).GetTempLowerTime(maxStoreTemp) < minDeathTime);
        }
    }
}
