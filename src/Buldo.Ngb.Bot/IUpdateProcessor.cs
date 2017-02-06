using System.Collections.Generic;
using Buldo.Ngb.Bot.UsersManagement;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Buldo.Ngb.Bot
{
    public interface IUpdateProcessor
    {
        /// <summary>
        /// Ключ, который определяет, что обновление должно быть обработано в этом процессоре
        /// </summary>
        string Key { get; }
        
        void ProcessUpdate(Update update, BotUser user, TelegramBotClient client);
    }
}