using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository.ReportRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicReportAPI.Controllers {
    [Route ("api/[controller]")]
    public class MenuController : Controller {

        public IConfiguration _iconfiguration;

        public MenuController (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        [HttpGet ("getCrisMacMenu")]
        public IActionResult getCrisMacMenu () {
            MenuRepository _menuRepository = new MenuRepository (_iconfiguration);

            var Results = _menuRepository.getCrisMacMenuList();

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results
            });
        }

    }
}