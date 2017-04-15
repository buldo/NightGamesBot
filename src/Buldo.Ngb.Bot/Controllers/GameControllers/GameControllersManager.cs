using System;
using System.Collections.Generic;
using System.Text;

namespace Buldo.Ngb.Bot.Controllers
{
    using EnginesManagement;
    using GameControllers;
    using Routing;

    internal class GameControllersManager
    {
        private readonly EnginesManager _enginesManager;
        private readonly Router _router;

        private Type _lastGameControllerType;

        public GameControllersManager(EnginesManager enginesManager, Router router)
        {
            _enginesManager = enginesManager;
            _router = router;

            _enginesManager.ActiveEngineChanged += EnginesManagerOnActiveEngineChanged;
            EnginesManagerOnActiveEngineChanged(null, EventArgs.Empty);
        }

        private void EnginesManagerOnActiveEngineChanged(object sender, EventArgs eventArgs)
        {
            if (_lastGameControllerType != null)
            {
                _router.Unregister(_lastGameControllerType);
            }

            switch (_enginesManager.ActiveEngine.Type)
            {
                case GameType.RedFoxLine:
                    _lastGameControllerType = typeof(RedFoxLineGameController);
                    _router.RegisterMessageContoller<RedFoxLineGameController>();
                    break;
                case GameType.RedFoxSafari:
                    break;
                case GameType.Drl:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
