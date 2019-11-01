using System.Collections.Generic;
using System.Threading.Tasks;
using NoDb.Web.Models.Cues;

namespace NoDb.Web.Services
{
    public interface ICuesRepository
    {
        Task<IEnumerable<CueMongo>> GetAllCues();

        Task<CueMongo> GetCue(string id);

        // query after multiple parameters
        Task<IEnumerable<CueMongo>> GetCues(string question, int categoryNumber);

        // add new Question document
        Task AddCue(CueMongo item);

        // remove a single document / Cue
        Task<bool> RemoveCue(string id);

        // update just a single document / Cue
        Task<bool> UpdateCue(string id, string question, string answer, int sequence);

        // demo interface - full document update
        Task<bool> UpdateCueDocument(string id, string question, string answer, int sequence);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllCues();

        Task<string> CreateIndex();
    }
}