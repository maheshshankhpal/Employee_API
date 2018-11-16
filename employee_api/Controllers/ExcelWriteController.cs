using DynamicReportAPI.Models;
using DynamicReportAPI.Models.Repository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
namespace DynamicReportAPI.Controllers
{
    [Route("api/[controller]")]
    public class ExcelWrite : Controller
    {

        public IConfiguration _iconfiguration;
        private IHostingEnvironment _hostingEnvironment;
        DataSet[] Results;
        public ExcelWrite(IHostingEnvironment env, IConfiguration iconfiguration)
        {
            _hostingEnvironment = env;
            _iconfiguration = iconfiguration;
        }

        [HttpGet("getexcelwritedata/{ReportId}/{UserLoginId}/{TimeKey}")]
        public IActionResult getExcelWriteData(string ReportId, string UserLoginId, int TimeKey)
        {

            ExcelWriteRepository _ExcelWriteRepository = new ExcelWriteRepository(_iconfiguration);
            string[] _reportList = ReportId.Split(",");
            DataSet[] myDataSets = new DataSet[_reportList.Length];

            for (int index = 0; index < _reportList.Length; index++)
            {

                DataSet Result = _ExcelWriteRepository.getExcelWriteData(_reportList[index], UserLoginId, TimeKey);
                myDataSets[index] = Result;
            }

            var FinalResult = WriteExcel(myDataSets);
            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg = "ok",
                Data = FinalResult

            });
        }

        [HttpGet("GetFrequencyDate")]
        public IActionResult GetFrequencyDate()
        {
            ExcelWriteRepository _ExcelWriteRepository = new ExcelWriteRepository(_iconfiguration);

            var Results = _ExcelWriteRepository.GetFrequencyDate();

            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg = "ok",
                Data = Results

            });
        }


        private object WriteExcel(DataSet[] myDataSets)
        {
            try
            {
                string filePath = myDataSets[0].Tables[0].Rows[0]["ExcelFilePath"].ToString();
                string fileName = myDataSets[0].Tables[0].Rows[0]["ExcelName"].ToString();
                string IsRepeat = myDataSets[0].Tables[0].Rows[0]["IsRepeat"].ToString();
                string fileExtension = fileName.Substring(fileName.LastIndexOf("."));
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = webRootPath + "" + filePath.Substring(filePath.LastIndexOf("/upload"));
                newPath = newPath.Replace("/", "\\");

                FileInfo newFilePath = new FileInfo(newPath.Substring(0, newPath.LastIndexOf(".")) + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + fileExtension);
                string downloadFilePath = filePath.Substring(0, filePath.LastIndexOf(".")) + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + fileExtension;

                FileInfo file = new FileInfo(newPath);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    for (int index = 0; index < myDataSets.Length; index++)
                    {

                        DataSet myDataSet = myDataSets[index];


                        string excelSheetName = myDataSet.Tables[0].Rows[0]["SheetName"].ToString();
                        string SqlGroupBy = myDataSet.Tables[0].Rows[0]["SqlGroupBy"].ToString();
                        string ReportType = myDataSet.Tables[0].Rows[0]["ReportType"].ToString();

                        string GroupBy = SqlGroupBy.Substring(SqlGroupBy.LastIndexOf(".") + 1);
                        int ExcelRowStartPosition = Convert.ToInt32(myDataSet.Tables[0].Rows[0]["ExcelRowStartPosition"].ToString());

                        int _rowindex = 0;

                        string _cell;


                        ExcelWorksheet worksheet = package.Workbook.Worksheets[excelSheetName];

                        DataTable _DataTable = new DataTable();

                        _DataTable = myDataSet.Tables[2].Copy();

                        for (int i = 3; i < myDataSet.Tables.Count; i++)
                        {
                            _DataTable = JoinTwoDataTablesOnOneColumn(_DataTable, myDataSet.Tables[i], GroupBy).Copy();

                        }

                        if (ReportType == "Single Line")
                        {

                            DataColumnCollection columns = _DataTable.Columns;
                            int _ExcelRowStartPosition = ExcelRowStartPosition;
                            int _IsRowBreakTotal = 0, IsRowBreakTotal = 0, PrvwriteEndCell = 0, RepeatLastcellWritten = 0;
                            foreach (DataRow row in _DataTable.Rows)
                            {
                                foreach (DataColumn headername in columns)
                                {
                                    _cell = headername.ToString() + "" + _ExcelRowStartPosition;

                                    try
                                    {
                                        worksheet.Cells[_cell].Value = row[headername];
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }



                                if (IsRepeat == "True")
                                {
                                    foreach (DataRow RowBreakrow in myDataSet.Tables[1].Rows)
                                    {
                                        if ((_ExcelRowStartPosition - (ExcelRowStartPosition - 1)) == ((myDataSet.Tables[2].Rows.Count < (Convert.ToInt32(RowBreakrow["RowNumber"]) - 1)) ? myDataSet.Tables[2].Rows.Count : (Convert.ToInt32(RowBreakrow["RowNumber"]) - 1)))
                                        {
                                            if (myDataSet.Tables[2].Rows.Count < (Convert.ToInt32(RowBreakrow["RowNumber"]) - 1))
                                                IsRowBreakTotal = ExcelRowStartPosition - 1 + (Convert.ToInt32(RowBreakrow["RowNumber"]) - 1) + Convert.ToInt32(RowBreakrow["TotalOnRow"]);
                                            else
                                                IsRowBreakTotal = _ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["TotalOnRow"]);

                                            foreach (DataColumn headername in columns)
                                            {
                                                _cell = headername.ToString() + "" + IsRowBreakTotal;

                                                try
                                                {



                                                    double value;
                                                    bool IsInt = false;
                                                    for (int j = ExcelRowStartPosition; j <= _ExcelRowStartPosition; j++)
                                                    {
                                                        var _tempcell = headername.ToString() + "" + j;
                                                        if ((worksheet.Cells[_tempcell].Value).ToString() != "" && double.TryParse((worksheet.Cells[_tempcell].Value).ToString(), out value))
                                                        {
                                                            j = _ExcelRowStartPosition + 1;
                                                            IsInt = true;
                                                        }
                                                    }
                                                    if (IsInt)
                                                    {
                                                        if (Convert.ToBoolean(RowBreakrow["IsRowBreakTotal"]))
                                                            worksheet.Cells[_cell].Formula =
                                                          "SUM("
                                                          + headername.ToString() + "" + ExcelRowStartPosition
                                                          + ":"
                                                          + headername.ToString() + "" + _ExcelRowStartPosition
                                                          + ")";
                                                    }

                                                }
                                                catch (Exception e)
                                                {

                                                }
                                            }

                                            _ExcelRowStartPosition = _ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["RowBreakCount"]);//Set next cell to write
                                            ExcelRowStartPosition = ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["RowBreakCount"]);//increase Possition to get first cell to cal total 
                                            RepeatLastcellWritten = _ExcelRowStartPosition;
                                        }

                                        if ((_ExcelRowStartPosition - (ExcelRowStartPosition - 1)) > (Convert.ToInt32(RowBreakrow["RowNumber"]) - 1))//Check Current Cell greater than row no
                                        {
                                            if ((_ExcelRowStartPosition - (ExcelRowStartPosition - 1) - (Convert.ToInt32(RowBreakrow["RowNumber"]) - 1)) % (Convert.ToInt32(RowBreakrow["RepeatBreakNumber"])) == 0)//Check Cell Bunch as per RepeatBreakNumber
                                            {

                                                IsRowBreakTotal = _ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["TotalOnRow"]);//Get Cell to write To Total

                                                foreach (DataColumn headername in columns)
                                                {
                                                    _cell = headername.ToString() + "" + IsRowBreakTotal;

                                                    try
                                                    {


                                                        double value;
                                                        bool IsInt = false;
                                                        for (int j = (_ExcelRowStartPosition - Convert.ToInt32(RowBreakrow["RepeatBreakNumber"]) + 1); j <= _ExcelRowStartPosition; j++)
                                                        {
                                                            var _tempcell = headername.ToString() + "" + j;
                                                            if ((worksheet.Cells[_tempcell].Value).ToString() != "" && double.TryParse((worksheet.Cells[_tempcell].Value).ToString(), out value))
                                                            {
                                                                j = _ExcelRowStartPosition + 1;
                                                                IsInt = true;
                                                            }
                                                        }
                                                        if (IsInt)
                                                        {
                                                            if (Convert.ToBoolean(RowBreakrow["IsRowBreakTotal"]))
                                                                worksheet.Cells[_cell].Formula =
                                                               "SUM("
                                                               + headername.ToString() + "" + (_ExcelRowStartPosition - Convert.ToInt32(RowBreakrow["RepeatBreakNumber"]) + 1)
                                                               + ":"
                                                               + headername.ToString() + "" + _ExcelRowStartPosition
                                                               + ")";
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }

                                                }

                                                _ExcelRowStartPosition = _ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["RowBreakCount"]);//Set next cell to write
                                                ExcelRowStartPosition = ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["RowBreakCount"]);//increase Possition to get first cell to cal total 
                                                RepeatLastcellWritten = _ExcelRowStartPosition;
                                            }
                                            else
                                            {
                                                if (row == _DataTable.Rows[_DataTable.Rows.Count - 1])
                                                {
                                                    IsRowBreakTotal = (RepeatLastcellWritten + 1) + Convert.ToInt32(RowBreakrow["RepeatBreakNumber"]) + Convert.ToInt32(RowBreakrow["TotalOnRow"]) - 1;//Get Cell to write To Total

                                                    foreach (DataColumn headername in columns)
                                                    {
                                                        _cell = headername.ToString() + "" + IsRowBreakTotal;

                                                        try
                                                        {


                                                            double value;
                                                            bool IsInt = false;
                                                            for (int j = (RepeatLastcellWritten + 1); j <= _ExcelRowStartPosition; j++)
                                                            {
                                                                var _tempcell = headername.ToString() + "" + j;
                                                                if ((worksheet.Cells[_tempcell].Value).ToString() != "" && double.TryParse((worksheet.Cells[_tempcell].Value).ToString(), out value))
                                                                {
                                                                    j = _ExcelRowStartPosition + 1;
                                                                    IsInt = true;
                                                                }
                                                            }
                                                            if (IsInt)
                                                            {
                                                                if (Convert.ToBoolean(RowBreakrow["IsRowBreakTotal"]))
                                                                    worksheet.Cells[_cell].Formula =
                                                                  "SUM("
                                                                  + headername.ToString() + "" + (RepeatLastcellWritten + 1)
                                                                  + ":"
                                                                  + headername.ToString() + "" + _ExcelRowStartPosition
                                                                  + ")";
                                                            }
                                                        }
                                                        catch (Exception e)
                                                        {

                                                        }

                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (_rowindex < myDataSet.Tables[1].Rows.Count)
                                    {
                                        DataRow RowBreakrow = myDataSet.Tables[1].Rows[_rowindex];
                                        if ((_ExcelRowStartPosition - ExcelRowStartPosition) == (Convert.ToInt32(RowBreakrow["RowNumber"]) - 1) - 1)
                                        {
                                            if (PrvwriteEndCell == 0)
                                                _IsRowBreakTotal = ExcelRowStartPosition - 1;
                                            else
                                                _IsRowBreakTotal = PrvwriteEndCell;

                                            IsRowBreakTotal = _ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["TotalOnRow"]);
                                            PrvwriteEndCell = IsRowBreakTotal + (Convert.ToInt32(RowBreakrow["RowBreakCount"]) - Convert.ToInt32(RowBreakrow["TotalOnRow"]));
                                            var IsSum = false;
                                            foreach (DataColumn headername in columns)
                                            {
                                                _cell = headername.ToString() + "" + IsRowBreakTotal;

                                                try
                                                {

                                                    double value;
                                                    bool IsInt = false;
                                                    for (int j = (_IsRowBreakTotal + 1); j <= _ExcelRowStartPosition; j++)
                                                    {
                                                        var _tempcell = headername.ToString() + "" + j;
                                                        if ((worksheet.Cells[_tempcell].Value).ToString() != "" && double.TryParse((worksheet.Cells[_tempcell].Value).ToString(), out value))
                                                        {
                                                            j = _ExcelRowStartPosition + 1;
                                                            IsInt = true;
                                                        }
                                                    }
                                                    if (IsInt)
                                                    {
                                                        if (Convert.ToBoolean(RowBreakrow["IsRowBreakTotal"]))
                                                            worksheet.Cells[_cell].Formula =
                                                         "SUM("
                                                         + headername.ToString() + "" + (_IsRowBreakTotal + 1)
                                                         + ":"
                                                         + headername.ToString() + "" + _ExcelRowStartPosition
                                                         + ")";
                                                        IsSum = true;

                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }
                                            }
                                            if (IsSum)
                                            {
                                                _rowindex = _rowindex + 1;
                                                ExcelRowStartPosition = _ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["RowBreakCount"]) + 1;
                                            }
                                            _ExcelRowStartPosition = _ExcelRowStartPosition + Convert.ToInt32(RowBreakrow["RowBreakCount"]);

                                        }

                                        else
                                        {
                                            if (row == _DataTable.Rows[_DataTable.Rows.Count - 1])
                                            {
                                                int RowNumberlast = 0;
                                                int TotalOnRow = 0;
                                                foreach (DataRow RowBreakrowTemp in myDataSet.Tables[1].Rows)
                                                {

                                                    if ((_ExcelRowStartPosition - ExcelRowStartPosition) < Convert.ToInt32(RowBreakrow["RowNumber"]) - 1)
                                                    {
                                                        if (RowNumberlast == 0)
                                                        {
                                                            RowNumberlast = Convert.ToInt32(RowBreakrow["RowNumber"]) - 1;
                                                            TotalOnRow = Convert.ToInt32(RowBreakrow["TotalOnRow"]);
                                                        }
                                                        else
                                                        {
                                                            if (RowNumberlast > Convert.ToInt32(RowBreakrow["RowNumber"]) - 1)
                                                                RowNumberlast = Convert.ToInt32(RowBreakrow["RowNumber"]) - 1;
                                                            TotalOnRow = Convert.ToInt32(RowBreakrow["TotalOnRow"]);
                                                        }
                                                    }
                                                }
                                                if (RowNumberlast != 0)
                                                {
                                                    if (PrvwriteEndCell == 0)
                                                        _IsRowBreakTotal = ExcelRowStartPosition - 1;
                                                    else
                                                        _IsRowBreakTotal = PrvwriteEndCell;

                                                    IsRowBreakTotal = Convert.ToInt32(RowNumberlast) + 1 + Convert.ToInt32(TotalOnRow);
                                                    foreach (DataColumn headername in columns)
                                                    {
                                                        _cell = headername.ToString() + "" + IsRowBreakTotal;

                                                        try
                                                        {

                                                            double value;
                                                            bool IsInt = false;
                                                            for (int j = (_IsRowBreakTotal + 1); j <= _ExcelRowStartPosition; j++)
                                                            {
                                                                var _tempcell = headername.ToString() + "" + j;
                                                                if ((worksheet.Cells[_tempcell].Value).ToString() != "" && double.TryParse((worksheet.Cells[_tempcell].Value).ToString(), out value))
                                                                {
                                                                    j = _ExcelRowStartPosition + 1;
                                                                    IsInt = true;
                                                                }
                                                            }
                                                            if (IsInt)
                                                            {
                                                                worksheet.Cells[_cell].Formula =
                                                             "SUM("
                                                             + headername.ToString() + "" + (_IsRowBreakTotal + 1)
                                                             + ":"
                                                             + headername.ToString() + "" + _ExcelRowStartPosition
                                                             + ")";
                                                                _rowindex = _rowindex + 1;
                                                            }
                                                        }
                                                        catch (Exception e)
                                                        {

                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }


                                _ExcelRowStartPosition++;
                            }

                        }
                        else if (ReportType == "Multiline")
                        {

                            DataColumnCollection columns = _DataTable.Columns;
                            foreach (DataRow row in _DataTable.Rows)
                            {
                                foreach (DataColumn headername in columns)
                                {
                                    _cell = headername.ToString() + "" + row["ExcelRowPosition"];

                                    try
                                    {
                                        worksheet.Cells[_cell].Value = row[headername];
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }


                        }


                    }

                    package.SaveAs(newFilePath);

                    object returnObject = new { downloadFilePath = downloadFilePath, result = 1 };

                    return returnObject;
                }
            }
            catch (Exception e)
            {

                if (e.ToString().Contains("401"))
                {
                    Response.StatusCode = 401;
                    return e.ToString();
                }
                else
                {
                    object returnObject = new { downloadFilePath = "", result = 0 };

                    return returnObject;
                }

            }
        }

        private static DataTable JoinTwoDataTablesOnOneColumn(DataTable dtblLeft, DataTable dtblRight, string colToJoinOn)
        {


            string strTempColName = colToJoinOn + "_2";
            if (dtblRight.Columns.Contains(colToJoinOn))
                dtblRight.Columns[colToJoinOn].ColumnName = strTempColName;

            //Get columns from dtblLeft
            DataTable dtblResult = dtblLeft.Clone();

            string[] columnNames1 = (from dc in dtblLeft.Columns.Cast<DataColumn>()
                                     select dc.ColumnName).ToArray();
            for (int i = 0; i < columnNames1.Length; i++)
            {
                if (dtblRight.Columns.Contains(columnNames1[i]) && columnNames1[i] != colToJoinOn)
                {
                    dtblRight.Columns.Remove(columnNames1[i]);
                }
            }

            //Get columns from dtblRight
            var dt2Columns = dtblRight.Columns.OfType<DataColumn>().Select(dc => new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));

            //Get columns from dtblRight that are not in dtblLeft
            var dt2FinalColumns = from dc in dt2Columns.AsEnumerable()
                                  where !dtblResult.Columns.Contains(dc.ColumnName)
                                  select dc;

            //Add the rest of the columns to dtblResult
            dtblResult.Columns.AddRange(dt2FinalColumns.ToArray());

            //No reason to continue if the colToJoinOn does not exist in both DataTables.
            if (!dtblLeft.Columns.Contains(colToJoinOn) || (!dtblRight.Columns.Contains(colToJoinOn) && !dtblRight.Columns.Contains(strTempColName)))
            {
                if (!dtblResult.Columns.Contains(colToJoinOn))
                    dtblResult.Columns.Add(colToJoinOn);
                return dtblResult;
            }


            var rowDataLeftOuter = from rowLeft in dtblLeft.Rows.Cast<DataRow>()
                                   join rowRight in dtblRight.Rows.Cast<DataRow>() on rowLeft[colToJoinOn] equals rowRight[strTempColName] into gj
                                   from subRight in gj.DefaultIfEmpty()
                                   select rowLeft.ItemArray.Concat((subRight == null) ? (dtblRight.NewRow().ItemArray) : subRight.ItemArray).ToArray();


            //Add row data to dtblResult
            foreach (object[] values in rowDataLeftOuter)
                dtblResult.Rows.Add(values);


            //Change column name back to original
            dtblRight.Columns[strTempColName].ColumnName = colToJoinOn;

            //Remove extra column from result
            dtblResult.Columns.Remove(strTempColName);

            return dtblResult;

        }
    }
}