using Cyvzn.Cpus.Candling;
using System.Collections.Generic;

namespace Cyvzn
{
    public class Frequency
    {
        public readonly CandleCpu MyCandleCpu;
        public readonly uint BaseInterval;
        public readonly string BaseType;
        internal readonly uint CpuInterval;
        public string Name { get; }
        public Frequency(ApiSetting.ApiMarket.ApiChart chart, Symbol symbol)
        {
            BaseInterval = chart.Interval;
            BaseType = chart.Type;  
            Name = BaseInterval.ToString() + chart.Type;
            CpuInterval = BaseInterval;
            switch (chart.Type)
            {
                case "Q":
                    MyCandleCpu = new QCpu(this, symbol, ".");
                    break;
                case "s":
                    MyCandleCpu = new TCpu(this, symbol, ".");
                    break;
                case "m":
                    CpuInterval = BaseInterval * 60;
                    MyCandleCpu = new TCpu(this, symbol, ".");
                    break;
                case "h":
                    CpuInterval = BaseInterval * 3600;
                    MyCandleCpu = new TCpu(this, symbol, ".");
                    break;
                default:
                    throw new System.Exception("Mod Q/s/m/h");
            }
        }
    }
}
