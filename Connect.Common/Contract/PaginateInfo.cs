using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.Common.Contract
{
    public class PaginateInfo
    {
        public int TotalRecords { get; set; } //
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string TSQL { get; set; }
        public string TSQLTotal { get; set; }
        public string Owner { get; set; }
        public string UserName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Group { get; set; } = "";
        public string SearchText { get; set; }
        public List<SortDataInfo> Sortings { get; set; }
        public List<FilterDataInfo> Filtering { get; set; }
        public PaginateInfo()
        {
            TotalRecords = 0;
            PageSize = 100;
            PageIndex = 0;
            TSQL = "";
            TSQLTotal = "";
            Owner = "";
            UserName = "";
        }
    }
}
