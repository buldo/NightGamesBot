namespace Buldo.Ngb.Bot.Controllers.GameControllers
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Engines;
    using EnginesManagement;
    using FoxApi;
    using Routing;

    [Route("")]
    internal class RedFoxLineGameController : BaseGameController
    {
        private readonly RedFoxLineEngine _engine;

        public RedFoxLineGameController(EnginesManager enginesManager)
        {
            _engine = (RedFoxLineEngine) enginesManager.ActiveEngine;
        }

        [Route("статус")]
        [Route("status")]
        [Route("state")]
        public async Task GetStatus()
        {
            var status = await _engine.GetStatus();
            await ResponseAsync(PrepareMessage(status.NewStatus));
        }

        [Route("interval")]
        public async Task SetAutoRefresh(int interval)
        {
            if (interval > 0)
            {
                _engine.SetAutoRefreshInterval(interval);
                await ResponseAsync($"Автообновление с периодом {interval} секунд");
            }
            else
            {
                _engine.DisableAutoRefresh();
                await ResponseAsync($"Автообновление отключено");
            }
        }

        [Route("")]
        public async Task InputDataAsync(string data)
        {
            var statuses = await _engine.ProcessUserInput(data);
            var status = statuses.NewStatus;
            string message;
            switch (status.InputResult)
            {
                case InputResult.None:
                    message = "Ничего";
                    break;
                case InputResult.CodeAccepted:
                    var codeType = status.AcceptedCodesWithMessages.FirstOrDefault(c => c.Value == data)?.Type ?? string.Empty;
                    message = $"Принят {codeType} {status.Message}";
                    break;
                case InputResult.CodeNotExists:
                    message = "Не существует";
                    break;
                case InputResult.CodeAlreadyAccepted:
                    message = "Уже введён";
                    break;
                case InputResult.SpoilerOpened:
                    message = "Спойлер открыт";
                    break;
                case InputResult.WrongSpoiler:
                    message = "Неверный спойлер";
                    break;
                case InputResult.NewLevel:
                    message = "Новый уровень";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            await ResponseAsync(message);
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

            if (!string.IsNullOrWhiteSpace(status.Message))
            {
                builder.AppendLine(status.Message);
            }

            if (!string.IsNullOrWhiteSpace(status.TaskName))
            {
                builder.AppendLine(status.TaskName);
            }

            if (status.MainCodes.Count > 0)
            {
                var acceptedCodesDic = status.AcceptedMainCodes.GroupBy(c => c.Type).ToDictionary(g => g.Key);
                builder.AppendLine("Основные коды");
                foreach (var code in status.MainCodes.OrderBy(c => c.Key, StringComparer.InvariantCultureIgnoreCase))
                {
                    var acceptedCodesCnt = 0;
                    if (acceptedCodesDic.TryGetValue(code.Key, out var acceptedCodes))
                    {
                        acceptedCodesCnt = acceptedCodes.Count();
                    }

                    builder.AppendLine($"{code.Key}: {acceptedCodesCnt}/{code.Value}");
                }
            }

            if (status.BonusCodes.Count > 0)
            {
                var acceptedCodesDic = status.AcceptedBonusCodes.GroupBy(c => c.Type).ToDictionary(g => g.Key);
                builder.AppendLine();
                builder.AppendLine("Бонусные коды");
                foreach (var code in status.BonusCodes)
                {
                    var acceptedCodesCnt = 0;
                    if (acceptedCodesDic.TryGetValue(code.Key, out var acceptedCodes))
                    {
                        acceptedCodesCnt = acceptedCodes.Count();
                    }

                    builder.AppendLine($"{code.Key}: {acceptedCodesCnt}/{code.Value}");
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
