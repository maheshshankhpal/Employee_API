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
using DynamicReportAPI.Models.Repository.userCreationRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserCreationController : Controller
    {
        public IConfiguration _iconfiguration;
        public UserCreationController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        [HttpGet("UserCreationMasterData/{userid}")]
        public IActionResult getUserCreationMaster(string userid)
        {
            userCreationRepository _userCreationRepository = new userCreationRepository(_iconfiguration);

            var Results = _userCreationRepository.GetUserCreationMasterData(userid);
            // var json = JsonConvert.SerializeObject(Results);

            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg = "ok",
                Data = Results

            });
        }

        [Route("UserCreationInsertUpdate")]
        [HttpPost]
        public IActionResult UserCreationInsertUpdate([FromBody] UserCreationModel userCreationData)
        {
            string password = CreatePassword(6);

            if (userCreationData.OperationMode == 1)
            {
                userCreationData.LoginPassword = EncrPassword(password);
            }

            userCreationRepository _userCreationRepository = new userCreationRepository(_iconfiguration);

            int Results = _userCreationRepository.InsertUpdateUserCreationData(userCreationData);
            var json = JsonConvert.SerializeObject(Results);
            if (Results > 0)
            {
                // Send message on 
                if (userCreationData.OperationMode == 1)
                {
                    EmailModel sendmail = new EmailModel();
                    sendmail.To = userCreationData.Email_ID.Replace(";", ",");
                    sendmail.Subject = "User Createtion";
                    sendmail.Email = "d2ktechcloud@gmail.com";
                    sendmail.Password = "miswak@365miswak@365";
                    sendmail.Body = "login ID : " + userCreationData.UserId + "\n" + "Password :" + password;

                    sendmail.sendmail(sendmail);
                }

                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = json
                });
            }
            else
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = "Erorr" },
                    Msg = "ok",
                    Data = null
                });
            }

        }

        [HttpGet("GetUserCreationData/{userid}")]
        public IActionResult GetUserCreationData(string userid)
        {
            userCreationRepository _userCreationRepository = new userCreationRepository(_iconfiguration);

            var Results = _userCreationRepository.GetUserCreationData(userid);
            var json = JsonConvert.SerializeObject(Results);

            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg = "ok",
                Data = json

            });
        }

        [Route("ChangePasswordInsertUpdate")]
        [HttpPost]
        public IActionResult ChangePasswordInsertUpdate([FromBody] UserCreationModel changePasswordData)
        {

            changePasswordData.NewPassword = EncrPassword(changePasswordData.NewPassword);
            //mitali 28082018
            changePasswordData.OldPassword = EncrPassword(changePasswordData.OldPassword);

            userCreationRepository _userCreationRepository = new userCreationRepository(_iconfiguration);

            var Results = _userCreationRepository.ChangePasswordInsertUpdate(changePasswordData);
            var json = JsonConvert.SerializeObject(Results);
            //mitali 28082018
            if ((int)Results == -3)
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = "not ok" },
                    Msg = "ok",
                    Data = json

                });

            }
            else
            {

                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                    Msg = "ok",
                    Data = json

                });

            }
        }

        private string EncrPassword(string _password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            Byte[] bytProducts = uEncode.GetBytes(_password);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(bytProducts);
            return Convert.ToBase64String(hash);
        }
        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}