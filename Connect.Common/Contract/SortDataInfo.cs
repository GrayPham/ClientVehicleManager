using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Common.Contract
{
    public class SortDataInfo
    {
        public string ColumName { get; set; }

        public string TypeSorting { get; set; }
        public string Table { get; set; } = "";
    }
}
