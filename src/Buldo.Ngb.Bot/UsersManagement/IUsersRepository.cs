namespace Buldo.Ngb.Bot.UsersManagement
{
    public interface IUsersRepository
    {
        BotUser GetUser(int telegramId);

        void AddUser(BotUser botUser);
    }
}