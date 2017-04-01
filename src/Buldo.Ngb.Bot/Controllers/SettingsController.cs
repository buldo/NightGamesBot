using System.Text;
using Buldo.Ngb.Bot.EnginesManagement;

namespace Buldo.Ngb.Bot.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Routing;

    [Route("engines")]
    public class SettingsController : BaseTelegramController
    {
        private readonly IEnginesRepository _enginesRepository;

        public SettingsController(IEnginesRepository enginesRepository)
        {
            _enginesRepository = enginesRepository;
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

                //_bot.SetActiveEngine(engine);
                return Response($"Выбран движок {engine.Id}");
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