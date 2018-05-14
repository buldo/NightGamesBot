namespace Buldo.Ngb.Bot.EnginesManagement
{
    using System.Diagnostics;

    [DebuggerDisplay("{GameType} {Name} {Login}")]
    public class EngineInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public GameType GameType { get; set; }
    }
}