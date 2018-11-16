
using System;

namespace DynamicReportAPI.Models
{
    public class DynamicReportProperties
    {
        public string ElementSequence { get; set; }
        public string ElementNature { get; set; }
        public string MasterTable { get; set; }
        public string MasterCol { get; set; }
        public string MasterValue { get; set; }
        public string MasterValueOperator { get; set; }
        public string MasterRefCol { get; set; }
        public string FactTable { get; set; }
        public string FactColumn { get; set; }
        public string ValueTable { get; set; }
        public string ValueCol { get; set; }
        public string Value { get; set; }
        public string ValueRefCol { get; set; }
        public string ValueCompareOperator { get; set; }
        public string SQLJoin { get; set; }
        public int EntityKey { get; set; }

    }
}