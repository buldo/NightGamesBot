namespace Buldo.Ngb.Bot.UsersManagement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersRepository
    {
        BotUser GetUser(int telegramId);

        void AddUser(BotUser botUser);

        List<BotUser> GetAllUsers();

        Task<List<BotUser>> GetActiveUsersAsync();

        Task DisableUserAsync(BotUser botUser);
    }
}