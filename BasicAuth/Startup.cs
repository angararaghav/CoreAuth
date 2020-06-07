
using BasicAuth.AuthorizationRequirements;
using BasicAuth.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace BasicAuth
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Grandmas.Coockie";
                    config.LoginPath = "/Home/Authenticate";
                });
           

            services.AddAuthorization(config =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultPolicy = defaultAuthBuilder
                //.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.DateOfBirth)
                //.Build();

                //config.DefaultPolicy = defaultPolicy;

                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));
                config.AddPolicy("Claim.DOB", policyBuilder =>
                 {
                     policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                 });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            services.AddControllersWithViews(config =>
            {

                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultPolicy = defaultAuthBuilder
                .RequireAuthenticatedUser()
                .Build();

                //Global authorization all pages are secured.
                //config.Filters.Add(new AuthorizeFilter(defaultPolicy));

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //Who are you?
            app.UseAuthentication();

            //Are you allowed?
            app.UseAuthorization();
                       

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapDefaultControllerRoute();

             });
        }
    }
}
