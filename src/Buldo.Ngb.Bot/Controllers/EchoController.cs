namespace Buldo.Ngb.Bot.Controllers
{
    using System.Threading.Tasks;
    using UsersManagement;
    using Telegram.Bot;
    using Telegram.Bot.Types;

    internal class EchoController : IUpdateProcessor
    {
        public Task ProcessUpdate(Update update, BotUser user, TelegramBotClient client)
        {
            return client.SendTextMessageAsync(update.Message.Chat.Id, update.Message.Text);
        }
    }
}
