namespace Buldo.Ngb.Web.BotInfrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Buldo.Ngb.Bot.UsersManagement;
    using Buldo.Ngb.Web.Data;

    internal class BotUsersRepository : BaseBotRepository, IUsersRepository
    {
        public BotUsersRepository(Func<ApplicationDbContext> contextCreator)
            : base(contextCreator)
        {
        }

        public BotUser GetUser(int telegramId)
        {
            using (var context = ContextCreator())
            {
                return context.BotUsers.Find(telegramId);
            }
        }

        public void AddUser(BotUser botUser)
        {
            using (var context = ContextCreator())
            {
                context.BotUsers.Add(botUser);
                context.SaveChanges();
            }
        }

        public List<BotUser> GetAllUsers()
        {
            using (var context = ContextCreator())
            {
                return context.BotUsers.ToList();
            }
        }
    }
}
