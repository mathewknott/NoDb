using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;

namespace NoDb.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "CUES DEMO API",
                    Description = "This is a demo.",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Mathew Knott", Email = "mathewknott@gmail.com", Url = "https://www.mathewknott.com" }
                });
            });
            services.ConfigureSwaggerGen(options =>
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Mat.Web.xml");
                options.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureHsts(this IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(100);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });
        }

        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));   
                    options.FormatterMappings.SetMediaTypeMappingForFormat("config", MediaTypeHeaderValue.Parse("application/xml"));
                    options.FormatterMappings.SetMediaTypeMappingForFormat("js", MediaTypeHeaderValue.Parse("application/json"));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                })
                .AddXmlSerializerFormatters()
                .AddRazorPagesOptions(options => { });
        }

        public static void ConfigureJwt(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters();
                });
        }
    }
}