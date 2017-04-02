﻿namespace Buldo.Ngb.Bot.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using EnginesManagement;
    using Routing;

    [Route("engines")]
    internal class SettingsController : BaseTelegramController
    {
        private readonly IEngineInfosRepository _enginesRepository;
        private readonly EnginesManager _enginesManager;

        public SettingsController(IEngineInfosRepository enginesRepository, EnginesManager enginesManager)
        {
            _enginesRepository = enginesRepository;
            _enginesManager = enginesManager;
        }

        [Route("list")]
        public Task List()
        {
            var builder = new StringBuilder();
            foreach (var engineInfo in _enginesRepository.GetEngines())
            {
                builder.AppendLine($"{engineInfo.Id}. {engineInfo.Name}");
            }

            return Response(builder.ToString());
        }

        [Route("select")]
        public Task Select(string param)
        {
            int engineId;
            if (int.TryParse(param, out engineId))
            {
                var engine = _enginesRepository.GetEngineById(engineId);
                if (engine == null)
                {
                    return Response("Движок не найден");
                }

                _enginesManager.ActivateEngine(engine);

                return Response($"Выбран движок {engine.Id} {engine.Name}");
            }

            return Response("Идентификатор не разпознан");
        }

        [Route("")]
        public Task Index()
        {
            return Response($"engines list - список движков{Environment.NewLine}engines select [Id]- выбор движка");
        }
    }
}