using System;

namespace Cambistry
{
    public readonly struct Quote
    {
        public readonly double Bid;
        public readonly double Ask;
        public readonly DateTime Time;
        public readonly double MidPrice;
        public Quote(double bid, double ask, DateTime time)
        {
            MidPrice = (bid + ask) / 2.0d;
            Bid = bid; Ask = ask; Time = time;
        }
    }
}