namespace Buldo.Ngb.Web.BotInfrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Buldo.Ngb.Bot.UsersManagement;
    using Buldo.Ngb.Web.Data;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<List<BotUser>> GetActiveUsersAsync()
        {
            using (var context = ContextCreator())
            {
                return await context.BotUsers.Where(u => u.IsActive == true).ToListAsync();
            }
        }

        public async Task DisableUserAsync(BotUser botUser)
        {
            using (var context = ContextCreator())
            {
                var user = await context.BotUsers.FindAsync(botUser.TelegramId);
                user.IsActive = false;
                await context.SaveChangesAsync();
            }
        }
    }
}
