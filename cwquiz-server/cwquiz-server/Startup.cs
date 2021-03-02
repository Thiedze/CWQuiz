using CW.Thiedze.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(CW.Thiedze.Startup))]

namespace CW.Thiedze
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            Configuration(builder.Services);
        }

        private void Configuration(IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}