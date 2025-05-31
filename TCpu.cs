using Cambistry;
using System;

namespace Cyvzn.Cpus.Candling
{
    public class TCpu : CandleCpu
    {
        internal TCpu(Frequency f, Symbol s, string prefix) : base(f, s, prefix) { }
        private DateTime Timing;
        private void ResetTiming(Plot p)
        {
            var mySeconds = Frequency.CpuInterval;
            var _ = p.Time;
            Timing = _.AddTicks(-(_.Ticks % (TimeSpan.TicksPerSecond * mySeconds))).AddSeconds(mySeconds).AddTicks(-1);
        }
        internal override void Init(Quote q, Plot p)
        {
            StartCandle(p);
            ResetTiming(p);
        }
        internal override void In(Quote q, Plot p)
        {
            if (!TryUpdate()) { StopCandle(); StartCandle(p); ResetTiming(p); }
            bool TryUpdate()
            {
                if (p.Time > Timing) return false;
                Working.In(p);
                return true;
            }
            SetLiveWithPlotAndQuote(p, q);
        }
    }
}
