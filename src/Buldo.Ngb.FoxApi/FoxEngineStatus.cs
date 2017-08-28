namespace Buldo.Ngb.FoxApi
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using JetBrains.Annotations;

    public class FoxEngineStatus : IEquatable<FoxEngineStatus>
    {
        public FoxEngineStatus([NotNull] string teamName,
                               bool isGameRunning,
                               InputResult inputResult,
                               [NotNull] string taskName,
                               [NotNull] string message,
                               [NotNull] IDictionary<string, int> mainCodes,
                               [NotNull] IDictionary<string, int> bonusCodes,
                               [NotNull] IList<AcceptedCode> acceptedCodes)
        {
            TeamName = teamName;
            InputResult = inputResult;
            TaskName = taskName;
            Message = message;
            IsGameRunning = isGameRunning;
            AcceptedCodes = new ReadOnlyCollection<AcceptedCode>(acceptedCodes);
            BonusCodes = new ReadOnlyDictionary<string, int>(bonusCodes);
            MainCodes = new ReadOnlyDictionary<string, int>(mainCodes);
        }

        public bool IsGameRunning { get; }

        public string TeamName { get; }

        public InputResult InputResult { get; }

        public string Message { get; }

        public string TaskName { get; }

        public IReadOnlyDictionary<string, int> MainCodes { get; }

        public IReadOnlyDictionary<string, int> BonusCodes { get; }

        public IReadOnlyList<AcceptedCode> AcceptedCodes { get; }

        public bool Equals(FoxEngineStatus other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(TeamName, other.TeamName) &&
                   InputResult == other.InputResult &&
                   string.Equals(Message, other.Message) &&
                   CompareDictionaries(MainCodes, other.MainCodes) &&
                   CompareDictionaries(BonusCodes, other.BonusCodes) &&
                   CompareCodes(AcceptedCodes, other.AcceptedCodes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FoxEngineStatus)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TeamName != null ? TeamName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (int)InputResult;
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MainCodes != null ? MainCodes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (BonusCodes != null ? BonusCodes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AcceptedCodes != null ? AcceptedCodes.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(FoxEngineStatus left, FoxEngineStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FoxEngineStatus left, FoxEngineStatus right)
        {
            return !Equals(left, right);
        }

        private bool CompareDictionaries([NotNull] IReadOnlyDictionary<string, int> first, [NotNull] IReadOnlyDictionary<string, int> second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            foreach (var firstPair in first)
            {
                if (!second.TryGetValue(firstPair.Key, out var secondValue) || firstPair.Value != secondValue)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CompareCodes(IReadOnlyList<AcceptedCode> first, IReadOnlyList<AcceptedCode> second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            foreach (var firstCode in first)
            {
                if (!second.Contains(firstCode))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
