namespace Buldo.Ngb.Bot.EnginesManagement
{
    using System;
    using Engines;

    internal class EnginesFactory
    {
        private readonly BroadcastSender _broadcastSender;

        public EnginesFactory(BroadcastSender broadcastSender)
        {
            _broadcastSender = broadcastSender;
        }

        public IGameEngine CreateEngine(EngineInfo engine)
        {
            switch (engine.GameType)
            {
                case GameType.RedFoxLine:
                    return new RedFoxLineEngine(engine, _broadcastSender);
                case GameType.RedFoxSafari:
                    return new RexFoxSafariEngine();
                case GameType.Drl:
                    return new DrlEngine();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}