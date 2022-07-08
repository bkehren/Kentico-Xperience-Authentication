using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using CMS.Helpers;
using Kentico.Membership;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

namespace KXAuthentication
{
    public class Startup
    {
        private const string AUTHENTICATION_COOKIE_NAME = "identity.authentication";
        public const string DEFAULT_WITHOUT_LANGUAGE_PREFIX_ROUTE_NAME = "DefaultWithoutLanguagePrefix";

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable desired Kentico Xperience features
            var kenticoServiceCollection = services.AddKentico(features =>
            {
                features.UsePageBuilder();
                features.UsePageRouting(new PageRoutingOptions { EnableAlternativeUrls = true, CultureCodeRouteValuesKey = "culture" });
            });

            if (Environment.IsDevelopment())
            {
                kenticoServiceCollection.SetAdminCookiesSameSiteNone();
                kenticoServiceCollection.DisableVirtualContextSecurityForLocalhost();
            }

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddLocalization()
                .AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
                });

            ConfigureMembershipServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            // page not found config
            app.Use(async (context, next) =>
            {
                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        context.Request.Path = "/page-not-found"; // page in the content tree
                        break;
                    case (int)HttpStatusCode.Unauthorized:
                        context.Request.Path = URLHelper.AddParameterToUrl("/Account/SignIn", "ReturnUrl", context.Request.Path);
                        break;
                    case (int)HttpStatusCode.Forbidden:
                        context.Request.Path = URLHelper.AddParameterToUrl("/Account/PermissionDenied", "ReturnUrl", context.Request.Path);
                        break;
                    case (int)HttpStatusCode.InternalServerError:
                        context.Request.Path = "/error"; // page in the content tree
                        break;
                }
                await next();
            });

            app.UseStaticFiles();

            app.UseKentico();
            app.UseCookiePolicy();
            app.UseCors();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Kentico().MapRoutes();

                endpoints.MapControllerRoute(
                    name: DEFAULT_WITHOUT_LANGUAGE_PREFIX_ROUTE_NAME,
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });
        }

        private static void ConfigureMembershipServices(IServiceCollection services)
        {
            // Adds Xperience services required by the system's Identity implementation
            services.AddScoped<IPasswordHasher<ApplicationUser>, Kentico.Membership.PasswordHasher<ApplicationUser>>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>()
                            // Adds token providers used to generate tokens for email confirmations, password resets, etc.
                            .AddApplicationDefaultTokenProviders()
                            // Adds an implementation of the UserStore for working with Xperience user objects
                            .AddUserStore<ApplicationUserStore<ApplicationUser>>()
                            // Adds an implementation of the RoleStore used for working with Xperience roles
                            .AddRoleStore<ApplicationRoleStore<ApplicationRole>>()
                            // Adds an implementation of the UserManager for Xperience membership
                            .AddUserManager<ApplicationUserManager<ApplicationUser>>()
                            // Adds the default implementation of the SignInManger
                            .AddSignInManager<SignInManager<ApplicationUser>>();

            // Adds authentication and authorization services provided by the framework
            services.AddAuthentication();
            services.AddAuthorization();

            // Configures the application's authentication cookie
            services.ConfigureApplicationCookie(c =>
            {
                c.LoginPath = new PathString("/Account/SignIn");
                c.AccessDeniedPath = new PathString("/Account/PermissionDenied");
                c.ExpireTimeSpan = TimeSpan.FromDays(10);
                c.SlidingExpiration = true;
                c.Cookie.Name = AUTHENTICATION_COOKIE_NAME;
                c.ReturnUrlParameter = "ReturnUrl";
            });

            // Registers the authentication cookie in Xperience with the 'Essential' cookie level
            // Ensures that the cookie is preserved when changing a visitor's allowed cookie level below 'Visitor'
            CookieHelper.RegisterCookie(AUTHENTICATION_COOKIE_NAME, CookieLevel.Essential);
        }
    }
}
