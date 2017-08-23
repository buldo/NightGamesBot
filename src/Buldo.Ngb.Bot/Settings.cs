namespace Buldo.Ngb.Bot
{
    using System;
    using EnginesManagement;
    
    public class Settings
    {
        private EngineInfo _activeEngineSettings;

        public EngineInfo ActiveEngineSettings
        {
            get
            {
                return _activeEngineSettings;
            }
            set
            {
                _activeEngineSettings = value;
                ActiveEngineSettingsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> ActiveEngineSettingsUpdated;
    }
}
