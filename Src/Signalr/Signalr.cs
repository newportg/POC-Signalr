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
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "SignalRConnectionInfo")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "SignalR connection Issue")]
        public SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "HubValue", ConnectionStringSetting = "AzureSignalRConnectionString")] SignalRConnectionInfo connectionInfo)
        {
            _logger.LogInformation($"SignalR Connection URL = '{connectionInfo.Url}'");
            return connectionInfo;
        }

        [Function("BroadcastToAll")]
        [OpenApiOperation(operationId: "BroadcastToAll")]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(string), Description = "message", Example = typeof(string))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "SignalRMessageAction")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "SignalR connection Issue")]
        [SignalROutput(HubName = "HubValue", ConnectionStringSetting = "AzureSignalRConnectionString")]
        public SignalRMessageAction BroadcastToAll([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            using var bodyReader = new StreamReader(req.Body);

            _logger.LogInformation($"SignalR BroadcastToAlL");

            return new SignalRMessageAction("newEvent")
            {
                // broadcast to all the connected clients without specifying any connection, user or group.
                Arguments = new[] { bodyReader.ReadToEnd() },
            };

        }
    }
}
