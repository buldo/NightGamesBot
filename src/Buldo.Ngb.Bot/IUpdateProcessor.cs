using System.Threading.Tasks;
using Buldo.Ngb.Bot.UsersManagement;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Buldo.Ngb.Bot
{
    public interface IUpdateProcessor
    {
        Task ProcessUpdate(Update update, BotUser user, TelegramBotClient client);
    }
}