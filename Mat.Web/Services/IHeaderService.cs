using System.Threading.Tasks;
using Mat.Web.Models;

namespace Mat.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHeaderService
    {
        Task<HeaderModel> GetGtmAsync();
    }
}