using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Buldo.Ngb.Bot.UsersManagement;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Buldo.Ngb.Bot.Modules
{
    public class SettingsManager : IUpdateProcessor
    {
        private readonly Dictionary<string, Action<Update, BotUser, TelegramBotClient>> _commands;

        public string Key => "/settings";

        public SettingsManager()
        {
            _commands = new Dictionary<string, Action<Update, BotUser, TelegramBotClient>>()
            {
                {"engines", ShowEnginesHelp},
                {"engines list", ShowEnginesHelp}
            };
        }


        public void ProcessUpdate(Update update, BotUser user, TelegramBotClient client)
        {
            
        }

        private async void ShowEnginesHelp(Update update, BotUser botUser, TelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(update.Message.)
            update.Message.From.Id
        }
    }
}