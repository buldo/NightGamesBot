using System;
using System.Collections.Generic;
using System.Text;

namespace Buldo.Ngb.Bot.Engines
{
    using System.Threading.Tasks;
    using EnginesManagement;
    using FoxApi;

    internal class RedFoxLineEngine : BaseGameEngine
    {
        private readonly FoxApi _api;

        public RedFoxLineEngine(EngineInfo settings)
        {
            _api = new FoxApi(settings.Address);
            _api.SetCredentials(settings.Login, settings.Password);
        }

        public override GameType Type => GameType.RedFoxLine;

        public Task<FoxEngineStatus> GetStatus()
        {
            return _api.GetStatusAsync();
        }
    }
}
