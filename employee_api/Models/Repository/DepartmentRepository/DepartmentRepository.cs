using System.Data;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository.ReportRepository {
    public class DepartmentRepository {
        public IConfiguration _iconfiguration;
        UserGroupDeptModel _DeptModel;
        public DepartmentRepository (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public int UserGroupDeptInUp(UserGroupDeptModel deptModel) {
            _DeptModel = new UserGroupDeptModel (_iconfiguration);
            return _DeptModel.UserGroupInsertUpdate(deptModel);
        }
   
    }
}