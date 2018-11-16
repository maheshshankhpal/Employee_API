using System;
using System.Collections.Generic;

namespace DynamicReportAPI.Models
{
    public class ItemGrpMappingProperties
    {
        public string ReportId { get; set; }
        public string ItemId { get; set; }
        public string ExcelMappingType { get; set; }
        public string ExcelSheetName { get; set; }
        public string ExcelColumnPosition { get; set; }
        public string ExcelRowPosition { get; set; }
        public string DataFor { get; set; }
        public int EntityKey { get; set; }
      
    }
}