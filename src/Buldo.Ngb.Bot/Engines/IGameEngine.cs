namespace Buldo.Ngb.Bot.Engines
{
    using System;
    using EnginesManagement;

    public interface IGameEngine : IDisposable
    {
        GameType Type { get; }

        void Activate();
    }
}