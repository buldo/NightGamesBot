namespace Buldo.Ngb.Bot.Engines
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EnginesManagement;
    using FoxApi;

    internal class RedFoxLineEngine : BaseGameEngine
    {
        private readonly BroadcastSender _broadcastSender;
        private readonly FoxApi _api;
        private readonly Timer _timer;

        private FoxEngineStatus _lastStatus;

        public RedFoxLineEngine(EngineInfo settings, BroadcastSender broadcastSender)
        {
            _broadcastSender = broadcastSender;
            _timer = new Timer(RefreshTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            _api = new FoxApi(settings.Address);
            _api.SetCredentials(settings.Login, settings.Password);
        }

        public override GameType Type => GameType.RedFoxLine;

        public async Task<(FoxEngineStatus OldStatus, FoxEngineStatus NewStatus)> GetStatus()
        {
            var newStatus = await _api.GetStatusAsync();
            var retValue = (_lastStatus, newStatus);
            ProcessStatusInfo(newStatus);
            return retValue;
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

        public async Task<(FoxEngineStatus OldStatus, FoxEngineStatus NewStatus)> ProcessUserInput(string data)
        {
            var newStatus = await _api.SendCodeAsync(data);
            var retValue = (_lastStatus, newStatus);
            ProcessStatusInfo(newStatus);
            return retValue;
        }

        private async void RefreshTimerCallback(object state)
        {
            var currentStatus = await _api.GetStatusAsync();
            ProcessStatusInfo(currentStatus);
        }

        private async void ProcessStatusInfo(FoxEngineStatus currentStatus)
        {
            if (!currentStatus.IsGameRunning)
            {
                DisableAutoRefresh();
                await _broadcastSender.SendBroadcastMessageAsync("Игра не запущена. Автообновление отключено");
            }

            if (currentStatus == _lastStatus)
            {
                return;
            }

            if (currentStatus.IsGameRunning)
            {
                var realLastStatus = _lastStatus;
                _lastStatus = currentStatus;

                if (realLastStatus?.TaskName != currentStatus.TaskName)
                {
                    await _broadcastSender.SendBroadcastMessageAsync($"Новое задание {currentStatus.TaskName}");
                }
            }
        }
    }
}