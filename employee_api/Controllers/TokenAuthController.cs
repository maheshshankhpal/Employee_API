using employee_api.Utility;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using employee_api.Models;
using Newtonsoft.Json.Linq;
using employee_api.Models.Repository.LoginRepository;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using employee_api.Hubs;

namespace employee_api.Controllers
{
    [Route("api/[controller]")]
    public class TokenAuthController : Controller
    {
        public IConfiguration _iconfiguration;
        public TokenAuthController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        [HttpPost("token")]
        public IActionResult token([FromBody]User user)
        {
            User existUser = user;
            string Password = EncrPassword(user.Password);

            Login objlogin = new Login();

            JObject JLoginObj = new JObject();
            JLoginObj.Add("userName", user.Username);
            JLoginObj.Add("Password", Password);
            JLoginObj.Add("authType", "DB");
            JLoginObj.Add("AuthSuccess", "N");

            LoginRepository repository = new LoginRepository(_iconfiguration);
            DataSet UserData = repository.SelectLoginDetails(JLoginObj.ToString());
            UserStatus userLoginstatus = new UserStatus();

            if (UserData.Tables["UserLogin"] != null)
            {
                userLoginstatus = getLoginUserStatus(UserData, Password);
            }
            else
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = "Username or password is invalid" },
                });
            }



            if (userLoginstatus.IsLogin)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
                var token = GenerateToken(existUser, expiresIn);

               // var strGroups = UserData.Tables["UserLogin"].Rows[0]["DeptGroupCode"].ToString();
               // var userGroups = strGroups.Split(',');
                var userFullName = UserData.Tables["UserLogin"].Rows[0]["UserName"];

                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                    Data = new
                    {
                        requertAt = requestAt,
                        expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                        tokeyType = TokenAuthOption.TokenType,
                        accessToken = token,
                        Userinfo = new
                        {
                            UserID = existUser.Username,
                            username = userFullName
                        }
                    }
                });
            }
            else
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = userLoginstatus.Msg },
                });
            }
        }

        private UserStatus getLoginUserStatus(DataSet userData, string Password)
        {
            UserStatus userLoginstatus = new UserStatus();

            bool IsSUSPEND = Convert.ToBoolean(userData.Tables["UserLogin"].Rows[0]["SUSPEND"]);
            bool IsExpiredUser = Convert.ToBoolean(userData.Tables["UserLogin"].Rows[0]["ExpiredUser"]);
            bool isActivate = Convert.ToBoolean(userData.Tables["UserLogin"].Rows[0]["Activate"]);
            
            var IsLoginPassword = userData.Tables["UserLogin"].Rows[0]["LoginPassword"].ToString();


            if (IsSUSPEND)
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "User has been Suspended";
                return userLoginstatus;
            }
            else if (IsExpiredUser)
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "User has been Expired";
                return userLoginstatus;
            }
            else if (!isActivate)
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "User is Not Active";
                return userLoginstatus;
            }
            else if (IsLoginPassword != Password)
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "Invalid Password";
                return userLoginstatus;
            }
            else
            {
                userLoginstatus.IsLogin = true;
                userLoginstatus.Msg = "Login Successfully";
                return userLoginstatus;
            }

        }

        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Username)
                , new[] { new Claim("ID", user.ID.ToString()) }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }
        private string EncrPassword(string _password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            Byte[] bytProducts = uEncode.GetBytes(_password);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(bytProducts);
            return Convert.ToBase64String(hash);
        }
    }

    public class HelpController : Controller
    {
        public IActionResult HelpPage()
        {
            return View();
        }

    }
}