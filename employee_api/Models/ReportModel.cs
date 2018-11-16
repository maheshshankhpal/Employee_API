using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Models
{
    public class ReportModel
    {
        public IConfiguration _iconfiguration;
        public int ExcelId { get; set; }
        public string InputJson { get; set; }
        public string UserLoginId { get; set; }
        public int OperationFlag { get; set; }
        // public string ScreenName { get; set; }
        public int TimeKey { get; set; }
        public ReportModel(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public DataSet SelectReportMasterList(string ScreenName)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("SLBC_master");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@ScreenName", System.Data.DbType.String, ScreenName);
                    ds = database.ExecuteDataSet(command);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int ExcelFileInUp(string FileJson, string userID, int OperationFlag)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("ExcelDirectoryInUp");
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@InputJson", System.Data.DbType.String, FileJson);
                    database.AddInParameter(command, "@UserLoginId", System.Data.DbType.String, userID);
                    database.AddInParameter(command, "@OperationFlag", System.Data.DbType.Int32, OperationFlag);
                    database.AddOutParameter(command, "@Result", System.Data.DbType.Int32, -1);

                    database.ExecuteNonQuery(command);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return (int)(command.Parameters)["@Result"].Value;
            }
        }

        public int DynamicReportDataInUp(ReportModel dynamicReportMeta)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("DynamicReportMetaInUp");
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@ExcelId", System.Data.DbType.Int32, dynamicReportMeta.ExcelId);
                    database.AddInParameter(command, "@InputJson", System.Data.DbType.String, dynamicReportMeta.InputJson);
                    database.AddInParameter(command, "@UserLoginId", System.Data.DbType.String, dynamicReportMeta.UserLoginId);
                    database.AddInParameter(command, "@OperationFlag", System.Data.DbType.Int32, dynamicReportMeta.OperationFlag);
                    database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, TimeKey);
                    database.AddOutParameter(command, "@Result", System.Data.DbType.Int32, -1);
                    database.ExecuteNonQuery(command);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return (int)(command.Parameters)[4].Value;
            }
        }
        public DataSet SelectReportList(string tagAltkeys)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("GetReportList");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@tagAltKeys", System.Data.DbType.String, tagAltkeys);
                    ds = database.ExecuteDataSet(command);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataSet GetReportQueryMetaData(string ReportId, string UserLoginId)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("DynamicReportMetaSelect");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@ReportId", System.Data.DbType.String, ReportId);
                    database.AddInParameter(command, "@UserLoginId", System.Data.DbType.String, UserLoginId);
                    database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, TimeKey);
                    ds = database.ExecuteDataSet(command);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //mitali 30082019
        public DataSet selectExcelListByStateAltKey(int stateAltkey, string ScreenName)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("getExcelFileListInStateId");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@stateAltkey", System.Data.DbType.Int32, stateAltkey);
                    database.AddInParameter(command, "@ScreenName", System.Data.DbType.String, ScreenName);
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