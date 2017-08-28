namespace Buldo.Ngb.Bot.Controllers.GameControllers
{
    using System;
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
            if (status != null)
            {
                await ResponseAsync(status);
            }
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
            var status = await _engine.ProcessUserInput(data);
            string message;
            switch (status.InputResult)
            {
                case InputResult.None:
                    message = "Ничего";
                    break;
                case InputResult.CodeAccepted:
                    message = "Принят";
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            await ResponseAsync(message);
        }
    }
}
