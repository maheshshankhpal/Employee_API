using System.Data;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository.ItemGroupRepository
{
    public class ItemGroupRepository
    {
        public IConfiguration _iconfiguration;
        ItemGroupModel _itemGrpModel;
        public ItemGroupRepository(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public DataSet GetItemList()
        {
            _itemGrpModel = new ItemGroupModel(_iconfiguration);
            return _itemGrpModel.SelectItemList().GetTableName();
        }
        public int GroupCreationInsertUpdate(ItemGroupModel groupdata)
        {
            _itemGrpModel = new ItemGroupModel(_iconfiguration);
            return _itemGrpModel.GroupCreationDataInUp(groupdata);
        }
        public DataSet GetItembyTag(string tagKeys)
        {
            _itemGrpModel = new ItemGroupModel(_iconfiguration);
            return _itemGrpModel.SelectItemListByTag(tagKeys).GetTableName();
        }
    }
}