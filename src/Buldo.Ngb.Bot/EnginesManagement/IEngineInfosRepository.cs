namespace Buldo.Ngb.Bot.EnginesManagement
{
    using System.Collections.Generic;

    public interface IEngineInfosRepository
    {
        List<EngineInfo> GetEngines();
        EngineInfo GetEngineById(int engineId);
    }
}