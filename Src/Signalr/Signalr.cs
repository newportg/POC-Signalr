using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Signalr
{
    public class Signalr
    {
        private readonly ILogger _logger;

        public Signalr(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Signalr");
        }

        [Function("negotiate")]
        [OpenApiOperation(operationId: "negotiate")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(SignalRConnectionInfo), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "Configuration / Database issue")]
        public SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "HubValue", ConnectionStringSetting = "AzureSignalRConnectionString")] SignalRConnectionInfo connectionInfo)
        {
            _logger.LogInformation($"SignalR Connection URL = '{connectionInfo.Url}'");
            return connectionInfo;
        }
    }
}
