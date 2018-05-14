namespace Buldo.Ngb.Bot.EnginesManagement
{
    using System;
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
            var users = await _usersRepository.GetActiveUsersAsync();
            foreach (var botUser in users)
            {
                try
                {
                    await _client.SendTextMessageAsync(botUser.TelegramId, message);
                }
                catch (Exception e)
                {
                    await _usersRepository.DisableUserAsync(botUser);
                }
            }
        }
    }
}
