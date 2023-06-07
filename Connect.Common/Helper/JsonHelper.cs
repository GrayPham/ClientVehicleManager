using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Connect.Common.Helper
{
    public class JsonHelper
    {
        public static string ErrorCurrent = "";
        public static string ListInfoToJson(object value)
        {
            try
            {
                if (value == null) return "[]";
                return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
            catch (Exception ex)
            {
                ErrorCurrent = ex.Message;
                return "[]";
            }
    
        }
        public static List<T> JsonToListInfo<T>(string value)
        {
            try
            {
                return (Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(value ?? (value = ""))) ?? (new List<T>());
            }
            catch (Exception ex)
            {
                ErrorCurrent = ex.Message;
                return new List<T>();
            }
        }
        public static string InfoToJson(object value)
        {

            if (value == null) return "";
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
        public static T JsonToInfo<T>(string value) where T : new()
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value ?? (value = ""));
            }
            catch (Exception ex)
            {
                ErrorCurrent = ex.Message;
                return new T();
            }

        }
        public static DataTable List2DataTable<T>(List<T> entities) where T : class, new()
        {
            try
            {

                var modelEntityType = typeof(T);
                // Retrieve all properties with ColumnAttribute
                var columnProperties = modelEntityType.GetProperties();

                // Create a DataTable and initialize columns
                var dataTable = new DataTable(modelEntityType.Name);
                foreach (var columnProperty in columnProperties)
                {
                    var type = columnProperty.PropertyType;

                    if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        var underlyingType = Nullable.GetUnderlyingType(columnProperty.PropertyType);
                        var column = new DataColumn(columnProperty.Name, underlyingType);
                        dataTable.Columns.Add(column);
                    }
                    else
                    {
                        dataTable.Columns.Add(new DataColumn(columnProperty.Name, type));
                    }
                }

                // Add data (rows) to the created DataTable
                foreach (var modelEntity in entities)
                {
                    var values = columnProperties.Select(p => p.GetValue(modelEntity, null)).ToArray();
                    dataTable.Rows.Add(values);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                ErrorCurrent = ex.Message;
                return null;
            }
        }
    }
}
