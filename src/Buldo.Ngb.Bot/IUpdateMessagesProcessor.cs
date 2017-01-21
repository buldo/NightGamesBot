namespace Buldo.Ngb.Bot
{
    using System.Threading.Tasks;
    using Telegram.Bot.Types;

    public interface IUpdateMessagesProcessor
    {
        Task ProcessUpdateAsync(Update update);
    }
}