using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository.ReportRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicReportAPI.Controllers {
    [Route ("api/[controller]")]
    public class DepartmentController : Controller {

        public IConfiguration _iconfiguration;

        public DepartmentController (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        [Route ("UserGroupDeptInsertUpdate")]
        [HttpPost]
        public IActionResult UserGroupDeptInsertUpdate ([FromBody] UserGroupDeptModel deptModel) {
            DepartmentRepository _itemGrpRepository = new DepartmentRepository (_iconfiguration);
            var response = _itemGrpRepository.UserGroupDeptInUp (deptModel);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = "json"

            });
        }

    }
}