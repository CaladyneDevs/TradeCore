using System;
using System.Collections.Generic;
using Cambistry;

namespace Cyvzn
{
   public class Trader
   {
        public Trader(Symbol s) { MySymbol = s; }
        public Signal Get(out OpenTrade ot) { ot = null; if (Trading != Signal.None) { ot = Open; } return Trading; }
        public List<Trade> GetClosed() {  return new List<Trade>(Closed); }
        internal Signal Trading { get; private set; } = Signal.None;
        private OpenTrade Open = null;
        private readonly Queue<Trade> Closed = new Queue<Trade>();
        private readonly Symbol MySymbol;
        internal int Store = 50;
        public Signal Last = Signal.None;
        internal void In(Quote q) { if (Trading != Signal.None) { Open.In(q); } }
        internal void In(Signal s)
        {
            switch (Trading)
            {
                case Signal.None:
                    switch (s)
                    {
                        case Signal.Up:
                            OpenNew();
                            break;
                        case Signal.Down:
                            OpenNew();
                            break;
                        default:
                            break;
                    }
                    break;
                case Signal.Up:
                    switch (s)
                    {
                        case Signal.None:
                            CloseAndStore();
                            break;
                        case Signal.Down:
                            CloseAndStore(); OpenNew();
                            break;
                        default:
                            break;
                    }
                    break;
                case Signal.Down:
                    switch (s)
                    {
                        case Signal.None:
                            CloseAndStore();
                            break;
                        case Signal.Up:
                            CloseAndStore(); OpenNew();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            void OpenNew()
            {
                Trading = s;
                Open = new OpenTrade(MySymbol, Trading, MySymbol.Quote);
            }
            void CloseAndStore()
            {
                Trading = Signal.None;
                Open.Close();
                Last = Open.Order;
                if (Store > 0)
                {
                    Closed.Enqueue(Open);
                    while (Closed.Count > Store) { Closed.Dequeue(); }
                }
            }
        }
    }
}
