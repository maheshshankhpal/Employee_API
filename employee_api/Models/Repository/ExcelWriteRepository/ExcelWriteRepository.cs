using System.Data;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository 
{
    public class ExcelWriteRepository {
        public IConfiguration _iconfiguration;
        ExcelWriteModel _excelwriteModel;
        public ExcelWriteRepository (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public DataSet getExcelWriteData (string ReportId, string UserLoginId,int TimeKey) {
            _excelwriteModel = new ExcelWriteModel (_iconfiguration);
            return _excelwriteModel.getExcelWriteData (ReportId, UserLoginId,TimeKey).GetTableName ();
        }

        public DataSet GetFrequencyDate () {
            _excelwriteModel = new ExcelWriteModel (_iconfiguration);
            return _excelwriteModel.GetFrequencyDate ().GetTableName ();
        }
    }
}