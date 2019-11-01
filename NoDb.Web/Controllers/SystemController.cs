using System;
using Microsoft.AspNetCore.Mvc;
using NoDb.Web.Models.Cues;
using NoDb.Web.Services;

namespace NoDb.Web.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        private readonly ICuesRepository _cueRepository;

        public SystemController(ICuesRepository cueRepository)
        {
            _cueRepository = cueRepository;
        }

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public string Get(string setting)
        {
            if (setting == "init")
            {
                _cueRepository.RemoveAllCues();

                _cueRepository.CreateIndex();

                _cueRepository.AddCue(new CueMongo
                {
                    CueId = "1",
                    Question = "What is Melanoma",
                    Answer = "Melanoma is a type of skin cancer",
                    Updated = DateTime.Now,
                    Created = DateTime.Now,
                    Category = new CategoryMongo
                    {
                        Name = "Cancer",
                        CategoryNumber = 1
                    }
                });

                _cueRepository.AddCue(new CueMongo
                {
                    CueId = "2",
                    Question = "What is Basal Cell Carcinoma",
                    Answer = "Basal Cell Carcinoma is a type of skin cancer",
                    Updated = DateTime.Now,
                    Created = DateTime.Now,
                    Category = new CategoryMongo
                    {
                        Name = "Cancer",
                        CategoryNumber = 1
                    }
                });

                return "Database cuesDb was created, and collection 'cues' was filled with 2 sample items";
            }

            return "Unknown";
        }
 
    }
}