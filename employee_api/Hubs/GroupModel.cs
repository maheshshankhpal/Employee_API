using System;
using System.Collections.Generic;

namespace DynamicReportAPI.Hubs
{
    public static class GroupModel
    {
        public static string UserID { get; set; }
        public static List<GroupsList> grouplist { get; set; }   
    }

    public class GroupsList
    {
        public string GroupName { get; set; }
    }
}