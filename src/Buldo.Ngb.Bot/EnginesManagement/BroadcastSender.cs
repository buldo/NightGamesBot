namespace Buldo.Ngb.Bot.EnginesManagement
{
    using System.Threading.Tasks;
    using Telegram.Bot;
    using UsersManagement;

    internal class BroadcastSender
    {
        private readonly IUsersRepository _usersRepository;
        private readonly TelegramBotClient _client;

        public BroadcastSender(IUsersRepository usersRepository, TelegramBotClient client)
        {
            _usersRepository = usersRepository;
            _client = client;
        }

        public async Task SendBroadcastMessageAsync(string message)
        {
            var users = _usersRepository.GetAllUsers();
            foreach (var botUser in users)
            {
                await _client.SendTextMessageAsync(botUser.TelegramId, message);
            }
        }
    }
}
