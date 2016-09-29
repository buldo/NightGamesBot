namespace Ngb.FoxClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    internal class Authenticator
    {
        private const string LOGIN_PATH = "/user/login/";

        private readonly HttpClient _httpClient;

        private string _login;
        private string _password;


        public Authenticator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool IsAuthenticated { get; private set; }

        public void Authenticate(string login, string password)
        {
            _login = login;
            _password = password;

            Reauthenticate();
        }

        public void Reauthenticate()
        {
            var responseMessage = _httpClient.PostAsync(
                LOGIN_PATH,
                new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("email", _login),
                        new KeyValuePair<string, string>("pass", _password),
                    })).Result;
            if (IsAuthPageResponse(responseMessage))
            {
                throw new Exception("AuthError");
            }

            IsAuthenticated = true;
        }


        public bool IsAuthPageResponse(HttpResponseMessage responseMessage)
        {
            return responseMessage.RequestMessage.RequestUri.LocalPath == LOGIN_PATH;
        }
    }
}
