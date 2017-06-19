namespace Buldo.Ngb.Bot.Engines
{
    using System;
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

        public async Task<string> GetStatus()
        {
            if (!await RequestNewStatusAsync())
            {
                return PrepareMessage(_lastStatus);
            }

            return null;
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

        public async Task ProcessUserInput(string data)
        {
            if (_lastStatus == null)
            {
                await RequestNewStatusAsync();
            }
        }

        private async void RefreshTimerCallback(object state)
        {
            var currentStatus = await _api.GetStatusAsync();
            ProcessStatusInfo(currentStatus);
        }

        private async void ProcessStatusInfo(FoxEngineStatus currentStatus)
        {
            await RequestNewStatusAsync();
        }

        private string PrepareMessage(FoxEngineStatus status)
        {
            var builder = new StringBuilder();

            if (!status.IsGameRunning)
            {
                builder.AppendLine(status.TeamName);
                builder.AppendLine("Игра не запущена");
                return builder.ToString();
            }

            if (string.IsNullOrWhiteSpace(status.Message))
            {
                builder.AppendLine(status.Message);
            }

            if (status.MainCodes.Count > 0)
            {
                builder.AppendLine("Основные коды");
                foreach (var code in status.MainCodes)
                {
                    builder.AppendLine($"{code.Key}: {code.Value}");
                }
                builder.AppendLine();
            }

            if (status.MainCodes.Count > 0)
            {
                builder.AppendLine("Бонусные коды");
                foreach (var code in status.BonusCodes)
                {
                    builder.AppendLine($"{code.Key}: {code.Value}");
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private async Task<bool> RequestNewStatusAsync()
        {
            var newStatus = await _api.GetStatusAsync();
            if (_lastStatus != newStatus)
            {
                _lastStatus = newStatus;
                await _broadcastSender.SendBroadcastMessageAsync(PrepareMessage(_lastStatus));
                return true;
            }

            return false;
        }
    }
}
