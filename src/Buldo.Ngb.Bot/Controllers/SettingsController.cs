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
        private readonly IEnginesRepository _enginesRepository;
        private readonly Dictionary<string, Func<Update, BotUser, TelegramBotClient, Task>> _commands;
        
        public SettingsController(IEnginesRepository enginesRepository)
        {
            _enginesRepository = enginesRepository;
            _commands = new Dictionary<string, Func<Update, BotUser, TelegramBotClient, Task>>()
            {
                {"engines list", ShowEnginesList},
                {"engines select", SelectActiveEngine}
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
            return client.SendTextMessageAsync(update.Message.Chat.Id, "Движок выбран");
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