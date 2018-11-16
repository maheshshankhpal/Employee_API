using System;
using System.Data;

namespace DynamicReportAPI.Models.Repository.userCreationRepository {
    public interface IuserCreationRepository {
      DataSet GetUserCreationMasterData(string userid);
        int InsertUpdateUserCreationData(UserCreationModel jsonData);
        DataSet GetUserCreationData(string userid);
        Object ChangePasswordInsertUpdate(UserCreationModel jsonData);
    }
}