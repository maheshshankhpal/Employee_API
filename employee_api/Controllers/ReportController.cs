using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository.ReportRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicReportAPI.Controllers {
    [Route ("api/[controller]")]
    public class ReportController : Controller {

        public IConfiguration _iconfiguration;

        public ReportController (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        [HttpGet ("getreportmasters/{ScreenName}")]
        public IActionResult getReportMaster (string ScreenName) {
            ReportRepository _groupRepository = new ReportRepository (_iconfiguration);

            var Results = _groupRepository.GetReportMasterData (ScreenName);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results

            });
        }

        [Route ("addupdatereportdata")]
        [HttpPost]
        public IActionResult AddUpdateReportData ([FromBody] ReportModel reportMetaData) {

            ReportRepository _groupRepository = new ReportRepository (_iconfiguration);
            int responce = _groupRepository.DynamicReportMetaInsertUpdate (reportMetaData);

            if (responce != -1) {
                return Json (new RequestResult {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                        Msg = "ok"
                });
            }
            else{
                return Json (new RequestResult {
                    Results = new { Status = RequestState.Failed, Msg = "error" },
                        Msg = "something went wrong"
                });
            }

        }

        [HttpGet ("getreportlist/{tagAltkeys}")]
        public IActionResult getReportList (string tagAltkeys) {
            if(tagAltkeys=="null")
            {
                tagAltkeys = null;
            }
            ReportRepository _groupRepository = new ReportRepository (_iconfiguration);
            var Results = _groupRepository.GetReportListData(tagAltkeys);
            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results

            });
        }
        
        [HttpGet ("getreportquerymetadata/{ReportId}/{UserLoginId}")]
        public IActionResult GetReportQueryMetaData (string ReportId,string UserLoginId) {
            ReportRepository _groupRepository = new ReportRepository (_iconfiguration);

            var Results = _groupRepository.GetReportQueryData(ReportId,UserLoginId);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results
            });
        }

    }
}