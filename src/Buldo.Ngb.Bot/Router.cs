using System.Threading.Tasks;

namespace Buldo.Ngb.Bot
{
    using System.Collections.Generic;
    using UsersManagement;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    internal class Router : IUpdateProcessor
    {
        private readonly Dictionary<string, IUpdateProcessor> _processors = new Dictionary<string, IUpdateProcessor>();

        private IUpdateProcessor _default;

        public void MapRoute(string path, IUpdateProcessor processor)
        {
            _processors.Add(path.Trim(' ', '/'), processor);
        }

        public void SetDefaultRoute(IUpdateProcessor processor)
        {
            _default = processor;
        }

        public Task ProcessUpdate(Update update, BotUser user, TelegramBotClient client)
        {
            if (update.Type == UpdateType.MessageUpdate && update.Message.Type == MessageType.TextMessage)
            {
                var trimmed = update.Message.Text.Trim(' ', '/');
                foreach (var processor in _processors)
                {
                    if (trimmed.StartsWith(processor.Key))
                    {
                        return processor.Value.ProcessUpdate(update, user, client);
                    }
                }
            }
            
            return _default.ProcessUpdate(update, user, client);
        }
    }
}
