namespace Buldo.Ngb.FoxApi
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using AngleSharp.Parser.Html;

    public class FoxApi
    {
        private const string STATUS_PATH = "play/";
        private const string SUBMIT_CODE_PATH = "play/submit/";

        private readonly FoxResponseParser _responseParser = new FoxResponseParser();
        private readonly Authenticator _authenticator;
        private readonly HttpClient _httpClient;

        public FoxApi(string baseUrl)
        {
            baseUrl = baseUrl.StartsWith("http") ? baseUrl : "http://" + baseUrl;
            var httpHandler = new HttpClientHandler { CookieContainer = new CookieContainer(), UseCookies = true, UseProxy = false };
            _httpClient = new HttpClient(httpHandler, true)
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Buldo NGB");
            _authenticator = new Authenticator(_httpClient);
        }

        public bool IsAuthenticated => _authenticator.IsAuthenticated;

        public void SetCredentials(string login, string password)
        {
            _authenticator.Authenticate(login, password);
        }

        public Task<FoxEngineStatus> GetStatusAsync()
        {
            return RequestAsync(() => _httpClient.GetAsync(STATUS_PATH));
        }

        public Task<FoxEngineStatus> SendCodeAsync(string code)
        {
            var formContent = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("code", code)
            };

            return RequestAsync(() => _httpClient.PostAsync(SUBMIT_CODE_PATH, new FormUrlEncodedContent(formContent)));
        }

        private async Task<FoxEngineStatus> RequestAsync(Func<Task<HttpResponseMessage>> request)
        {
            var result = await request();
            if (_authenticator.IsAuthPageResponse(result))
            {
                _authenticator.Reauthenticate();
                result = await request();
                if (_authenticator.IsAuthPageResponse(result))
                {
                    throw new Exception("Auth fail");
                }
            }

            return _responseParser.Parse(new HtmlParser().Parse(await result.Content.ReadAsStreamAsync()));
        }
    }
}
