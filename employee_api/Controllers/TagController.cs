using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository.ReportRepository;
using DynamicReportAPI.Models.Repository.TagRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicReportAPI.Controllers
{
    [Route("api/[controller]")]
    public class TagController : Controller
    {

        public IConfiguration _iconfiguration;

        public TagController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        [HttpPost("AddUpdateTag")]
        public IActionResult AddUpdateTag([FromBody] TagModel tagModel)
        {
            TagRepository _menuRepository = new TagRepository(_iconfiguration);

            var Results = _menuRepository.insertUpdateTags(tagModel);
            if (Results.Tables.Count == 0)
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = ErrorReponceMessage.InternalServerError },
                    Msg = "Error",
                    Data = null
                });
            }
            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = ErrorReponceMessage.RecordAdded },
                Msg = "success",
                Data = Results
            });
        }

        [HttpGet("getTagList")]
        public IActionResult SelectTagList()
        {
            TagRepository _menuRepository = new TagRepository(_iconfiguration);

            var Results = _menuRepository.getTagList();
            if (Results.Tables.Count == 0)
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = ErrorReponceMessage.InternalServerError },
                    Msg = "Error",
                    Data = null
                });
            }
            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = ErrorReponceMessage.SelectExecuted + Results.Tables.Count },
                Msg = "success",
                Data = Results
            });
        }

    }
}