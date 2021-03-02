using CW.Thiedze.Domain;
using CW.Thiedze.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CW.Thiedze
{
    public class LoginFunction
    {
        private IUserService UserService { get; }
        private ILogger Logger { get; }

        public LoginFunction(IUserService userService, ILoggerFactory loggerFactory)
        {
            UserService = userService;
            Logger = loggerFactory.CreateLogger<LoginFunction>();
        }

        [FunctionName("Login")]
        public ActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "login")] HttpRequest req, ILogger log)
        {
            User user = UserService.Login("sthiems", "6FEC2A9601D5B3581C94F2150FC07FA3D6E45808079428354B868E412B76E6BB");

            if (user.IsValid)
            {
                Logger.LogInformation(user.Firstname);
                return new ObjectResult(user.Firstname + "" + user.Lastname);
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
