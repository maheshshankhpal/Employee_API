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
    public class ItemMasterModel
    {

        #region Global Variables
        public IConfiguration _iconfiguration;
        public List<DynamicReportProperties> ItemList { get; set; }
        public string ItemName { get; set; }
        public string OutputType { get; set; }
        public string tagItem { get; set; }
        private string SqlSelect { get; set; }
        private string SQLSelectAggregate { get; set; }
        public string SQLCaseCondition { get; set; }
        private string SqlFrom { get; set; }
        public int OperationFlag { get; set; }
        public int ColumnConditionID { get; set; }
        // public string InputJson { get; set; }
        public string UserLoginId { get; set; }
        public int TimeKey { get; set; }
        public int ItemID { get; set; }
        public string excelRowStartPosition { get; set; }

        public List<ItemGrpMappingProperties> itemGridList { get; set; }
       
        public string FromScreen { get; set; }
        public string FirstItem { get; set; }
        public int GroupId { get; set; }
        #endregion
        public ItemMasterModel(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        private bool prepareItem()
        {
            try
            {
                DynamicReportProperties elementSequence_1 = ItemList.Find(t => t.ElementNature == "Value");

                string caseCondition = string.Empty;
                string joinStmt = string.Empty;

                foreach (var item in ItemList)
                {
                    string condition = string.Empty;
                    if (item.ElementNature.ToLower() != "value")
                    {
                        string condOperator = "";
                        string column = "";
                        string value = "";
                        if (!string.IsNullOrEmpty(item.MasterTable))
                        {
                            condOperator = item.MasterValueOperator;

                            item.SQLJoin = " LEFT JOIN " + item.MasterTable +
                                " ON " + item.MasterRefCol + " = " + item.FactColumn +
                                " AND " + item.MasterTable + ".EffectiveFromTimeKey<=@TimeKey AND " +
                                item.MasterTable + ".EffectiveToTimeKey>=@TimeKey";

                            // if (!joinStmt.Trim().Contains(join.Trim()))
                            //     joinStmt += join;
                            column = item.MasterCol;
                            value = item.MasterValue;
                            //condition = item.MasterCol + condOperator + "'" + item.MasterValue.Replace(",", "','") + "'";
                        }
                        else
                        {
                            column = item.ValueCol;
                            condOperator = item.ValueCompareOperator;
                            value = item.Value;
                            // condition = item.ValueCol + condOperator + "'" + item.Value.Replace(",", "','") + "'";
                        }

                        if (value.ToLower().Contains(" and "))
                        {
                            var values = value.Split(" AND ");
                            value = "";
                            for (int index = 0; index < values.Length; index++)
                            {
                                if (index > 0)
                                    value += " AND ";
                                value += "'" + values[index].Trim() + "'";
                            }
                        }
                        else
                        {
                            if (value.Contains(","))
                            {
                                string[] values = value.Split(",");
                                value = "";
                                foreach (string val in values)
                                {
                                    value += ",'" + val.Trim() + "'";
                                }
                                value = value.Remove(0, 1);
                            }
                            else
                                value = "'" + value + "'";

                            value = "(" + value + ")";
                        }

                        condition = column + " " + condOperator + " " + value;

                        caseCondition = string.IsNullOrEmpty(caseCondition) ? condition : caseCondition + " AND " + condition;
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(item.FactTable) && !string.IsNullOrEmpty(item.ValueRefCol))
                        {

                            item.SQLJoin = "LEFT JOIN " + item.ValueTable +
                                " ON " + item.ValueRefCol + " = " + item.FactColumn +
                                " AND " + item.ValueTable + ".EffectiveFromTimeKey<=@TimeKey AND " +
                                item.ValueTable + ".EffectiveToTimeKey>=@TimeKey";

                            // if (!joinStmt.Trim().Contains(join.Trim()))
                            //     joinStmt += join;
                        }
                    }
                }

                string selectAggregate = "";
                string sqlCaseCondition = "";

                if (OutputType.ToLower() == "count")
                {
                    if (!string.IsNullOrEmpty(caseCondition))
                    {
                        sqlCaseCondition = "When " + caseCondition + " then";
                        selectAggregate = "ISNULL(" + OutputType + "( distinct Case " + sqlCaseCondition + " " + elementSequence_1.ValueCol + " END),0)";
                    }
                    else
                        selectAggregate = "ISNULL(" + OutputType + "( " + elementSequence_1.ValueCol + "),0)";
                }
                else if (OutputType.ToLower() == "sum")
                {
                    if (!string.IsNullOrEmpty(caseCondition))
                    {
                        sqlCaseCondition = "When " + caseCondition + " then";
                        selectAggregate = "ISNULL(" + OutputType + "( Case  " + sqlCaseCondition + " ISNULL(" + elementSequence_1.ValueCol + ",0) END),0)/@Cost";
                    }
                    else
                        selectAggregate = "ISNULL(" + OutputType + "( " + "ISNULL(" + elementSequence_1.ValueCol + ",0)),0)/@Cost";
                }
                else
                {
                    if (!string.IsNullOrEmpty(caseCondition))
                        sqlCaseCondition = "When " + caseCondition + " then";
                }

                SQLSelectAggregate = selectAggregate;
                SQLCaseCondition = sqlCaseCondition;

                if (elementSequence_1 != null)
                    SqlSelect = elementSequence_1.ValueCol;

                // SqlFrom = joinStmt;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public int ItemInsertUpdate()
        {
            if (prepareItem())
            {
                string Json = JsonConvert.SerializeObject(ItemList);

                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("DynamicReportItemInUp");
                DataSet ds = new DataSet();
                try
                {
                    using (command)
                    {
                        database.AddInParameter(command, "@ItemName", System.Data.DbType.String, ItemName);
                        database.AddInParameter(command, "@InputJson", System.Data.DbType.String, Json);
                        database.AddInParameter(command, "@OutputType", System.Data.DbType.String, OutputType);
                        database.AddInParameter(command, "@tagAltKeys", System.Data.DbType.String, tagItem);
                        database.AddInParameter(command, "@SqlSelect", System.Data.DbType.String, SqlSelect);
                        database.AddInParameter(command, "@SQLSelectAggregate", System.Data.DbType.String, SQLSelectAggregate);
                        database.AddInParameter(command, "@SQLCaseCondition", System.Data.DbType.String, SQLCaseCondition);
                        database.AddInParameter(command, "@SqlFrom", System.Data.DbType.String, SqlFrom);
                        database.AddInParameter(command, "@OperationFlag", System.Data.DbType.Int16, OperationFlag);
                        database.AddInParameter(command, "@ItemID", System.Data.DbType.Int32, ItemID);
                        database.AddInParameter(command, "@ColumnConditionID", System.Data.DbType.Int16, ColumnConditionID);
                        database.AddInParameter(command, "@FromScreen", System.Data.DbType.String, FromScreen);
                        database.AddInParameter(command, "@FirstItem", System.Data.DbType.String, FirstItem);
                        database.AddInParameter(command, "@GroupId", System.Data.DbType.Int32, GroupId);
                        database.AddOutParameter(command, "@Result", System.Data.DbType.Int32, -1);
                        ds = database.ExecuteDataSet(command);
                        // return ds;
                        return (int)(command.Parameters)["@Result"].Value;
                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }

            }
            else
                return -1;
        }
        public DataSet SelectReportMappingMasterList(string getListypeData)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("SLBC_Mapping_Master");
            DataSet ds = new DataSet();
            using (command)
            {
                try
                {
                    database.AddInParameter(command, "@actionData", System.Data.DbType.String, getListypeData);
                    ds = database.ExecuteDataSet(command);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public DataSet SelectMasterColumnList(string TableID)
        {
            {
                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("GetTblMasterColumnList");
                DataSet ds = new DataSet();
                using (command)
                    try
                    {
                        database.AddInParameter(command, "@TableAlt_Key", System.Data.DbType.Int32, int.Parse(TableID));

                        ds = database.ExecuteDataSet(command);
                        return ds;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
            }
        }
        public DataSet getItemElementList(string itemid)
        {
            {
                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("SeletItemDetails");
                DataSet ds = new DataSet();
                using (command)
                    try
                    {
                        database.AddInParameter(command, "@ItemID", System.Data.DbType.Int32, int.Parse(itemid));

                        ds = database.ExecuteDataSet(command);
                        return ds;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
            }
        }

        #region Item Mapping 
        // Get GetItemGRPMappingMasterList 
        public DataSet GetItemGRPMappingMasterList(int OperationFlag)
        {
            {
                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("DynamicReportItemMappingSelect");
                DataSet ds = new DataSet();
                using (command)
                    try
                    {
                        database.AddInParameter(command, "@OperationFlag", System.Data.DbType.Int32, OperationFlag);
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
        public int ItemGRPMappingInsertUpdate(ItemMasterModel data)
        {
            string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");
            string JsonData = JsonConvert.SerializeObject(data.itemGridList);
            Database database;
            database = new SqlDatabase(Connection);
            DbCommand command = database.GetStoredProcCommand("DynamicReportItemMappingInUp");
            DataSet ds = new DataSet();
            try
            {
                using (command)
                {
                    database.AddInParameter(command, "@InputJson", System.Data.DbType.String, JsonData);
                    database.AddInParameter(command, "@excelRowStartPosition", System.Data.DbType.String, data.excelRowStartPosition);
                    database.AddInParameter(command, "@UserLoginId", System.Data.DbType.String, data.UserLoginId);
                    database.AddInParameter(command, "@OperationFlag", System.Data.DbType.Int32, data.OperationFlag);
                    database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, TimeKey);
                    database.AddOutParameter(command, "@Result", System.Data.DbType.Int32, -1);
                    ds = database.ExecuteDataSet(command);
                    return (int)(command.Parameters)["@Result"].Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetItemGrpMappingList(string reportid)
        {
            {
                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("SelectItemGrpMappingData");
                DataSet ds = new DataSet();
                using (command)
                    try
                    {
                        database.AddInParameter(command, "@ReportID", System.Data.DbType.String, reportid);
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
        public DataSet GetItemMappingSearchDetails(string tagAltKeys)
        {
            {
                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("GetItemMappingSearchData");
                DataSet ds = new DataSet();
                using (command)
                    try
                    {
                        database.AddInParameter(command, "@UserLoginId", System.Data.DbType.String, UserLoginId);
                        database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, TimeKey);
                        database.AddInParameter(command, "@tagAltKeys", System.Data.DbType.String, tagAltKeys);
                        ds = database.ExecuteDataSet(command);
                        return ds;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
            }
        }
        #endregion

        #region Get Next Column Data List By Table And Columns
        public DataSet GetNEXTMasterColumnData(string tableName, string tableColumnName)
        {
            {
                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("GetMasterColumnData");
                DataSet ds = new DataSet();
                using (command)
                    try
                    {
                        database.AddInParameter(command, "@TableName", System.Data.DbType.String, tableName);
                        database.AddInParameter(command, "@ColumnName", System.Data.DbType.String, tableColumnName);
                        database.AddInParameter(command, "@TimeKey", System.Data.DbType.Int32, 49999);
                        ds = database.ExecuteDataSet(command);
                        return ds;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
            }
        }
        #endregion

        #region Get List of Items and Groups By Tag Alt keys 
        public DataSet GetItemsGroupListByTag(string tagAltkeys)
        {
            {
                string Connection = _iconfiguration.GetValue<string>("ConnectionStrings:Connection_SLBC");

                Database database;
                database = new SqlDatabase(Connection);
                DbCommand command = database.GetStoredProcCommand("GetGroup_ItembyTag");
                DataSet ds = new DataSet();
                using (command)
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
        #endregion
    }
}