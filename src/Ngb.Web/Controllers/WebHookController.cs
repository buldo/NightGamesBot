using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ngb.Bot;

namespace Ngb.Web.Controllers
{
    using System.Net.Http;

    [Produces("application/json")]
    [Route("api/WebHook")]
    public class WebHookController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUpdateMessagesProcessor _messagesProcessor;

        public WebHookController(ILoggerFactory loggerFactory, IUpdateMessagesProcessor messagesProcessor)
        {
            _logger = loggerFactory.CreateLogger<WebHookController>();
            _messagesProcessor = messagesProcessor;
        }

        // POST: api/WebHook
        [HttpPost]
        public async void Post(HttpRequestMessage request)
        {
            _logger.LogDebug("Update received");
            var reqStr = await request.Content.ReadAsStringAsync();
            _logger.LogDebug(reqStr);
            _messagesProcessor.ProcessUpdateRequestAsync(reqStr);
        }
    }
}
