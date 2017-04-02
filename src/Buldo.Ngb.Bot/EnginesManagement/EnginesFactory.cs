namespace Buldo.Ngb.Bot.EnginesManagement
{
    using System;
    using Engines;

    internal class EnginesFactory
    {
        public IGameEngine CreateEngine(EngineInfo engine)
        {
            switch (engine.GameType)
            {
                case GameType.RedFoxLine:
                    return new RedFoxLineEngine(engine);
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