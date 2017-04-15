namespace Buldo.Ngb.FoxApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AngleSharp.Dom.Html;

    internal class FoxResponseParser
    {
        public FoxEngineStatus Parse(IHtmlDocument document)
        {
            var teamName = GetTeamName(document);
            var mainCodes = ParseCodes("Основные коды", document);
            var bonusCodes = ParseCodes("Бонусные коды", document);
            var inputResult = ParseInputResult(document);
            return new FoxEngineStatus(teamName, inputResult.result, inputResult.message,mainCodes,bonusCodes, new List<AcceptedCode>());
        }

        private (InputResult result, string message) ParseInputResult(IHtmlDocument document)
        {
            var message = document.GetElementById("message")?.TextContent;
            var lowMessage = message?.ToLower();
            if (string.IsNullOrWhiteSpace(lowMessage))
            {
                return (InputResult.None, string.Empty);
            }

            if (lowMessage.StartsWith("код не существует"))
            {
                return (InputResult.CodeNotExists, string.Empty);
            }

            if (lowMessage.StartsWith("код принят"))
            {
                if (lowMessage.Contains("spoiler alert"))
                {
                    return (InputResult.SpoilerOpened, string.Empty);
                }

                return (InputResult.CodeAccepted, message.Remove(0, "Код принят!".Length).Trim());
            }

            if (lowMessage.StartsWith("этот код уже был введен"))
            {
                return (InputResult.CodeAlreadyAccepted, string.Empty);
            }

            if (lowMessage.StartsWith("неверный код спойлера"))
            {
                return (InputResult.WrongSpoiler, string.Empty);
            }

            return (InputResult.None, string.Empty);
        }

        private Dictionary<string, int> ParseCodes(string codesSection, IHtmlDocument document)
        {
            Dictionary<string, int> codesToFill = new Dictionary<string, int>();

            var element = document.
                GetElementsByTagName("strong")?.
                FirstOrDefault(el => el.TextContent == codesSection)?.ParentElement;
            if (element != null)
            {
                var codesString = element.TextContent.Split(':')[1];
                var codes = codesString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(s => s.Trim())
                                       .Where(o => !string.IsNullOrWhiteSpace(o))
                                       .GroupBy(o => o);

                foreach (var code in codes)
                {
                    codesToFill.Add(code.Key, code.Count());
                }
            }

            return codesToFill;
        }

        private string GetTeamName(IHtmlDocument document)
        {
            return document.GetElementsByClassName("team_name").FirstOrDefault()?.TextContent ?? string.Empty;
        }
    }
}
