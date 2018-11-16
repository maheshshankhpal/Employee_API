using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;

namespace employee_api.Models {
    public class MenuModel {
        public IConfiguration _iconfiguration;
       

        public MenuModel (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }


        public DataSet GetSideMenuList () {
            string Connection = _iconfiguration.GetValue<string> ("ConnectionStrings");

            Database database;
            database = new SqlDatabase (Connection);
            DbCommand command = database.GetStoredProcCommand ("GetMenuList");
            DataSet ds = new DataSet ();
            using (command) {
                try {
                    ds = database.ExecuteDataSet (command);
                    return ds;
                } catch (Exception ex) {
                    throw ex;
                }
            }
        }
    }
}