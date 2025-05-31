using Cambistry.Charting;
using Cambistry;
using Cyvzn.Cpus.Power;
using System;

namespace Cyvzn.Cpus.Candling
{
    public abstract class CandleCpu : Cpu
    {
        public readonly PowerCpu PowerCpu;
        public readonly Trader LightTrader = null;
        public readonly Trader HkLiveTrader = null;
        internal Signal LiveColor = Signal.None;
        internal abstract void Init(Quote q, Plot p);
        internal abstract void In(Quote q, Plot p);
        protected void StartCandle(Plot p)
        {
            CurrentIndexer++;
            Working = new Candle(CurrentIndexer, p);
        }
        protected void StopCandle()
        {
            PowerCpu.In(Working.Clone());
            if (LastHk == null)
            {
                LastHk = new HaCandle(Working.Clone());
                PowerCpu.InHk(new HaCandle(Working.Clone()));
            }
            else
            {
                LastHk = new HaCandle(Working.Clone(), LastHk.Clone());
                PowerCpu.InHk(new HaCandle(Working.Clone(), LastHk.Clone()));
            }
        }
        protected void SetLiveWithPlotAndQuote(Plot p, Quote q)
        {
            LiveColor = Working.Body;
            LightTrader.In(LiveColor);
            LightTrader.In(q);
            PowerCpu.In(p);
            PowerCpu.In(q);
            if (LastHk != null)
            {
                var RtHk = new HaCandle(Working.Clone(), LastHk);

                HkLiveTrader.In(RtHk.Body);
                HkLiveTrader.In(q);
                PowerCpu.InHk(RtHk.Close);
            }
            
        }
        #region ctr
        protected Candle Working = null;
        private int CurrentIndexer = 0;
        private HaCandle LastHk = null;
        public CandleCpu(Frequency f, Symbol s, string prefix) : base(prefix, s, f)
        {
            PowerCpu = new PowerCpu(this); LightTrader = new Trader(Symbol);
            HkLiveTrader = new Trader(Symbol);
        }
        #endregion
    }
}