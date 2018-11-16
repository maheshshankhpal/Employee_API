using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository.ItemMappingRepository;
using DynamicReportAPI.Models.Repository.ReportRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicReportAPI.Controllers {
    [Route ("api/[controller]")]
    public class ItemMappingController : Controller {

        public IConfiguration _iconfiguration;

        public ItemMappingController (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        [HttpGet ("getItemMasters/{dataTypelist}")]
        public IActionResult getItemMasters (string dataTypelist) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);

            var Results = _itemRepository.GetItemMappingMasterData (dataTypelist);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results

            });
        }

        [HttpGet ("getMasterColumnList/{tableid}")]
        public IActionResult getMasterColumnList (string tableid) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);

            var Results = _itemRepository.GetMasterColumnList (tableid);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results

            });
        }

        [Route ("itemInsertUpdate")]
        [HttpPost]
        public IActionResult itemInsertUpdate ([FromBody] ItemMasterModel itemMasterModel) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);
            int responce = _itemRepository.ItemInsertUpdate (itemMasterModel);

            if (responce != -1) {
                return Json (new RequestResult {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                        Msg = "ok"
                });
            } else {
                return Json (new RequestResult {
                    Results = new { Status = RequestState.Failed, Msg = "error" },
                        Msg = "something went wrong"
                });
            }

        }

        [HttpGet ("GetItemElementByitem/{itemid}")]
        public IActionResult ItemElementList (string itemid) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);

            var Results = _itemRepository.getItemElementList (itemid);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results

            });
        }

        [HttpGet ("GetItemGRPMappingMaster/{OperationFlag}")]
        public IActionResult GetItemGRPMaster (int OperationFlag) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);
            var Results = _itemRepository.GetItemGRPMappingMaster(OperationFlag);
            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results
            });
        }

        [HttpGet ("GetItemAndGroupList/{tagAltKeys}")]
        public IActionResult getitemAndgrouplistbyTagAltKey (string tagAltKeys) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);
            var Results = _itemRepository.getGroupItemAltKey(tagAltKeys);
            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results
            });
        }

        [Route ("ItemGrpMappingInUp")]
        [HttpPost]
        public IActionResult ItemGrpMappingInUp ([FromBody] ItemMasterModel data) {

            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);
            int responce = _itemRepository.ItemGRPMappingInsertUpdate (data);

            if (responce != -1) {
                return Json (new RequestResult {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                        Msg = "ok"
                });
            } else {
                return Json (new RequestResult {
                    Results = new { Status = RequestState.Failed, Msg = "error" },
                        Msg = "something went wrong"
                });
            }

        }

        [HttpGet ("GetItemGrpMappingData/{reportid}")]
        public IActionResult GetItemGrpMappingData (string reportid) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);

            var Results = _itemRepository.GetItemGrpMappingList (reportid);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results

            });
        }

        [HttpGet ("GetItemMappingGridData/{tagAltKeys}")]
        public IActionResult GetItemMappingGridData (string tagAltKeys) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);
          if(tagAltKeys=="null"){
              tagAltKeys = null;
          }
            var Results = _itemRepository.GetItemMappingSearchData (tagAltKeys);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results
            });
        }

        [HttpGet ("GetMasterColumnDataByTableAndColumn/{tableName}/{tableColumnName}")]
        public IActionResult GetMasterColumnDataByTableAndColumn (string tableName, string tableColumnName) {
            ItemMappingRepository _itemRepository = new ItemMappingRepository (_iconfiguration);

            var Results = _itemRepository.GetNextDropDownlistColumDataData (tableName, tableColumnName);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = Results
            });
        }
    }
}