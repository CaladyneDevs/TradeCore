namespace Cyvzn.Cpus
{
    public abstract class Cpu
    {
        protected Cpu(string prefix, Symbol s, Frequency f)
        {
            Prefix = prefix; Symbol = s; Frequency = f;
        }
        public Symbol Symbol { get; private set; }
        public Frequency Frequency { get; private set; }
        public string Prefix { get; private set; } = "";
    }
}