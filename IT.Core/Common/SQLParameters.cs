using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.Common
{
    public class SQLParameters
    {
        public static string GetParameters<TClass>(TClass modelClass)
        {
            // var smsystemParam = new SmSmsystemParam();
            PropertyInfo[] properties = modelClass.GetType().GetProperties();
            int count = 0;
            string parameter = "@Flag";
            var spParams = new List<SqlParameter>();
            foreach (PropertyInfo pi in properties)
            {
                count += 1;
                string propertyName = pi.Name;
                parameter = parameter + ",@" + propertyName + "\n";

                var type = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

            }
            return parameter;
        }
        public static IEnumerable<SqlParameter> GetSQLParameters<TClass>(TClass modelClass, string condition)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (!string.IsNullOrWhiteSpace(condition))
            {
                parameters.Add(
                                    new SqlParameter("@Flag", System.Data.SqlDbType.VarChar)
                                    { Value = condition });
            }
            foreach (var item in modelClass.GetType().GetProperties())
            {
                if (item.PropertyType == typeof(int) || item.PropertyType == typeof(int) || item.PropertyType == typeof(Nullable<int>) || item.PropertyType == typeof(Nullable<int>))
                {
                    parameters.Add(new SqlParameter("@" + item.Name + "", System.Data.SqlDbType.Int) { Value = (object)item.GetValue(modelClass) ?? DBNull.Value });
                }

                else if (item.PropertyType == typeof(bool) || item.PropertyType == typeof(Boolean) || item.PropertyType == typeof(Nullable<bool>) || item.PropertyType == typeof(Nullable<Boolean>))
                {
                    parameters.Add(new SqlParameter("@" + item.Name + "", System.Data.SqlDbType.Bit) { Value = (object)item.GetValue(modelClass) ?? DBNull.Value });
                }
                else if (item.PropertyType == typeof(DateTimeOffset) || item.PropertyType == typeof(Nullable<DateTimeOffset>))
                {
                    parameters.Add(new SqlParameter("@" + item.Name + "", System.Data.SqlDbType.DateTimeOffset) { Value = (object)item.GetValue(modelClass) ?? DBNull.Value });
                }
                else if (item.PropertyType == typeof(string))
                {
                    parameters.Add(new SqlParameter("@" + item.Name + "", System.Data.SqlDbType.NVarChar) { Value = (object)item.GetValue(modelClass) ?? DBNull.Value });
                }

            }
            return parameters;
        }
    }
}
