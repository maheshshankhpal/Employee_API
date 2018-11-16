using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository.groupRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Controllers {
    [Route ("api/[controller]")]
    public class GroupController : Controller {
        public IConfiguration _iconfiguration;
        public GroupController (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        [HttpGet ("getGroupMasterList")]
        public IActionResult getGroupsMaster () {
            groupRepository _groupRepository = new groupRepository (_iconfiguration);

            var Results = _groupRepository.GetGroupMasterData ();
            var json = JsonConvert.SerializeObject (Results);

            return Json (new RequestResult {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = json

            });
        }

        [Route ("GroupsInsertUpdate")]
        [HttpPost]
        public IActionResult GroupsInsertUpdate ([FromBody]groupsModel groupdata) {

            groupRepository _groupRepository = new groupRepository(_iconfiguration);

            var Results = _groupRepository.InsertUpdateGroupData(groupdata);
            var json = JsonConvert.SerializeObject(Results);

            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg ="ok",
                Data =json

            });
        }
    }
}