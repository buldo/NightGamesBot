namespace Buldo.Ngb.Bot.EnginesManagement
{
    using System;
    using Engines;

    internal class EnginesManager
    {
        private readonly EnginesFactory _factory;
        private IGameEngine _activeEngine;

        public EnginesManager(EnginesFactory factory)
        {
            _factory = factory;
        }

        public event EventHandler<EventArgs> ActiveEngineChanged;

        public IGameEngine ActiveEngine
        {
            get
            {
                return _activeEngine;
            }

            private set
            {
                var oldEngine = _activeEngine;
                _activeEngine = value;
                _activeEngine.Activate();
                oldEngine?.Dispose();
                ActiveEngineChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ActivateEngine(EngineInfo engine)
        {
            ActiveEngine = _factory.CreateEngine(engine);
        }
    }
}
