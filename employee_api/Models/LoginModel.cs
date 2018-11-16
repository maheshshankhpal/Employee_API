
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace DynamicReportAPI.Models
{
  public  class LoginModel : CommonProperties
    {
        public IConfiguration _iconfiguration;
        public LoginModel(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        DatabaseProviderFactory factory = new DatabaseProviderFactory();
        public string userName { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public string LoginSucceeded { get; set; }
        public string authType { get; set; }
        public string AuthSuccess { get; set; }
        public DataSet Select_LoginDetails()
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:LiveConnection");
          
            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("UserAuthentication_AD");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@UserLoginID", System.Data.DbType.String, userName);
                    database.AddInParameter(command, "@LoginPassword", System.Data.DbType.String, Password);
                    database.AddInParameter(command, "@authType", System.Data.DbType.String, authType);       //Auth Type can be AD or DB
                    database.AddInParameter(command, "@AuthSuccess", System.Data.DbType.String, AuthSuccess); //Added to Know AD login is Successful or Not

                    ds = database.ExecuteDataSet(command);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}