using BlogProject.Application.Authentication.Commands;
using BlogProject.Application.Authentication.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClearApplication(this IServiceCollection services) 
        {
            //services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
            //services.AddScoped<IAuthenticationQueryService, AuthenticationQueryService>();

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            return services;
        }
    }
}
