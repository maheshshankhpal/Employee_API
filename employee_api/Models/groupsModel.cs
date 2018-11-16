using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Models {
    public class groupsModel {
        public IConfiguration _iconfiguration;
        public groupsModel (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public int GroupAlt_key { get; set; }
        public string GroupName { get; set; }
        public string GroupNameShortName { get; set; }
        public string GroupNameShortNameEnum { get; set; }
        public string GroupNameDescription { get; set; }
        public string MapppedLoantype { get; set; }
        public string Menu { get; set; }
        public string Remark { get; set; }
        public int MenuID { get; set; }
        public int OperationFlag { get; set; }
        public string AuthMode { get; set; }

        public DataSet SelectGroupMasterList () {
            string Connection = _iconfiguration.GetValue<string> ("ConnectionStrings:LiveConnection");

            Database database;
            database = new SqlDatabase (Connection);
            DbCommand command = database.GetStoredProcCommand ("DimGroupAllocationList");
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

        public object InsertUpdateGroupDetails (groupsModel datagroupModel) {

            string Connection = _iconfiguration.GetValue<string> ("ConnectionStrings:LiveConnection");

            Database database;
            database = new SqlDatabase (Connection);
            DbCommand command = database.GetStoredProcCommand ("DimGroupAllocationInUp");
            DataSet ds = new DataSet ();
            try {
                using (command) {

                    database.AddInParameter (command, "@GroupAlt_key", System.Data.DbType.Int32, datagroupModel.GroupAlt_key);
                    database.AddInParameter (command, "@GroupName", System.Data.DbType.String, datagroupModel.GroupName);
                    database.AddInParameter (command, "@GroupNameShortName", System.Data.DbType.String, datagroupModel.GroupNameShortName);
                    database.AddInParameter (command, "@GroupNameShortNameEnum", System.Data.DbType.String, datagroupModel.GroupNameShortNameEnum);
                    database.AddInParameter (command, "@GroupNameDescription", System.Data.DbType.String, datagroupModel.GroupNameDescription);
                    database.AddInParameter (command, "@MapppedLoantype", System.Data.DbType.String, datagroupModel.MapppedLoantype);
                    database.AddInParameter (command, "@Menu", System.Data.DbType.String, datagroupModel.Menu);
                    database.AddInParameter (command, "@Remark", System.Data.DbType.String, datagroupModel.Remark);
                    database.AddInParameter (command, "@MenuID", System.Data.DbType.Int32, 0);
                    database.AddInParameter (command, "@OperationFlag", System.Data.DbType.Int32, 1);
                    database.AddInParameter (command, "@AuthMode", System.Data.DbType.String, datagroupModel.AuthMode);
                    database.AddInParameter (command, "@EffectiveFromTimeKey", System.Data.DbType.Int32, 24909);
                    database.AddInParameter (command, "@EffectiveToTimeKey", System.Data.DbType.Int32, 49999);
                    database.AddInParameter (command, "@TimeKey", System.Data.DbType.Int32, 49999);
                    database.AddInParameter (command, "@CrModApBy", System.Data.DbType.String, "Omi");

                    ds = database.ExecuteDataSet (command);
                   // return ds;

                }
            } catch (Exception ex) {
                throw ex;
            }

            JObject DBReturnResult = new JObject ();

            DBReturnResult.Add ("Result", (command.Parameters) [command.Parameters.Count - 1].Value.ToString ());
            DBReturnResult.Add ("D2Ktimestamp", (command.Parameters) [command.Parameters.Count - 2].Value.ToString ());

            return DBReturnResult;
        }
    }
}