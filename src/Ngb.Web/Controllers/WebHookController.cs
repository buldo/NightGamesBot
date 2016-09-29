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
        public void Post([FromBody]string value)
        {
            _logger.LogDebug("Update received");
            _logger.LogDebug(value);
            _messagesProcessor.ProcessUpdateRequestAsync(value);
        }
    }
}
