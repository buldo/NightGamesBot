using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buldo.Ngb.Bot.EnginesManagement;
using Buldo.Ngb.Web.Data;

namespace Buldo.Ngb.Web.BotInfrastructure
{
    internal class BotEnginesRepository : BaseBotRepository, IEngineInfosRepository
    {
        public BotEnginesRepository(Func<ApplicationDbContext> contextCreator)
            : base(contextCreator)
        {
        }

        public List<EngineInfo> GetEngines()
        {
            using (var context = ContextCreator())
            {
                return context.Engines.ToList();
            }
        }

        public EngineInfo GetEngineById(int engineId)
        {
            using (var context = ContextCreator())
            {
                return context.Engines.FirstOrDefault(e => e.Id == engineId);
            }
        }
    }
}
