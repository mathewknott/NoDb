using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoDb.Web.Services;

namespace NoDb.Web.ViewComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class TrackingViewComponent : ViewComponent
    {
        private readonly IHeaderService _headerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerService"></param>
        public TrackingViewComponent(IHeaderService headerService)
        {
            _headerService = headerService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _headerService.GetGtmAsync());
        }
    }
}