using System.Data;
using DynamicReportAPI.Models;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository.groupRepository
{
    public class groupRepository:IgroupRepository
    {
        public IConfiguration _iconfiguration;
        groupsModel _groupsModel;
        public groupRepository(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public DataSet GetGroupMasterData()
        {
            _groupsModel  = new groupsModel(_iconfiguration);
            return _groupsModel.SelectGroupMasterList().GetTableName();
        }

        public object InsertUpdateGroupData(groupsModel modeData)
        {
            _groupsModel  = new groupsModel(_iconfiguration);
            // _groupsModel  =modeData;
            object obj = null;
            obj = _groupsModel.InsertUpdateGroupDetails(modeData);
            return obj;
        }
    }
}