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
            var strongTags = document.GetElementsByTagName("strong").FirstOrDefault(d => d.TextContent == "Основные коды");
            var status = new FoxEngineStatus
            {
                TeamName = GetTeamName(document)
            };

            FillCodesOnLocation(status, document);

            return status;
        }

        private void FillCodesOnLocation(FoxEngineStatus status, IHtmlDocument document)
        {
            var element = document.
                GetElementsByTagName("strong")?.
                FirstOrDefault(el => el.TextContent == "Основные коды").ParentElement;
            if (element == null)
            {
                return;
            }

            var codesString = element.TextContent.Split(':')[1];
            var codes = codesString.
                Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).
                Select(s => s.Trim()).
                GroupBy(o => o);

            foreach (var code in codes)
            {
                status.CodesOnLocation.Add(code.Key, code.Count());
            }
        }

        private string GetTeamName(IHtmlDocument document)
        {
            return document.GetElementsByClassName("team_name").FirstOrDefault()?.TextContent ?? string.Empty;
        }


    }
}
