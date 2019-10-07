using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mat.Web.Extensions;
using Mat.Web.Models;
using Mat.Web.Models.Configuration;
using Mat.Web.Models.Cues;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NoDb;

namespace Mat.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CueController : Controller
    {
        private readonly IBasicQueries<Question> _pageQueriesQuestion;
        private readonly IBasicCommands<Question> _pageCommandsQuestion;
        private readonly IBasicQueries<Category> _pageQueriesCategory;
        private readonly IBasicCommands<Category> _pageCommandsCategory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="fileProvider"></param>
        /// <param name="memoryCache"></param>
        /// <param name="hosting"></param>
        /// <param name="logger"></param>
        /// <param name="pageQueriesCategory"></param>
        /// <param name="pageCommandsCategory"></param>
        /// <param name="pageQueriesQuestion"></param>
        /// <param name="pageCommandsQuestion"></param>
        public CueController(
            IOptions<AppOptions> optionsAccessor,
            IFileProvider fileProvider, 
            IMemoryCache memoryCache, 
            IHostingEnvironment hosting,
            ILogger<CueController> logger,
            IBasicQueries<Category> pageQueriesCategory,
            IBasicCommands<Category> pageCommandsCategory,
            IBasicQueries<Question> pageQueriesQuestion,
            IBasicCommands<Question> pageCommandsQuestion
            )
        {
            _pageQueriesQuestion = pageQueriesQuestion;
            _pageCommandsQuestion = pageCommandsQuestion;
            _pageQueriesCategory = pageQueriesCategory;
            _pageCommandsCategory = pageCommandsCategory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/cue/GetCategories", Name = "Get Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var results = await _pageQueriesCategory.GetAllAsync("Cues");
            
            var enumerable = results.ToList();

            if (!enumerable.Any())
            {
                return Json(new JsonPagedResult<IEnumerable<Category>>
                {
                    Total = 0
                });
            }

            return Json(new JsonPagedResult<IEnumerable<Category>>
            {
                Total = enumerable.Count,
                Rows = enumerable
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("/cue/CreateCategory", Name = "Create Category")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = new Random().Next(0, 100000000);
                category.InternalName = category.Title.ToSlug();

                if (category.ParentCategoryId == 0)
                {
                    await _pageCommandsCategory.CreateAsync("Cues", category.Id.ToString(), category);
                    return Content("success");
                }

                var findParent = await _pageQueriesCategory.FetchAsync("Cues", category.ParentCategoryId.ToString());

                if (findParent != null)
                {
                    await _pageCommandsCategory.CreateAsync("Cues", category.Id.ToString(), category);
                    return Content("success");
                }
                ModelState.AddModelError("CreateCategory", "Cannot find the parent Category");
            }
            return Json(new { success = "fail", errorList = JsonConvert.SerializeObject(ModelState.Values.Where(x => x.Errors.Count > 0), Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut("/cue/UpdateCategory", Name = "Update Category")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                var c = _pageQueriesCategory.FetchAsync("Cues", category.Id.ToString()).Result;

                if (c != null)
                {
                    c.Title = category.Title;
                    c.InternalName = c.Title.ToSlug();
                    c.ParentCategoryId = category.ParentCategoryId == 0 ? c.ParentCategoryId : category.ParentCategoryId;
                    await _pageCommandsCategory.UpdateAsync("Cues", c.Id.ToString(), c);
                }
                else
                {
                    return Json(new
                    {
                        success = "fail",
                        errorList = "Could not find Category Id"
                    });
                }

                return Content("success");
            }
            return Json(new { success = "fail", errorList = JsonConvert.SerializeObject(ModelState.Values.Where(x => x.Errors.Count > 0), Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/cue/GetAllQuestions", Name = "Get All Category Questions")]
        public async Task<IActionResult> GetAllQuestions(bool randomise = false)
        {
            var results = await _pageQueriesQuestion.GetAllAsync("Cues");
            var list = randomise ? results.OrderBy(x => x.Sequence).ToList() : results.ToList();

            if (!list.Any())
            {
                return Json(new JsonPagedResult<IEnumerable<Question>>
                {
                    Total = 0
                });
            }

            return Json(new JsonPagedResult<IEnumerable<Question>>
            {
                Total = list.Count,
                Rows = list
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="randomise"></param>
        /// <returns></returns>
        [HttpGet("/cue/GetCategoryQuestions", Name = "Get Category Questions")]
        public async Task<IActionResult> GetCategoryQuestions(int categoryId, bool randomise = false)
        {
            var results = await _pageQueriesQuestion.GetAllAsync("Cues");

            var list = randomise ? results.Where(x => x.CategoryId.Equals(categoryId)).OrderBy(x => x.Sequence).ToList() : results.Where(x => x.CategoryId.Equals(categoryId)).ToList();
            
            if (!list.Any())
            {
                return Json(new JsonPagedResult<IEnumerable<Question>>
                {
                    Total = 0
                });
            }

            return Json(new JsonPagedResult<IEnumerable<Question>>
            {
                Total = list.Count,
                Rows = list
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("/cue/DeleteCategory", Name = "Delete Category")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (ModelState.IsValid)
            {
                var parentExistsResult = _pageQueriesCategory.GetAllAsync("Cues").Result.Where(x => x.ParentCategoryId == categoryId).ToList();

                if (!parentExistsResult.Any())
                {
                    await _pageCommandsCategory.DeleteAsync("Cues", categoryId.ToString());
                }
                else
                {
                    return Json(new
                    {
                        success = "fail",
                        errorList = $"The category cannot be deleted. At least one category uses this category as a parent category. {string.Join(",", parentExistsResult.Select(x => x.Id))}"
                    });
                }

                return Content("success");
            }
            return Json(new { success = "fail", errorList = JsonConvert.SerializeObject(ModelState.Values.Where(x => x.Errors.Count > 0), Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost("/cue/CreateQuestion", Name = "Create Category Question")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                var count = _pageQueriesQuestion.GetCountAsync("Cues").Result;
                question.Id = count + 1;
                question.Sequence = new Random().Next(0, 100000000);
                question.InternalName = question.Title.ToSlug();
                await _pageCommandsQuestion.CreateAsync("Cues", question.Id.ToString(), question);
                return Content("success");
            }
            return Json(new { success = "fail", errorList = JsonConvert.SerializeObject(ModelState.Values.Where(x => x.Errors.Count > 0), Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPut("/cue/UpdateQuestion", Name = "Update Category Question")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                var q = _pageQueriesQuestion.FetchAsync("Cues", question.Id.ToString()).Result;

                if (q != null)
                {
                    q.Title = question.Title;
                    q.InternalName = q.Title.ToSlug();
                    q.Answer = question.Answer; 
                    q.CategoryId = question.CategoryId ?? q.CategoryId;
                    await _pageCommandsQuestion.UpdateAsync("Cues", q.Id.ToString(), q);
                }
                else
                {
                    return Json(new
                    {
                        success = "fail",
                        errorList = "Could not find ID"
                    });
                }

                return Content("success");
            }
            return Json(new { success = "fail", errorList = JsonConvert.SerializeObject(ModelState.Values.Where(x => x.Errors.Count > 0), Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut("/cue/Randomise", Name = "Randomises the Category Questions")]
        [IgnoreAntiforgeryToken]
        public IActionResult Randomise()
        {
            if (ModelState.IsValid)
            {
                var result = _pageQueriesQuestion.GetAllAsync("Cues").Result;

                if (result != null)
                {
                    foreach (var question in result)
                    {
                        var rand = new Random();
                        question.Sequence = rand.Next(0, 100000000);
                        _pageCommandsQuestion.UpdateAsync("Cues", question.Id.ToString(), question);
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = "fail",
                        errorList = "Could not load the questions"
                    });
                }

                return Content("success");
            }
            return Json(new { success = "fail", errorList = JsonConvert.SerializeObject(ModelState.Values.Where(x => x.Errors.Count > 0), Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpDelete("/cue/DeleteQuestion", Name = "Delete Question")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            if (ModelState.IsValid)
            {
                var result = _pageQueriesQuestion.FetchAsync("Cues", questionId.ToString()).Result;

                if (result != null)
                {
                    await _pageCommandsQuestion.DeleteAsync("Cues", result.Id.ToString());
                }
                else
                {
                    return Json(new
                    {
                        success = "fail",
                        errorList = "Could not find ID"
                    });
                }

                return Content("success");
            }
            return Json(new { success = "fail", errorList = JsonConvert.SerializeObject(ModelState.Values.Where(x => x.Errors.Count > 0), Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) });
        }
    }
}