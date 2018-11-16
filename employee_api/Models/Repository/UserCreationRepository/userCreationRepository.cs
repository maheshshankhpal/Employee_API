using System.Data;
using DynamicReportAPI.Models;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository.userCreationRepository {
    public class userCreationRepository : IuserCreationRepository {
        public IConfiguration _iconfiguration;
        UserCreationModel _userCreationModel;
        public userCreationRepository (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public DataSet GetUserCreationMasterData (string userid) {
            _userCreationModel = new UserCreationModel (_iconfiguration);
            return _userCreationModel.UserCreationParameterisedMasterData (userid).GetTableName ();
        }

        public int InsertUpdateUserCreationData (UserCreationModel modeData) {
            _userCreationModel = new UserCreationModel (_iconfiguration);
            int obj;
            obj = _userCreationModel.UserCreationInsert (modeData);
            return obj;
        }
        public DataSet GetUserCreationData (string userid) {
            _userCreationModel = new UserCreationModel (_iconfiguration);
            return _userCreationModel.GetUserCreationData (userid).GetTableName ();
        }

        public object ChangePasswordInsertUpdate (UserCreationModel modeData) {
            _userCreationModel = new UserCreationModel (_iconfiguration);
            object obj = null;
            obj = _userCreationModel.ChangePasswordInsertUpdate (modeData);
            return obj;
        }
    }
}