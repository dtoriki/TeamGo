using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeamGo.Identity.Server.Engine.Contexts;

namespace TeamGo.Identity.Server.Registration
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddDbContext<UserStoreContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });

            _ = services
                .AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 1;
                    options.User.RequireUniqueEmail = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddEntityFrameworkStores<UserStoreContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
