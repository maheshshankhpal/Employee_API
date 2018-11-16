using System.Data;
using Microsoft.Extensions.Configuration;

namespace employee_api.Models.Repository.ReportRepository {
    public class MenuRepository {
        public IConfiguration _iconfiguration;
        MenuModel _MenuModel;
        public MenuRepository (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public DataSet getCrisMacMenuList () {
            _MenuModel = new MenuModel (_iconfiguration);
            return _MenuModel.GetSideMenuList().GetTableName();
        }
   
    }
}