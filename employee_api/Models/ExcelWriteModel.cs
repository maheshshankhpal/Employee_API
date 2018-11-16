using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Models {
    public class ExcelWriteModel {
        public IConfiguration _iconfiguration;
        public String ReportId { get; set; }
        public string UserLoginId { get; set; }
        public int OperationFlag { get; set; }
        public int TimeKey { get; set; }

        public ExcelWriteModel (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public DataSet getExcelWriteData (string ReportId, string UserLoginId,int TimeKey) {
            string Connection = _iconfiguration.GetValue<string> ("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase (Connection);
            DbCommand command = database.GetStoredProcCommand ("getExcelWriteData");
            DataSet ds = new DataSet ();
            using (command) {
                try {
                     database.AddInParameter (command, "@ReportId", System.Data.DbType.String, ReportId);
                     database.AddInParameter (command, "@TimeKey", System.Data.DbType.Int32, TimeKey);
                     database.AddInParameter (command, "@UserLoginId", System.Data.DbType.String, UserLoginId);
                     ds = database.ExecuteDataSet (command);
                    return ds;
                } catch (Exception ex) {
                    throw ex;
                }
            }

            
        }

        public DataSet GetFrequencyDate () {
            string Connection = _iconfiguration.GetValue<string> ("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase (Connection);
            DbCommand command = database.GetStoredProcCommand ("GetFrequencyDate");
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