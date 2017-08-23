using System;
using System.Collections.Generic;
using System.Text;

namespace Buldo.Ngb.Bot.Engines
{
    using System.Threading;
    using EnginesManagement;

    internal abstract class BaseGameEngine : IGameEngine
    {
        public abstract GameType Type { get; }

        public virtual void Activate()
        {
            //throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
