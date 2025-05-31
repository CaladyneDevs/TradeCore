using Cyvzn.Cambistry;
using System;

namespace Cambistry
{
    public class Trade
    {
        public bool Closed { get; protected set; } = false;
        public TradeStat EntryPower = new TradeStat();
        public TradeStat MaxPower = new TradeStat();
        public TradeStat ExitPower = new TradeStat();
        protected Trade(Cyvzn.Symbol symbol, Signal order, Quote entry)
        {
            Symbol = symbol;   
            StandardFluc = symbol.StandardFluctuation;

            switch (order)
            {
                case Signal.Up:
                case Signal.Down:
                    Order = order;
                    break;
                default:
                    throw new Exception("MOD Trade None");
            }

            Entry = entry;
            MaxProfit = entry;
            MaxPullback = entry;
            MaxRisk = entry;
            MaxRiskAtMaxProfit = entry;
            Exit = entry;
        }

        protected readonly double StandardFluc;
        public readonly Signal Order = Signal.None;
        public readonly Quote Entry;
        public Quote MaxProfit { get; protected set; } public Quote MaxPullback { get; protected set; }
        public Quote MaxRisk { get; protected set; }
        public Quote MaxRiskAtMaxProfit { get; protected set; }
        public Quote Exit { get; protected set; }
        public readonly Cyvzn.Symbol Symbol;
        public double EntryPrice { get { switch (Order) {  case Signal.Down:return Entry.Bid; case Signal.Up:return Entry.Ask; default: return 0.0d; } } }
        public double ExitPrice { get { switch (Order) { case Signal.Down: return Exit.Ask; case Signal.Up: return Entry.Bid; default: return 0.0d; } } }

        public string GetEntryTimeFormatted()
        {
            return GetTimeFormatted(Entry.Time);
        }
        public string GetExitTimeFormatted()
        {
            return GetTimeFormatted(Exit.Time);
        }
        private string GetTimeFormatted(DateTime dt)
        {
            return dt.ToString("MM-dd hh:mm tt");
        }
        public double GetPower()
        {
            var max = GetMaxProfitPoints();
            if (max > 0.0d) { return max + GetMaxPullbackPoints(); }
            return 0.0d;
        }
        public double GetPerformance()
        {
            var mx = GetMaxProfitPoints();
            var pl = GetPLPoints();
            if (mx > 0.0d && pl > 0.0d)
            {
                return pl / mx * 100;
            }
            return 0;
        }
        public double GetPLPoints()
        {
            switch (Order)
            {
                case Signal.Up:
                    return (Exit.Bid - Entry.Ask) / StandardFluc;
                case Signal.Down:
                    return (Entry.Bid - Exit.Ask) / StandardFluc;
                default:
                    return 0.0d;
            }
        }
        public double GetPullbackPoints()
        {
            switch (Order)
            {
                case Signal.Up:
                    return (MaxProfit.Bid - Exit.Bid) / StandardFluc * -1;
                case Signal.Down:
                    return (Exit.Ask - MaxProfit.Ask) / StandardFluc * -1;
                default:
                    return 0.0d;
            }
        }
        public double GetMaxPullbackPoints()
        {
            switch (Order)
            {
                case Signal.Up:
                    return (MaxProfit.Bid - MaxPullback.Bid) / StandardFluc * -1;
                case Signal.Down:
                    return (MaxPullback.Ask - MaxProfit.Ask) / StandardFluc * -1;
                default:
                    return 0.0d;
            }
        }
        public double GetMaxRiskAtMaxProfitPoints()
        {
            switch (Order)
            {
                case Signal.Up:
                    return (MaxRiskAtMaxProfit.Bid - Entry.Ask) / StandardFluc;
                case Signal.Down:
                    return (Entry.Bid - MaxRiskAtMaxProfit.Ask) / StandardFluc;
                default:
                    return 0.0d;
            }
        }

        public double GetMaxProfitPoints()
        {
            switch (Order)
            {
                case Signal.Up:
                    return (MaxProfit.Bid - Entry.Ask) / StandardFluc;
                case Signal.Down:
                    return (Entry.Bid - MaxProfit.Ask) / StandardFluc;
                default:
                    return 0.0d;
            }
        }
    }
}
