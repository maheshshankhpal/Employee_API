// //using Microsoft.Practices.EnterpriseLibrary.Data;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Data;
// using System.Data.Common;
// using System.Linq;
// using System.Web;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Microsoft.AspNetCore.Authorization;
// using System.Threading.Tasks;

// namespace DynamicReportAPI.Filters
// {

//     public class LogFilter : ActionFilterAttribute
//     {
//         public override void OnActionExecuted(ActionExecutedContext filterContext)
//         {
//             base.OnActionExecuted(filterContext);
//             AddDataToSP Obj = new AddDataToSP();
//             try
//             {
//                 //Obj.APIAreaName = ConfigurationManager.AppSettings["AreaName"].ToString();
//                 Obj.Token = filterContext.HttpContext.Request.Headers["Authorization"].ToString();
//                 Obj.Response = filterContext.HttpContext.Response.Body.ToString();
//                 Obj.ResponseType = filterContext.HttpContext.Request.Method.ToString();
//                 Obj.StatusCode = filterContext.HttpContext.Response.StatusCode.ToString();
//                 Obj.Port = filterContext.HttpContext.Request.Host.Port.ToString();
//                 Obj.AbsoluteUri = filterContext.HttpContext.Request.Path.Value;
//                 Obj.Host = filterContext.HttpContext.Request.Host.Host.ToString();

//                 if (Obj.ResponseType.ToUpper() == "POST")
//                 {
//                     Obj.Param = filterContext.HttpContext.Request.Body.ToString();
//                 }
//                 // Obj.AddToLogTable();
//             }
//             catch (Exception e)
//             {

//             }
//         }
//     }
//     public enum Policy { EndPoint };
//     public class EndPointRequirement : IAuthorizationRequirement
//     {
//         public EndPointRequirement()
//         {
//         }
//     }
//     public class EndPointHandler : AuthorizationHandler<EndPointRequirement>
//     {
//         protected IHttpContextAccessor accessor;
//         public EndPointHandler(IHttpContextAccessor accessor)
//         {
//             this.accessor = accessor;
//         }

//         protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EndPointRequirement requirement)
//         {
//             // Your logic here... or anything else you need to do.
//             string[] splitUrl = accessor.HttpContext.Request.Path.ToString().Split('/');
//             var PathIndex = 0;
//             for (int i = 0; i < splitUrl.Length; i++)
//             {
//                 if (splitUrl[i] == "CrisMAc")
//                     PathIndex = i + 1;
//                 break;
//             }
//             AuthorizeUserDB _AuthorizeUserDB;

//             string UserId = context.User.Identity.Name;
//             _AuthorizeUserDB = new AuthorizeUserDB();
//             if (!_AuthorizeUserDB.SelectAuthorizeUserDB(UserId, splitUrl[PathIndex]))
//             {
//                 context.Fail();
//             }

//             else
//             {
//                 context.Succeed(requirement);
//             }
                       
//         }

//     }



  
//     public class AddDataToSP
//     {

//         public string APIAreaName { get; set; }
//         public string Response { get; set; }
//         public string StatusCode { get; set; }
//         public string Port { get; set; }
//         public string AbsoluteUri { get; set; }
//         public string Host { get; set; }
//         public string ResponseType { get; set; }
//         public string IP { get; set; }
//         public string Device { get; set; }
//         public string Param { get; set; }
//         public string Token { get; set; }

//         //DatabaseProviderFactory factory = new DatabaseProviderFactory();
//         //public DataSet AddToLogTable()
//         //{
//         //    Database database = factory.Create("ConnStringDecr");
//         //    DbCommand command = database.GetStoredProcCommand("WebAPILogInsertUpdate");
//         //    DataSet ds = new DataSet();
//         //    try
//         //    {
//         //        using (command)
//         //        {
//         //            database.AddInParameter(command, "@Status", System.Data.DbType.String, StatusCode);
//         //            database.AddInParameter(command, "@Port", System.Data.DbType.String, Port);
//         //            database.AddInParameter(command, "@Url", System.Data.DbType.String, AbsoluteUri);
//         //            database.AddInParameter(command, "@Response", System.Data.DbType.String, Response);
//         //            database.AddInParameter(command, "@ServerName", System.Data.DbType.String, Host);
//         //            database.AddInParameter(command, "@API", System.Data.DbType.String, APIAreaName);
//         //            database.AddInParameter(command, "@ResponseType", System.Data.DbType.String, ResponseType);
//         //            database.AddInParameter(command, "@IP", System.Data.DbType.String, IP);
//         //            database.AddInParameter(command, "@Device", System.Data.DbType.String, Device);
//         //            database.AddInParameter(command, "@Param", System.Data.DbType.String, Param);
//         //            database.AddInParameter(command, "@Token", System.Data.DbType.String, Token);


//         //            ds = database.ExecuteDataSet(command);
//         //        }
//         //        return ds;
//         //    }
//         //    catch (Exception ex)
//         //    {
//         //        return null;
//         //    }
//         //}
//     }

//     public class AuthorizeUserDB
//     {
//         //DatabaseProviderFactory factory = new DatabaseProviderFactory();

//         public bool SelectAuthorizeUserDB(string UserLoginID, string endPoint)
//         {
//             //Database database = factory.Create("ConnStringDecr");
//             //DbCommand command = database.GetStoredProcCommand("SelectAuthorizeUserDB");
//             //DataSet ds = new DataSet();
//             //try
//             //{
//             //    using (command)
//             //    {
//             //        database.AddInParameter(command, "@UserLoginID", System.Data.DbType.String, UserLoginID);
//             //        database.AddInParameter(command, "@endPoint", System.Data.DbType.String, endPoint);
//             //        ds = database.ExecuteDataSet(command);
//             //    }
//             //    bool flg = ds.Tables[0].Rows.Count > 0 ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UserAccess"]) : false;
//             //    return flg;
//             //}
//             //catch (Exception ex)
//             //{
//             //    return true;
//             //}
//             return true;
//         }

//     }



// }
