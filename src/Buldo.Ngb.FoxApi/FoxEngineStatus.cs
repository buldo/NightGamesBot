namespace Buldo.Ngb.FoxApi
{
    using System.Collections.Generic;

    public class FoxEngineStatus
    {
        public string TeamName { get; internal set; }

        public InputResult InputResult { get; internal set; }

        public string MessageText { get; internal set; }

        public Dictionary<string, int> CodesOnLocation { get; private set; } = new Dictionary<string, int>();

        public List<EnteredCode> EnteredCodes { get; private set; } = new List<EnteredCode>();
    }
}
