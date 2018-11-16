using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DynamicReportAPI.Models
{
    public class UserCreationModel: CommonProperties
    {
         public IConfiguration _iconfiguration;
        public UserCreationModel(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        DatabaseProviderFactory factory = new DatabaseProviderFactory();
        public string LoginPassword { get; set; }
        public string EmployeeID { get; set; }
        public string IsEmployee { get; set; }
        public string DeptGroupCode { get; set; }
        public string _IsChecker { get; set; }
        
        public string _IsActive { get; set; }
        public string UserLocation { get; set; }
        public string Email_ID { get; set; }
        public string SecuritQsnAlt_Key { get; set; }
        public string SecurityAns { get; set; }
        public string MobileNo { get; set; }
        public string Designation { get; set; }
        public string IsCma { get; set; }
        public string WorkFlowUserRoleAlt_Key { get; set; }
        public string _EmployeeType { get; set; }
        public string _GradeScale { get; set; }
        public string ValidateMobile { get; set; }
        public string UserId { get; set; }
        public string BranchCode { get; set; }
        public string ParentColumnValue { get; set; }

        public string CheckFor { get; set; }
        public string BaseColumnValue { get; set; }
        public string Value { get; set; }
        public string UserName { get; set; }
        public string LocationCode { get; set; }
        public string UserRole { get; set; }
        public string DeptGroupId { get; set; }
        public int DesignationAlt_Key { get; set; }
        public int UserRoleAlt_Key { get; set; }

        public string NewPassword { get; set; }

        //mitali 
        public string OldPassword { get; set; }
        public DataSet UserCreationParameterisedMasterData(string userid)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:LiveConnection");
            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("UserCreationParameterisedMasterData_SLBC");
            DataSet ds = new DataSet();
            try
            {
                using (command)
                {
                    database.AddInParameter(command, "@UserLoginID", System.Data.DbType.String, userid);
                    database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, 24992);
                    database.AddInParameter(command, "@UserCreationModification", System.Data.DbType.String, "Y");
                    ds = database.ExecuteDataSet(command);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int UserCreationInsert(UserCreationModel _obj)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:LiveConnection");
            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("dbo.UserCreationInsert");

            try
            {
                using (command)
                {
                    database.AddInParameter(command, "@UserLoginID", System.Data.DbType.String, _obj.UserId);
                    database.AddInParameter(command, "@EmployeeID", System.Data.DbType.String, _obj.UserId);
                    database.AddInParameter(command, "@IsEmployee", System.Data.DbType.String, 'Y');
                    database.AddInParameter(command, "@UserName", System.Data.DbType.String, _obj.UserName);
                    database.AddInParameter(command, "@LoginPassword", System.Data.DbType.String, _obj.LoginPassword);
                    database.AddInParameter(command, "@UserLocation", System.Data.DbType.String, _obj.UserLocation);
                    database.AddInParameter(command, "@UserLocationCode", System.Data.DbType.String, _obj.LocationCode);
                    database.AddInParameter(command, "@UserRoleAlt_Key", System.Data.DbType.Int32, _obj.UserRoleAlt_Key);
                    database.AddInParameter(command, "@DeptGroupCode", System.Data.DbType.String, _obj.DeptGroupId);
                    database.AddInParameter(command, "@DateCreatedmodified", System.Data.DbType.Date, DateTime.Now);
                    database.AddInParameter(command, "@CreatedModifiedBy", System.Data.DbType.String, "Omi568");
                    database.AddInParameter(command, "@Activate", System.Data.DbType.String, _obj._IsActive);
                    database.AddInParameter(command, "@IsChecker", System.Data.DbType.String, _obj._IsChecker);
                    database.AddInParameter(command, "@WorkFlowUserRoleAlt_Key", System.Data.DbType.Int16, 1);
                    database.AddInParameter(command, "@DesignationAlt_Key", System.Data.DbType.Int16, _obj.DesignationAlt_Key); //ad3
                    database.AddInParameter(command, "@IsCma", System.Data.DbType.String, _obj.IsCma);
                    database.AddInParameter(command, "@MobileNo", System.Data.DbType.String, _obj.MobileNo);
                    database.AddInParameter(command, "@Email_ID", System.Data.DbType.String, _obj.Email_ID);
                    database.AddInParameter(command, "@SecuritQsnAlt_Key", System.Data.DbType.String, _obj.SecuritQsnAlt_Key);
                    database.AddInParameter(command, "@SecurityAns", System.Data.DbType.String, _obj.SecurityAns);
                    database.AddInParameter(command, "@MenuId", System.Data.DbType.Int32, 1);
                    database.AddInParameter(command, "@EffectiveFromTimeKey", System.Data.DbType.Int32, 24992);
                    database.AddInParameter(command, "@EffectiveToTimeKey", System.Data.DbType.Int32, 49999);
                    database.AddInParameter(command, "@Flag", System.Data.DbType.Int32, _obj.OperationMode);// _obj.OperationMode
                    database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, 24992);
                    database.AddOutParameter(command, "@Result", System.Data.DbType.Int32, -1);

                    database.ExecuteNonQuery(command);
                }
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            }
            return (int)(command.Parameters)[21].Value;
        }
        public DataSet GetUserCreationData(string userid)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:LiveConnection");
            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("UserModificationAuxSelect");
            DataSet ds = new DataSet();
            try
            {
                using (command)
                {
                    database.AddInParameter(command, "@UserLoginID", System.Data.DbType.String, userid);
                    database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, 24992);
                    ds = database.ExecuteDataSet(command);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int ChangePasswordInsertUpdate(UserCreationModel _obj)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:LiveConnection");
            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("dbo.UserChangePasswordUpdate");

            try
            {
                using (command)
                {
                    database.AddInParameter(command, "UserLoginID", System.Data.DbType.String, _obj.UserId);
                    database.AddInParameter(command, "TimeKey", System.Data.DbType.Int32, 24968);
                    database.AddInParameter(command, "LoginPassword", System.Data.DbType.String, _obj.NewPassword);
                    //mitali 28082018
                    database.AddInParameter(command, "OldPassword", System.Data.DbType.String, _obj.OldPassword);
                    database.AddInParameter(command, "PasswordChangeDate", System.Data.DbType.DateTime, System.DateTime.Now);
                    database.AddInParameter(command, "EffectiveFromTimeKey", System.Data.DbType.Int32, 24968);
                    database.AddInParameter(command, "EffectiveToTimeKey", System.Data.DbType.Int32, 49999);
                    database.AddOutParameter(command, "@Result", System.Data.DbType.Int32, -1);

                    database.ExecuteNonQuery(command);
                }
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            }
            return (int)(command.Parameters)[7].Value;
        }
    }
}