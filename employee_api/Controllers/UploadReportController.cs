using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DynamicReportAPI.Models.Repository.ReportRepository;
using DynamicReportAPI.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace DynamicReportAPI.Controllers
{
    [Route("api/[controller]")]
    public class UploadReportController : Controller
    {
        // private const string UploadFolder = "uploads";
        private IHostingEnvironment _hostingEnvironment;
        public IConfiguration _iconfiguration;

        public UploadReportController(IHostingEnvironment env, IConfiguration iconfiguration)
        {
            _hostingEnvironment = env;
            _iconfiguration = iconfiguration;

        }

        #region FileUpload Function

        [Route("upload/{statename}/{stateid}/{userid}/{ReportAmountIn}/{FrequencyAlt_Key}/{NumberUnit}")]
        [HttpPost, DisableRequestSizeLimit]
        public JsonResult UploadFile(string statename, string stateid, string userid, int ReportAmountIn, string FrequencyAlt_Key,int NumberUnit)
        {
            try
            {
                var file = Request.Form.Files[0];
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, "Upload", statename);

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string Guidfilename = string.Empty;
                string extantion = string.Empty;
                string fileName = string.Empty;
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    extantion = Path.GetExtension(fullPath);
                    Guidfilename = Guid.NewGuid().ToString() + extantion;
                    using (var stream = new FileStream(Path.Combine(newPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                string HostName = _iconfiguration.GetValue<string>("Host:livehost");
                var ExcelFilePath = HostName + "upload/" + statename + "/" + fileName;

                string fileNameWithpath = Path.Combine(newPath, fileName);

                var Sheetsname = GetSheetList(fileNameWithpath);

                if(Sheetsname == "Error: Blank Sheet Uploaded"){
                    return Json(new FileData(filename: "Blank Sheet Uploaded", ext: null, fileid: -1));
                }
                FileDataModelDB FileData = new FileDataModelDB();
                FileData.ExcelFilePath = ExcelFilePath;
                FileData.ExcelId = null;
                FileData.ExcelName = fileName;
                FileData.StateAlt_Key = stateid;
                FileData.excelSheetName = Sheetsname;
                FileData.FrequencyAlt_Key = FrequencyAlt_Key;
                FileData.ReportAmountIn = ReportAmountIn;
                FileData.NumberUnit = NumberUnit;

                var jsonFile = JsonConvert.SerializeObject(FileData);

                ReportRepository _groupRepository = new ReportRepository(_iconfiguration);
                var fileid = _groupRepository.ExcelFileInUp(jsonFile, userid, 1);

                if (fileid != -1)
                {
                    return Json(new FileData(filename: fileName, ext: extantion, fileid: fileid));
                }
                return Json(new FileData(filename: "Upload failed", ext: null, fileid: -1));
            }
            catch (Exception ex)
            {
                return Json(new FileData(filename: "Upload failed", ext: null, fileid: -1));
            }
        }
        #endregion
        private string GetSheetList(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            var rawText = string.Empty;
            var check = string.Empty;
            int len;
            StringBuilder checkForDelete;
            int check1;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                Dictionary<string, int> toDeleteIndexWorkSheet = new Dictionary<string, int>();

                for (int i = 1; i <= package.Workbook.Worksheets.Count(); i++)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[i];
                    Console.WriteLine(worksheet.Name);
                    if (worksheet.Dimension == null && worksheet.Cells.Address == "A:XFD")
                    {
                        // package.Workbook.Worksheets.Delete(i);
                        toDeleteIndexWorkSheet.Add(worksheet.Name, i);
                    }
                }
				check = String.Join("/", package.Workbook.Worksheets);
                if(toDeleteIndexWorkSheet.Count()>0)
                {
                    //check = String.Join(",", package.Workbook.Worksheets);
                    foreach (var item in toDeleteIndexWorkSheet)
                    {
                        Console.WriteLine(item.Key);
                            
                            check1 = check.IndexOf(item.Key);
                            len = item.Key.Length;
                            if(check1 == 0){
                                check1 = 1;
                                if (check.Length == len)
                                {
                                    len--;
                                }
                                
                            }
                            checkForDelete = new StringBuilder(check); // to access Remove fn of StringBuilder
                            checkForDelete.Remove(check1-1,len+1); //remove ", and SheetName"
                            check = checkForDelete.ToString();
                            Console.WriteLine(check);
                        
                        //package.Workbook.Worksheets.Delete(item.Key);
                    }
                }

                if (check == "" || check == null )
                {
                    rawText = "Error: Blank Sheet Uploaded";
                }
                else
                {
                    rawText = String.Join("/", check);
                }

                //rawText = String.Join(",", check);

                Console.WriteLine(rawText);
            }
            return rawText;
        }

        [Route("getExcelListByState/{stateAltkey}/{ScreenName}")]
        [HttpGet]
        public JsonResult getExcelListByState(int stateAltkey, string ScreenName)
        {
            ReportRepository _report = new ReportRepository(_iconfiguration);
            var results = _report.getExcelList(stateAltkey, ScreenName);

            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Msg = "ok",
                Data = results

            });

        }

    }
    public class FileData
    {
        public FileData(string filename, string ext, int fileid)
        {
            this.fileName = filename;
            this.fileExt = ext;
            this.insertedExcelFileID = fileid;
        }
        public int insertedExcelFileID { get; set; }
        public string fileName { get; set; }
        public string fileExt { get; set; }

    }

    public class FileDataModelDB
    {
        public string ExcelId { get; set; }
        public string ExcelFilePath { get; set; }
        public string StateAlt_Key { get; set; }
        public string ExcelName { get; set; }
        public string excelSheetName { get; set; }
        public int ReportAmountIn { get; set; }
        public string FrequencyAlt_Key { get; set; }
        public int NumberUnit { get; set; }
    }

}