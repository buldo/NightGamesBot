namespace Buldo.Ngb.Bot.Controllers
{
    using System.Threading.Tasks;
    using Routing;
    using Telegram.Bot.Types;

    [Route("")]
    internal class EchoController : BaseTelegramController
    {
        [Route("")]
        public Task ProcessUpdate(Message message)
        {
            return ResponseAsync(message.Text);
        }
    }
}
