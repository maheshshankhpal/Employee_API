using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository.ItemGroupRepository;
using DynamicReportAPI.Models.Repository.ItemMappingRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DynamicReportAPI.Controllers
{
    [Route("api/[controller]")]
    public class ItemGroupController : Controller
    {

        public IConfiguration _iconfiguration;

        public ItemGroupController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        [HttpGet("getItemList")]
        public IActionResult getItemList()
        {
            ItemGroupRepository _itemGrpRepository = new ItemGroupRepository(_iconfiguration);

            var Results = _itemGrpRepository.GetItemList();

            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg = "ok",
                Data = Results

            });
        }

        [Route("groupCreationInUp")]
        [HttpPost]
        public IActionResult AddUpdateReportData([FromBody] ItemGroupModel groupMetaData)
        {
            
            ItemGroupRepository _itemGrpRepository = new ItemGroupRepository(_iconfiguration);
            string listofnewItems="";
            for (int i=0 ;i < groupMetaData.ItemForCreation.Count ;i++)
            {
               ItemMappingRepository _itemRepository=new ItemMappingRepository (_iconfiguration);
               groupMetaData.ItemForCreation[i].FromScreen="Group";
               if(i==0)
               {
                    groupMetaData.ItemForCreation[i].FirstItem="Y";
                    groupMetaData.ItemForCreation[i].GroupId=groupMetaData.GroupId;

               }
               else
               {
                    groupMetaData.ItemForCreation[i].FirstItem="N";
                    groupMetaData.ItemForCreation[i].GroupId=groupMetaData.GroupId;
               }
               int res = _itemRepository.ItemInsertUpdate (groupMetaData.ItemForCreation[i]);
               if (res ==-1)
               {
                    return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = "error" },
                    Msg = "something went wrong in Item Creation"
                });
               }
               else
               {
                   listofnewItems=listofnewItems + res  + ",";
               }
            }
            if(listofnewItems.Contains(","))
            {
                listofnewItems=listofnewItems.Remove(listofnewItems.Length-1);
            }
            if(listofnewItems!="")
            {
              groupMetaData.ListOfItem=listofnewItems;
            }
            int responce  = _itemGrpRepository.GroupCreationInsertUpdate(groupMetaData);

            if (responce != -1)
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok"
                });
            }
            else
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = "error" },
                    Msg = "something went wrong"
                });
            }

        }

        [HttpGet("getItemListByTag/{tagAltKeys}")]
        public IActionResult getItemListByTag(string tagAltKeys)
        {
            ItemGroupRepository _itemGrpRepository = new ItemGroupRepository(_iconfiguration);

            var Results = _itemGrpRepository.GetItembyTag(tagAltKeys);
            
            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg = "ok",
                Data = Results
            });
        }


    }
}