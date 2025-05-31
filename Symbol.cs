using Cambistry;
using System;
using System.Collections.Generic;

namespace Cyvzn
{
    public class Symbol : Market
    {
        public bool RealTime { get; set; } = false;
        public readonly List<Frequency> Frequencies = new List<Frequency>();
        internal Symbol(ApiSetting.ApiMarket m) : base(m.Name)
        {
            foreach (var c in m.ApiCharts)
            {
                Frequencies.Add(new Frequency(c, this));
            }
            Hk60m = new Trader(this);
        }
        public Quote Quote { get; private set; } = new Quote(0.0d, 0.0d, DateTime.MinValue);
        internal void In(Quote q)
        {
            Quote = q;
            var p = new Plot(q);
            if (HasInitQuote)
            {
                if (q.Time.Ticks > LastQuoteTicks) { foreach (var f in Frequencies) { f.MyCandleCpu.In(q, p); } }
                else { throw new Exception(Name + " Quote Sequencing Error"); }
            }
            else
            {
                foreach (var f in Frequencies) { f.MyCandleCpu.Init(q, p); }
                HasInitQuote = true;
            }
            LastQuoteTicks = q.Time.Ticks;
            InTrader(q);
            Statistics.OnQuoteReceived(this, q);
        }
        private void InTrader(Quote q)
        {
            foreach (var f in Frequencies)
            {
                if (f.Name == "60m")
                {
                    var s1 = f.MyCandleCpu.PowerCpu.HkLine.Get(out OpenTrade st1);
                    var s2 = f.MyCandleCpu.HkLiveTrader.Get(out OpenTrade st2);
                    if (s1 == s2)
                    {
                        Hk60m.In(s1);
                    }
                    else { Hk60m.In(Signal.None); }
                }
            }
            Hk60m.In(q);
        }
        private long LastQuoteTicks = DateTime.MinValue.Ticks;
        private bool HasInitQuote = false;
        public readonly Trader Hk60m;
    }
}
