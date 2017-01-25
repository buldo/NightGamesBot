using System.Collections.Generic;

namespace Buldo.Ngb.Bot.EnginesManagement
{
    public interface IEnginesRepository
    {
        List<EngineInfo> GetEngines();
    }
}