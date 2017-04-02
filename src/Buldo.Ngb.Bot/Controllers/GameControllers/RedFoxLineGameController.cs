using System;
using System.Collections.Generic;
using System.Text;

namespace Buldo.Ngb.Bot.Controllers
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
            Response($"{status.TeamName}");
        }

    }
}
