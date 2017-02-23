using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Buldo.Ngb.FoxClient
{
    using FoxApi;

    public class RedFoxLineClient
    {
        private readonly FoxApi _api;

        public event EventHandler<EventArgs> TaskChanged;

        public event EventHandler<EventArgs> SpoilerOpened;

        public event EventHandler<EventArgs> HintOpened;

        public RedFoxLineClient(string baseUrl)
        {
            _api = new FoxApi(baseUrl);
        }

        public CodeEnterResult EnterCode(string code)
        {
            throw new NotImplementedException();
        }

        public void EnterSpoiler(string spoiler)
        {
            throw new NotImplementedException();
        }


    }
}
