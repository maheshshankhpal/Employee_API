using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicReportAPI.Models
{
    public class ItemGroupModel
    {

        public int GroupId { get; set; }
        public string InputJson { get; set; }
        public string UserLoginId { get; set; }
        public string tagAltkeys { get; set; }
        public int OperationFlag { get; set; }
        public string groupName { get; set; }
        public string ListOfItem { get; set; }
        public IConfiguration _iconfiguration;
        public ItemGroupModel(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        
        public List<ItemMasterModel> ItemForCreation { get; set; }

        public DataSet SelectItemList()
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("SelectItemList");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    ds = database.ExecuteDataSet(command);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        
        
        public int GroupCreationDataInUp(ItemGroupModel groupdata)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");
            
           

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("GroupCreationInUp");
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@GroupId", System.Data.DbType.Int32, groupdata.GroupId);
                     database.AddInParameter (command, "@InputJson", System.Data.DbType.String, groupdata.InputJson);
                    database.AddInParameter(command, "@UserLoginId", System.Data.DbType.String, groupdata.UserLoginId);
                    database.AddInParameter(command, "@tagAltkeys", System.Data.DbType.String, groupdata.tagAltkeys);
                    database.AddInParameter(command, "@OperationFlag", System.Data.DbType.Int32, groupdata.OperationFlag);
                    database.AddInParameter(command, "@groupName", System.Data.DbType.String, groupdata.groupName);
                    database.AddInParameter(command, "@ListOfItem", System.Data.DbType.String, groupdata.ListOfItem);

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

        public DataSet SelectItemListByTag(string tagKeys)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("getItemListbyTagAltkey");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@tagAltKeys", System.Data.DbType.String, tagKeys);
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