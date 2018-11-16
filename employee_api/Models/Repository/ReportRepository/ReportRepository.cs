using System.Data;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository.ReportRepository {
    public class ReportRepository {
        public IConfiguration _iconfiguration;
        ReportModel _reportModel;
        public ReportRepository (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public DataSet GetReportMasterData (string ScreenName) {
            _reportModel = new ReportModel (_iconfiguration);
            return _reportModel.SelectReportMasterList (ScreenName).GetTableName ();
        }
        public int ExcelFileInUp (string JsonFileData, string UserLoginId, int OperationFlag) {
            _reportModel = new ReportModel (_iconfiguration);
            return _reportModel.ExcelFileInUp (JsonFileData, UserLoginId, OperationFlag);
        }
        public int DynamicReportMetaInsertUpdate (ReportModel reportdata) {
            _reportModel = new ReportModel (_iconfiguration);
            return _reportModel.DynamicReportDataInUp (reportdata);
        }
        public DataSet GetReportListData (string tagAltkeys) {
            _reportModel = new ReportModel (_iconfiguration);
            return _reportModel.SelectReportList (tagAltkeys).GetTableName ();
        }
        public DataSet GetReportQueryData (string ReportId, string UserLoginId) {
            _reportModel = new ReportModel (_iconfiguration);
            return _reportModel.GetReportQueryMetaData(ReportId, UserLoginId).GetTableName();
        }

        public DataSet getExcelList (int stateAltkey,string ScreenName) {
            _reportModel = new ReportModel (_iconfiguration);
            return _reportModel.selectExcelListByStateAltKey (stateAltkey,ScreenName).GetTableName ();
        }
    }
}