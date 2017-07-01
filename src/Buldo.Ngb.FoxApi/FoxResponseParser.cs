namespace Buldo.Ngb.FoxApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AngleSharp.Dom.Html;
    using AngleSharp.Extensions;

    internal class FoxResponseParser
    {
        public FoxEngineStatus Parse(IHtmlDocument document)
        {
            var teamName = GetTeamName(document);
            var isRunning = ParseIsRunning(document);
            if (isRunning)
            {
                var mainCodes = ParseCodes("Основные коды", document);
                var bonusCodes = ParseCodes("Бонусные коды", document);
                var inputResult = ParseInputResult(document);
                var acceptedCodes = ParseAcceptedCodes(document);
                return new FoxEngineStatus(teamName, true, inputResult.result, inputResult.message, mainCodes, bonusCodes, acceptedCodes);
            }
            else
            {
                return new FoxEngineStatus(teamName,
                                           false,
                                           InputResult.None,
                                           string.Empty,
                                           new Dictionary<string, int>(),
                                           new Dictionary<string, int>(),
                                           new List<AcceptedCode>());
            }
        }
        
        private bool ParseIsRunning(IHtmlDocument document)
        {
            return document.GetElementsByTagName("h3").All(e => e.TextContent != "На данный момент нет активных игр");
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


        private IList<AcceptedCode> ParseAcceptedCodes(IHtmlDocument document)
        {
            var acceptesCodes = new List<AcceptedCode>();

            var enteredCodesElements = document.GetElementsByClassName("found_codes").FirstOrDefault()?.GetElementsByTagName("li");
            if (enteredCodesElements == null)
            {
                return acceptesCodes;
            }

            foreach (var element in enteredCodesElements)
            {
                var codeClass = element.GetElementsByClassName("class").FirstOrDefault()?.Text();
                var fullCodeText = element.GetElementsByClassName("code").FirstOrDefault()?.Text();
                if (!string.IsNullOrWhiteSpace(codeClass) && !string.IsNullOrWhiteSpace(fullCodeText))
                {
                    var splited = fullCodeText.Split(new[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (splited.Length == 1)
                    {
                        acceptesCodes.Add(new AcceptedCode(codeClass, splited[0], string.Empty));
                    }
                    else if (splited.Length == 2)
                    {
                        acceptesCodes.Add(new AcceptedCode(codeClass, splited[0], splited[1]));
                    }
                }
            }

            return acceptesCodes;
        }
    }
}
