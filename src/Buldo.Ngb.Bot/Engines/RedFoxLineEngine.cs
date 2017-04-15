using System;
using System.Collections.Generic;
using System.Text;

namespace Buldo.Ngb.Bot.Engines
{
    using System.Threading;
    using System.Threading.Tasks;
    using EnginesManagement;
    using FoxApi;

    internal class RedFoxLineEngine : BaseGameEngine
    {
        private readonly FoxApi _api;
        private readonly Timer _timer;

        private FoxEngineStatus _lastStatus;

        public RedFoxLineEngine(EngineInfo settings)
        {
            _timer = new Timer(RefreshTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            _api = new FoxApi(settings.Address);
            _api.SetCredentials(settings.Login, settings.Password);
        }

        public override GameType Type => GameType.RedFoxLine;

        public Task<FoxEngineStatus> GetStatus()
        {
            return _api.GetStatusAsync();
        }

        public void SetAutoRefreshInterval(int interval)
        {
            DisableAutoRefresh();
            _timer.Change(TimeSpan.FromSeconds(interval), TimeSpan.FromSeconds(interval));
        }

        public void DisableAutoRefresh()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private async void RefreshTimerCallback(object state)
        {
            var currentStatus = await _api.GetStatusAsync();
            if (_lastStatus != currentStatus)
            {
                _lastStatus = currentStatus;
                
            }
        }
    }
}
