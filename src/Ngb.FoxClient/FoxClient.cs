using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngb.FoxClient
{
    using System.Net;
    using System.Net.Http;

    using AngleSharp.Extensions;
    using AngleSharp.Parser.Html;

    public class FoxClient
    {
        private readonly Authenticator _authenticator;

        private readonly HttpClient _httpClient;
        

        public FoxClient(string baseUrl)
        {
            var httpHandler = new HttpClientHandler { CookieContainer = new CookieContainer(), UseCookies = true, UseProxy = false};
            _httpClient = new HttpClient(httpHandler, true)
                              {
                                  BaseAddress = new Uri(baseUrl)
                              };
            _authenticator = new Authenticator(_httpClient);
        }

        public string TeamName { get; private set; }

        public bool IsAuthenticated => _authenticator.IsAuthenticated;

        public void SetCredentials(string login, string password)
        {
            _authenticator.Authenticate(login, password);
            Refresh();
        }

        public void Refresh()
        {
            var result = _httpClient.GetAsync("play/").Result;
            if (_authenticator.IsAuthPageResponse(result))
            {
                _authenticator.Reauthenticate();
                result = _httpClient.GetAsync("play/").Result;
                if (_authenticator.IsAuthPageResponse(result))
                {
                    throw new Exception("Auth fail");
                }
            }

            var parser = new HtmlParser();
            var document = parser.Parse(result.Content.ReadAsStreamAsync().Result);
            TeamName = document.GetElementsByClassName("team_name").FirstOrDefault()?.TextContent;

        }
        
    }
}
