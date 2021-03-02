using CW.Thiedze.Domain;
using CW.Thiedze.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

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
        public ActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] HttpRequestMessage request, ILogger log)
        {
            LoginDto userDto = request.Content.ReadAsAsync<LoginDto>().Result;
            User user = UserService.Login(userDto.Username, userDto.Password);

            if (user.IsValid)
            {
                return new ObjectResult(user);
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
