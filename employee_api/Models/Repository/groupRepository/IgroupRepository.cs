using System;
using System.Data;

namespace DynamicReportAPI.Models.Repository.groupRepository
{
    public interface IgroupRepository
    {
          DataSet GetGroupMasterData();
          Object InsertUpdateGroupData(groupsModel jsonData); 
    }
}