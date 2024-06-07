using Microsoft.Extensions.DependencyInjection;
using mvc.Services;

namespace mvc.Configuration
{
    public static class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
