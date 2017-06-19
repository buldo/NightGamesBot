namespace Buldo.Ngb.Bot.Controllers.GameControllers
{
    using System.Threading.Tasks;
    using Engines;
    using EnginesManagement;
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

        [Route("set interval")]
        [Route("автообновление")]
        public void SetAutoRefresh(int interval)
        {
            if (interval > 0)
            {
                _engine.SetAutoRefreshInterval(interval);
            }
            else
            {
                _engine.DisableAutoRefresh();
            }
        }

        [Route("")]
        public Task InputDataAsync(string data)
        {
            return _engine.ProcessUserInput(data);
        }
    }
}
