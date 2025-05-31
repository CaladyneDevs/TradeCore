using System;

namespace Cambistry
{
    public class OpenTrade : Trade
    {
        public OpenTrade(Cyvzn.Symbol symbol, Signal signal, Quote fill) : base(symbol, signal, fill) { }
        public void In(Quote q)
        {
            if (Closed) { return; }
            switch (Order)
            {
                case Signal.Up:
                    if (q.Bid > MaxProfit.Bid) { MaxProfit = q; MaxPullback = q; MaxRiskAtMaxProfit = MaxRisk; }
                    if (q.Bid < MaxPullback.Bid) { MaxPullback = q; }
                    if (q.Bid < MaxRisk.Bid) { MaxRisk = q; }
                    break;
                case Signal.Down:
                    if (q.Ask < MaxProfit.Ask) { MaxProfit = q; MaxPullback = q; MaxRiskAtMaxProfit = MaxRisk; }
                    if (q.Ask > MaxPullback.Ask) { MaxPullback = q; }
                    if (q.Ask > MaxRisk.Ask) { MaxRisk = q; }
                    break;
                case Signal.None:
                default:
                    throw new Exception("X");
            }
            Exit = q;
        }
        public void Close() { Closed = true; }
    }
}
