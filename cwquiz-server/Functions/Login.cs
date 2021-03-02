using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CW.Thiedze
{
    public static class Login
    {
        

        [FunctionName("Login")]
        public static ObjectResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "login")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new ObjectResult("");
        }
    }
}
