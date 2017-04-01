using System;
using System.Collections.Generic;
using System.Text;

namespace Buldo.Ngb.Bot.Controllers
{
    using System.Threading.Tasks;
    using Fox;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using UsersManagement;

    internal class GameController : IUpdateProcessor
    {
        private LineEngine _engine;

        public GameController()
        {

        }

        public Task ProcessUpdate(Update update, BotUser user, TelegramBotClient client)
        {
            return Task.CompletedTask;
        }

        public void SetEngine(LineEngine engine)
        {
            _engine = engine;
        }
    }
}
