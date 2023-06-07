using Connect.Common;
using Connect.Common.Common;

namespace Connect.Common.Contract
{
    public class FilterDataInfo
    {
        public FilterDataInfo(string columName, string valueDefault, string valueSecond, EDataType dataValue, ETypeFilter typeFilter)
        {
            ColumName = columName;
            ValueDefault = valueDefault;
            ValueSecond = valueSecond;
            DataType = dataValue.ToString();
            TypeFilter = typeFilter.ToString();
        }

        public FilterDataInfo() { }
        public string ColumName { get; set; }
        public object ValueDefault { get; set; }
        public string ValueSecond { get; set; }
        public string DataType { get; set; }
        public string TypeFilter { get; set; }
        public string Table { get; set; } = "";
    }
}
