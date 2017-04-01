namespace Buldo.Ngb.Bot.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using UsersManagement;

    public class BaseTelegramController
    {
        public Update Update { get; set; }

        public TelegramBotClient TelegramBotClient { get; set; }

        public BotUser User { get; set; }

        public Task Response(string message)
        {
            switch (Update.Type)
            {
                case UpdateType.UnknownUpdate:
                    break;
                case UpdateType.MessageUpdate:
                    return TelegramBotClient.SendTextMessageAsync(Update.Message.Chat.Id, message);
                case UpdateType.InlineQueryUpdate:
                    break;
                case UpdateType.ChosenInlineResultUpdate:
                    break;
                case UpdateType.CallbackQueryUpdate:
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        }
    }
}
