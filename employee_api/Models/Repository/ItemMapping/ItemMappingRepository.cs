using System.Data;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository.ItemMappingRepository {
    public class ItemMappingRepository {
        public IConfiguration _iconfiguration;
        ItemMasterModel _itemModel;
        public ItemMappingRepository (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public DataSet GetItemMappingMasterData (string getListypeData) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.SelectReportMappingMasterList(getListypeData).GetTableName ();
        }

        public DataSet GetMasterColumnList (string tableid) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.SelectMasterColumnList(tableid).GetTableName ();
        }
        public int ItemInsertUpdate(ItemMasterModel itemMasterModel)
        {
           // _itemModel  = new ItemMasterModel(_iconfiguration);
            _itemModel = itemMasterModel;
            _itemModel._iconfiguration = _iconfiguration;
            // _groupsModel  =modeData;
             
        }
        public DataSet GetItemGRPMappingMaster(int OperationFlag) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.GetItemGRPMappingMasterList(OperationFlag).GetTableName ();
        }
        public DataSet getGroupItemAltKey(string tagAltkeys) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.GetItemsGroupListByTag(tagAltkeys).GetTableName ();
        }

        public DataSet getItemElementList(string itemid) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.getItemElementList(itemid).GetTableName ();
        }
        public int ItemGRPMappingInsertUpdate (ItemMasterModel data) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.ItemGRPMappingInsertUpdate (data);
        }
        public DataSet GetItemGrpMappingList(string reportid) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.GetItemGrpMappingList(reportid).GetTableName ();
        }
        public DataSet GetItemMappingSearchData(string tagAltKeys) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.GetItemMappingSearchDetails(tagAltKeys).GetTableName ();
        }
         public DataSet GetNextDropDownlistColumDataData(string tableName,string tableColumnName) {
            _itemModel = new ItemMasterModel (_iconfiguration);
            return _itemModel.GetNEXTMasterColumnData(tableName,tableColumnName).GetTableName ();
        }
    }
}