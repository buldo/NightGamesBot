﻿using Buldo.Ngb.Bot.Controllers;
using Buldo.Ngb.Bot.EnginesManagement;
using Buldo.Ngb.Bot.UsersManagement;
using Telegram.Bot.Types.Enums;

namespace Buldo.Ngb.Bot
{
    using System.Threading.Tasks;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;

    public class GamesBot : IUpdateMessagesProcessor
    {
        private readonly TelegramBotClient _client;
        private readonly IUsersRepository _usersRepository;
        private readonly IEnginesRepository _enginesRepository;
        private readonly BotStartupConfiguration _startupConfiguration;
        private readonly Router _router = new Router();

        public GamesBot(BotStartupConfiguration startupConfiguration, IUsersRepository usersRepository, IEnginesRepository enginesRepository)
        {
            _startupConfiguration = startupConfiguration;
            _client = new TelegramBotClient(_startupConfiguration.Token);
            _usersRepository = usersRepository;
            _enginesRepository = enginesRepository;

            _router.MapRoute("engines", new SettingsController(enginesRepository));
            _router.SetDefaultRoute(new EchoController());
        }

        public void StartLongPooling()
        {
            _client.OnUpdate += ClientOnOnUpdate;
            _client.StartReceiving();
        }

        public async Task ProcessUpdateAsync(Update update)
        {
            var user = await AuthBarrierStepAsync(update);
            if (user == null)
            {
                return;
            }

            await _router.ProcessUpdate(update, user, _client);
        }

        private async void ClientOnOnUpdate(object sender, UpdateEventArgs updateEventArgs)
        {
            await ProcessUpdateAsync(updateEventArgs.Update);
        }

        public void EnableWebHook(string url)
        {
            _client.SetWebhookAsync(url);
        }

        private async Task<BotUser> AuthBarrierStepAsync(Update update)
        {
            if (update.Type != UpdateType.MessageUpdate)
            {
                return null;
            }

            var user = _usersRepository.GetUser(update.Message.From.Id);
            if (user == null)
            {
                if (update.Message.Type == MessageType.TextMessage && 
                    update.Message.Text == _startupConfiguration.AccessKey)
                {
                    user = new BotUser {TelegramId = update.Message.From.Id, IsActive = true };
                    _usersRepository.AddUser(user);
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, "Добро пожаловать");
                    return null;
                }
                
                await _client.SendTextMessageAsync(update.Message.Chat.Id, "Введите код доступа");
                return null;
            }

            if (!user.IsActive)
            {
                await _client.SendTextMessageAsync(update.Message.Chat.Id, "Вы заблокированы");
                return null;
            }

            return user;
        }
    }
}
