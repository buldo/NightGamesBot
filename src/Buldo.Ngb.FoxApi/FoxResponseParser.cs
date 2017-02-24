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
            return status;
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
