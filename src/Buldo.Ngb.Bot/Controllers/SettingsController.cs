using System.Text;
using Buldo.Ngb.Bot.EnginesManagement;

namespace Buldo.Ngb.Bot.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UsersManagement;
    using Telegram.Bot;
    using Telegram.Bot.Types;

    public class SettingsController : IUpdateProcessor
    {
        private const string LIST_COMMAND = "engines list";
        private const string SELECT_COMMAND = "engines select";
        private readonly GamesBot _bot;
        private readonly IEnginesRepository _enginesRepository;
        private readonly Dictionary<string, Func<Update, BotUser, TelegramBotClient, Task>> _commands;
        
        public SettingsController(GamesBot bot, IEnginesRepository enginesRepository)
        {
            _bot = bot;
            _enginesRepository = enginesRepository;
            _commands = new Dictionary<string, Func<Update, BotUser, TelegramBotClient, Task>>()
            {
                {LIST_COMMAND, ShowEnginesList},
                {SELECT_COMMAND, SelectActiveEngine}
            };
        }

        public Task ProcessUpdate(Update update, BotUser user, TelegramBotClient client)
        {
            var trimmed = update.Message.Text.TrimStart(' ', '/');
            foreach (var command in _commands)
            {
                if (trimmed.StartsWith(command.Key))
                {
                    return command.Value(update, user, client);
                }
            }

            return ShowEnginesHelp(update, user, client);
        }

        private Task ShowEnginesHelp(Update update, BotUser botUser, TelegramBotClient botClient)
        {
            return botClient.SendTextMessageAsync(update.Message.Chat.Id,
                $"engines list - список движков{Environment.NewLine}engines select [Id]- выбор движка");
        }

        private Task SelectActiveEngine(Update update, BotUser user, TelegramBotClient client)
        {
            int engineId;
            if (int.TryParse(update.Message.Text.Remove(0, SELECT_COMMAND.Length).Trim(), out engineId))
            {
                var engine = _enginesRepository.GetEngineById(engineId);
                if (engine == null)
                {
                    return client.SendTextMessageAsync(update.Message.Chat.Id, "Движок не найден");
                }

                _bot.SetActiveEngine(engine);
            }
            
            return client.SendTextMessageAsync(update.Message.Chat.Id, "Идентификатор не разпознан");
        }

        private Task ShowEnginesList(Update update, BotUser user, TelegramBotClient client)
        {
            var builder = new StringBuilder();
            foreach (var engineInfo in _enginesRepository.GetEngines())
            {
                builder.AppendLine($"{engineInfo.Id}. {engineInfo.Name}");
            }

            return client.SendTextMessageAsync(update.Message.Chat.Id, builder.ToString());
        }
    }
}