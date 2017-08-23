using System;
using Buldo.Ngb.Web.Data;

namespace Buldo.Ngb.Web.BotInfrastructure
{
    internal abstract class BaseBotRepository
    {
        protected Func<ApplicationDbContext> ContextCreator { get; }

        protected BaseBotRepository(Func<ApplicationDbContext> contextCreator)
        {
            ContextCreator = contextCreator;
        }
    }
}
