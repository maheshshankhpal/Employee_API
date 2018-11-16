using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Models
{
    public class TagModel
    {

        public string tagName { get; set; }
        public string createdBy { get; set; }
        public string description { get; set; }
        public int? id { get; set; }
        public int? operationFlag { get; set; }
        public IConfiguration _iconfiguration;


        public TagModel(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }


        public DataSet insertUpdateTagData(TagModel tagModel)
        {

            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("InUpTag");
            DataSet ds = new DataSet();
            try
            {
                using (command)
                {

                    database.AddInParameter(command, "@tagName", System.Data.DbType.String, tagModel.tagName);
                    database.AddInParameter(command, "@tagDescription", System.Data.DbType.String, tagModel.description);
                    database.AddInParameter(command, "@createdModifyBy", System.Data.DbType.String, tagModel.createdBy);
                    database.AddInParameter(command, "@operationFlag", System.Data.DbType.Int32, tagModel.operationFlag);
                    ds = database.ExecuteDataSet(command);
                    return ds;

                }
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getTagDataList()
        {

            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("SelectTagList");
            DataSet ds = new DataSet();
            try
            {
                using (command)
                {
                    ds = database.ExecuteDataSet(command);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
    }
}