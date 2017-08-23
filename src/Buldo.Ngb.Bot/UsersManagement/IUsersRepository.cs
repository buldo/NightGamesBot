namespace Buldo.Ngb.Bot.UsersManagement
{
    using System.Collections.Generic;

    public interface IUsersRepository
    {
        BotUser GetUser(int telegramId);

        void AddUser(BotUser botUser);
        List<BotUser> GetAllUsers();
    }
}