using Mat.Web.Extensions;
using Mat.Web.Helpers;
using Mat.Web.Models.Configuration;
using Mat.Web.Models.Cues;
using Mat.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using NoDb;

namespace Mat.Web
{
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public IConfigurationRoot Configuration { get; }
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            _hostingEnvironment = env;
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureMvc();
            services.AddTransient<IHeaderService, HeaderService>();
            services.AddResponseCaching();
            services.ConfigureHsts();
            services.ConfigureSwagger();
            services.AddSingleton(_hostingEnvironment.ContentRootFileProvider);
            services.AddMemoryCache();
            services.ConfigureCors();
            services.ConfigureJwt();    

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //experiment nodb
            services.AddNoDb<Category>();
            services.AddNoDb<Question>();
            
            services.Configure<AppOptions>(Configuration.GetSection("App"));
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AppOptions> optionsAccessor)
        {
            if (env.IsDevelopment())
            {
				loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //HSTS is a protocol that instructs browsers to access the site via HTTPS.
                //The protocol has allowances for specifying how long the policy should be
                //enforced (max age) and whether the policy applies to subdomains or not.
                //You can also enable support for your domain to be added to the HSTS preload list.
                app.UseHsts();
            }
			
			loggerFactory.AddFile(Configuration.GetSection("Logging"));

            app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
                .AddDefaultSecurePolicy()
                .AddCustomHeader("Version", optionsAccessor.Value.Version)
                .AddCustomHeader("X-Robots-Tag", optionsAccessor.Value.Robots)
            );

            app.UseResponseCaching();
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                   const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });

            app.UseMvc();
        }
    }
}