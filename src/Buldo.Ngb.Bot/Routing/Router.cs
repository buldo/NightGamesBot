namespace Buldo.Ngb.Bot.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Controllers;
    using Grace.DependencyInjection;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using UsersManagement;

    internal class Router
    {
        private readonly DependencyInjectionContainer _container;
        private readonly List<MessageRoute> _messageRoutes = new List<MessageRoute>();
        private readonly TelegramBotClient _telegramClient;

        public Router(DependencyInjectionContainer container, TelegramBotClient client)
        {
            _container = container;
            _telegramClient = client;
        }

        public void RegisterMessageContoller<T>() where T: BaseTelegramController
        {
            var controllerType = typeof(T);
            if (!_container.CanLocate(controllerType))
            {
                _container.Add(bl => bl.Export<T>());
            }

            foreach (var classAttr in controllerType.GetTypeInfo().GetCustomAttributes<RouteAttribute>())
            {
                foreach (var method in controllerType.GetMethods())
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length > 2)
                    {
                        throw new NotSupportedException("Не поддерживаются методы более, чем 2мя аргументами");
                    }

                    var isAwaitable = method.GetCustomAttributes<AsyncStateMachineAttribute>().Any() || method.ReturnType == typeof(Task);
                    foreach (var methodAttr in method.GetCustomAttributes<RouteAttribute>())
                    {
                        var route = new MessageRoute(
                            $"{classAttr.Path} {methodAttr.Path}".Trim().ToLower(),
                            controllerType,
                            method,
                            isAwaitable);
                        _messageRoutes.Add(route);
                    }
                }
            }
        }

        public void Unregister(Type controllerType)
        {
            var toRemove = _messageRoutes.Where(r => r.ControllerType == controllerType).ToList();
            foreach (var route in toRemove)
            {
                _messageRoutes.Remove(route);
            }
        }

        public Task ProcessUpdateAsync(Update update, BotUser user)
        {
            switch (update.Type)
            {
                case UpdateType.UnknownUpdate:
                    break;
                case UpdateType.MessageUpdate:
                    return ProcessMessageAsync(update, user);
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

        private async Task ProcessMessageAsync(Update update, BotUser user)
        {
            var message = update.Message;
            var text = (message.Text?? string.Empty).ToLower().Trim().Trim('/');
            MessageRoute route;
            if (string.IsNullOrEmpty(text))
            {
                route = _messageRoutes.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Path));
            }
            else
            {
                route =
                    _messageRoutes.Where(r => text.StartsWith(r.Path))
                                  .OrderByDescending(r => r.Path.Length)
                                  .FirstOrDefault();
            }

            if (route == null)
            {
                await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, "Не найден обработчик", replyToMessageId: message.MessageId);
                return;
            }

            var controller = _container.Locate(route.ControllerType);
            ActivateController((BaseTelegramController) controller, update, user);

            var invoceParams = new List<object>();
            foreach (var parameter in route.Method.GetParameters())
            {
                if (parameter.ParameterType == typeof(string))
                {
                    invoceParams.Add(text?.Remove(0, route.Path.Length) ?? string.Empty);
                    continue;
                }

                if (parameter.ParameterType == typeof(Message))
                {
                    invoceParams.Add(message);
                }
            }

            if (route.IsAwaitable)
            {
                await (dynamic) route.Method.Invoke(controller, invoceParams.ToArray());
            }
            else
            {
                route.Method.Invoke(controller, invoceParams.ToArray());
            }
        }

        private void ActivateController(BaseTelegramController controller, Update update, BotUser user)
        {
            controller.TelegramBotClient = _telegramClient;
            controller.Update = update;
            controller.User = user;
        }

        private class MessageRoute
        {
            public MessageRoute(string path, Type controllerType, MethodInfo method, bool isAwaitable)
            {
                Path = path;
                ControllerType = controllerType;
                Method = method;
                IsAwaitable = isAwaitable;
            }

            public string Path { get; }

            public Type ControllerType { get; }

            public MethodInfo Method { get; }
            public bool IsAwaitable { get; }
        }
    }
}
