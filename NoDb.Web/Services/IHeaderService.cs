using System.Threading.Tasks;
using NoDb.Web.Models;

namespace NoDb.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHeaderService
    {
        Task<HeaderModel> GetGtmAsync();
    }
}