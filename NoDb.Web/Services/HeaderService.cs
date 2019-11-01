﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NoDb.Web.Models;
using NoDb.Web.Models.Configuration;

namespace NoDb.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderService : IHeaderService
    {
        private readonly IOptions<AppOptions> _optionsAccessor;
        private readonly IHostingEnvironment _hosting;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="hosting"></param>
        /// <param name="httpContextAccessor"></param>
        public HeaderService(IOptions<AppOptions> optionsAccessor,
            IHostingEnvironment hosting,
            IHttpContextAccessor httpContextAccessor)
        {
            _optionsAccessor = optionsAccessor;
            _hosting = hosting;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<HeaderModel> GetGtmAsync()
        {
            var m = new HeaderModel {Gtm = _optionsAccessor.Value.Tracking.Gtm};
            var gtmDataLayer = string.Format(
                "{{'page': {{'pageInfo': {{'masthead': '{2}','Url': '{1}'}}}},'pageInstanceId': '{0}:{2}'}}",
                _hosting.EnvironmentName,
                _httpContextAccessor.HttpContext.Request.Path.Value.Replace("/", ""),
                _optionsAccessor.Value.SiteAbbreviation);

            m.Domain = _httpContextAccessor.HttpContext.Request.Host.Value;
            m.GtmDataLayer = gtmDataLayer;
            return Task.FromResult(m);
        }
    }
}