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
            var status = new FoxEngineStatus
            {
                TeamName = GetTeamName(document)
            };

            FillCodesOnLocation(status.MainCodes, "Основные коды", document);
            FillCodesOnLocation(status.BonusCodes, "Бонусные коды", document);
            FillInputResult(status, document);
            return status;
        }

        private void FillInputResult(FoxEngineStatus status, IHtmlDocument document)
        {
            var message = document.GetElementById("message")?.TextContent;
            var lowMessage = message?.ToLower();
            if (string.IsNullOrWhiteSpace(lowMessage))
            {
                status.InputResult = InputResult.None;
                return;
            }

            if (lowMessage.StartsWith("код не существует"))
            {
                status.InputResult = InputResult.CodeNotExists;
                return;
            }

            if (lowMessage.StartsWith("код принят"))
            {
                status.InputResult = InputResult.CodeAccepted;
                status.Message = message.Remove(0, "Код принят!".Length).Trim();
                return;
            }


        }

        private void FillCodesOnLocation(Dictionary<string, int> codesToFill, string codesSection, IHtmlDocument document)
        {
            var element = document.
                GetElementsByTagName("strong")?.
                FirstOrDefault(el => el.TextContent == codesSection)?.ParentElement;
            if (element == null)
            {
                return;
            }

            var codesString = element.TextContent.Split(':')[1];
            var codes = codesString.
                Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).
                Select(s => s.Trim()).
                Where(o => !string.IsNullOrWhiteSpace(o)).
                GroupBy(o => o);

            foreach (var code in codes)
            {
                codesToFill.Add(code.Key, code.Count());
            }
        }


        private string GetTeamName(IHtmlDocument document)
        {
            return document.GetElementsByClassName("team_name").FirstOrDefault()?.TextContent ?? string.Empty;
        }
    }
}
