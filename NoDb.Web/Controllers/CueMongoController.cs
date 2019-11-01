using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoDb.Web.Models.Cues;
using NoDb.Web.Services;

namespace NoDb.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CueMongoController : Controller
    {
        private readonly ICuesRepository _cueRepository;

        public CueMongoController(ICuesRepository cueRepository)
        {
            _cueRepository = cueRepository;
        }

        //[NoCache]
        [HttpGet]
        public async Task<IEnumerable<CueMongo>> Get()
        {
            return await _cueRepository.GetAllCues();
        }

        // GET api/Cues/5 - retrieves a specific Cue using either Id or InternalId (BSonId)
        [HttpGet("{id}")]
        public async Task<CueMongo> Get(string id)
        {
            return await _cueRepository.GetCue(id) ?? new CueMongo();
        }

        // GET api/Cues/text/date/size
        // ex: http://localhost:53617/api/Cues/Test/2018-01-01/10000
        //[NoCache]
        [HttpGet(template: "{bodyText}/{updatedFrom}/{headerSizeLimit}")]
        public async Task<IEnumerable<CueMongo>> Get(string question, int catalogueNumber)
        {
            return await _cueRepository.GetCues(question, catalogueNumber)
                        ?? new List<CueMongo>();
        }

        // POST api/Cues - creates a new Cue
        [HttpPost]
        public void Post([FromBody] CueMongo newCue)
        {
            _cueRepository.AddCue(new CueMongo
            {
                Id = newCue.Id,
                Question = newCue.Question,
                Answer = newCue.Answer,
                Created = DateTime.Now,
                Updated = DateTime.Now
                //,UserId = newCue.UserId
            });
        }

        // PUT api/Cues/5 - updates a specific Cue
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string question, [FromBody]string answer, [FromBody]int sequence)
        {
            _cueRepository.UpdateCueDocument(id, question, answer, sequence);
        }

        // DELETE api/Cues/5 - deletes a specific Cue
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _cueRepository.RemoveCue(id);
        }
    }

}