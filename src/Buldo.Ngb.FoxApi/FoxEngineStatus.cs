﻿namespace Buldo.Ngb.FoxApi
{
    using System.Collections.Generic;

    public class FoxEngineStatus
    {
        public string TeamName { get; internal set; }

        public InputResult InputResult { get; internal set; }

        public string MessageText { get; internal set; }

        public Dictionary<string, int> MainCodes { get; private set; } = new Dictionary<string, int>();

        public Dictionary<string, int> BonusCodes { get; private set; } = new Dictionary<string, int>();

        public List<AcceptedCode> AcceptedCodes { get; private set; } = new List<AcceptedCode>();
    }
}
