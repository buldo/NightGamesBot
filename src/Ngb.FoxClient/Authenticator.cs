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

            var responce = _httpClient.PostAsync(
                LOGIN_PATH,
                new FormUrlEncodedContent(
                    new[] 
                    {
                        new KeyValuePair<string, string>("email", login),
                        new KeyValuePair<string, string>("pass", password),
                    })).Result;
            if (responce.RequestMessage.RequestUri.LocalPath == LOGIN_PATH)
            {
                throw new Exception("AuthError");
            }
            
            IsAuthenticated = true;
        }
    }
}
