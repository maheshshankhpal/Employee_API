using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Models {
    public class UserGroupDeptModel {
        public IConfiguration _iconfiguration;
        public string MenuId { get; set; }
        public int ParentId { get; set; }
        public string MenuCaption { get; set; }
        public int MenuTitled { get; set; }
        public int DataSequence { get; set; }
        public string MsgDescription { get; set; }
        public int EntityKey { get; set; }
        public int DeptGroupId { get; set; }
        public string DeptGroupName { get; set; }
        public string DeptGroupDesc { get; set; }
        public string AvailableFor { get; set; }
        public string IsUniversal { get; set; }
        public string DateCreatedModifiedApproved { get; set; }
        public string CreateModifyApprovedBy { get; set; }
        public int OperationFlag { get; set; }
        public UserGroupDeptModel (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public int UserGroupInsertUpdate (UserGroupDeptModel deptModel) {
            string Connection = _iconfiguration.GetValue<string> ("ConnectionStrings:Connection_SLBC");
            Database database;
            database = new SqlDatabase (Connection);
            DbCommand command = database.GetStoredProcCommand ("UserGroupInsertUpdate");
            using (command) {
                try {
                    // database.AddInParameter(command, "EntityKey", System.Data.DbType.Int32, EntityKey);
                    database.AddInParameter (command, "DeptGroupId", System.Data.DbType.Int32, DeptGroupId);
                    database.AddInParameter (command, "DeptGroupName", System.Data.DbType.String, DeptGroupName);
                    database.AddInParameter (command, "DeptGroupDesc", System.Data.DbType.String, DeptGroupDesc);
                    database.AddInParameter (command, "MenuId", System.Data.DbType.String, MenuId);
                    database.AddInParameter (command, "EffectiveFromTimeKey", System.Data.DbType.Int32, 24992);
                    database.AddInParameter (command, "EffectiveToTimeKey", System.Data.DbType.Int32, 49999);
                    database.AddInParameter (command, "CreateModifyApprovedBy", System.Data.DbType.String, CreateModifyApprovedBy);
                    database.AddInParameter (command, "timekey", System.Data.DbType.Int32, 49999);
                    database.AddInParameter (command, "OperationFlag", System.Data.DbType.Int32, OperationFlag);
                    database.AddInParameter (command, "IsUniversal", System.Data.DbType.String, IsUniversal);
                    //  database.AddInParameter(command, "IsUniversal", System.Data.DbType.String, IsUniversal);
                    database.AddInParameter (command, "DateCreatedModifiedApproved", System.Data.DbType.String, DateTime.UtcNow.ToString ());
                    //  database.AddInParameter(command, "CreateModifyApprovedBy", System.Data.DbType.String, CreateModifyApprovedBy);

                    database.AddOutParameter (command, "Result", System.Data.DbType.Int32, -1);
                    database.ExecuteNonQuery (command);
                } catch (Exception e) {

                }
            }

            return (int) (command.Parameters) [command.Parameters.Count - 1].Value;
        }

    }
}