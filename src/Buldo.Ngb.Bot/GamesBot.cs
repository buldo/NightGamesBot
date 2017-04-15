namespace Buldo.Ngb.Bot
{
    using System.Linq;
    using System.Threading.Tasks;
    using Grace.DependencyInjection;
    using Routing;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Controllers;
    using EnginesManagement;
    using UsersManagement;
    using Telegram.Bot.Types.Enums;

    public class GamesBot : IUpdateMessagesProcessor
    {
        private readonly TelegramBotClient _client;
        private readonly BotStartupConfiguration _startupConfiguration;
        private readonly IUsersRepository _usersRepository;
        private readonly Router _router;
        private readonly DependencyInjectionContainer _container = new DependencyInjectionContainer(configuration => configuration.AutoRegisterUnknown = false);
        private readonly EnginesManager _enginesManager;
        private readonly GameControllersManager _gameControllersManager;

        public GamesBot(BotStartupConfiguration startupConfiguration,
                        IUsersRepository usersRepository,
                        IEngineInfosRepository enginesRepository)
        {
            _startupConfiguration = startupConfiguration;
            _usersRepository = usersRepository;
            _client = new TelegramBotClient(_startupConfiguration.Token);
            _router = new Router(_container, _client);

            _container.Add(block => block.ExportInstance(usersRepository).As<IUsersRepository>());
            _container.Add(block => block.ExportInstance(enginesRepository).As<IEngineInfosRepository>());
            _container.Add(block => block.Export<EnginesFactory>().Lifestyle.Singleton());
            _container.Add(block => block.Export<EnginesManager>().Lifestyle.Singleton());

            // Выставляем Engine по умолчанию
            _enginesManager = _container.Locate<EnginesManager>();
            var defaultEngineInfo = enginesRepository.GetEngines().FirstOrDefault();
            if (defaultEngineInfo != null)
            {
                _enginesManager.ActivateEngine(defaultEngineInfo);
            }

            _gameControllersManager = new GameControllersManager(_enginesManager, _router);

            // Устанавливаем маршруты
            _router.RegisterMessageContoller<SettingsController>();
            //_router.RegisterMessageContoller<EchoController>();
        }



        public void StartLongPooling()
        {
            _client.OnUpdate += async (sender, args) => await ProcessUpdateAsync(args.Update);
            _client.StartReceiving();
        }

        public async Task ProcessUpdateAsync(Update update)
        {
            var user = await AuthBarrierStepAsync(update);
            if (user == null)
            {
                return;
            }

            await _router.ProcessUpdateAsync(update, user);
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
