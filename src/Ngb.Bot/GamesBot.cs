using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngb.Bot
{
    using Newtonsoft.Json;

    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;

    public class GamesBot : IUpdateMessagesProcessor
    {
        private readonly TelegramBotClient _client;

        public GamesBot(string token)
        {
            _client = new TelegramBotClient(token);
        }

        public void StartLongPooling()
        {
            _client.OnUpdate += ClientOnOnUpdate;
            _client.StartReceiving();
        }

        public async void ProcessUpdateRequestAsync(string updateStrging)
        {
            var update = JsonConvert.DeserializeObject<Update>(updateStrging);
            await ProcessUpdateAsync(update);
        }

        private async Task ProcessUpdateAsync(Update update)
        {
            await _client.SendTextMessageAsync(update.Message.Chat.Id, "echo " + update.Message.Text);
        }

        private async void ClientOnOnUpdate(object sender, UpdateEventArgs updateEventArgs)
        {
            await ProcessUpdateAsync(updateEventArgs.Update);
        }
    }
}
