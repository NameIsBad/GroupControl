using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Collections;

namespace GroupControl.Common
{
    public sealed class DataUtil
    {
        public const string MIN_DATE_TIME = "1970-01-01 00:00:00.000";

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(IList<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        /// <summary>
        /// 生成指定长度的随机码。
        /// </summary>
        public static string CreateRandomCode(int length)
        {
            string[] codes = new string[36] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            StringBuilder randomCode = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                randomCode.Append(codes[rand.Next(codes.Length)]);
            }
            return randomCode.ToString();
        }


        /// <summary>
        /// 获取枚举的中文描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static  string GetEnumDescription(Enum enumValue)
        {
            try
            {
                var str = enumValue.ToString();

                System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);

                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                if (objs == null || objs.Length == 0) return str;

                System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];

                return da.Description;
            }
            catch (Exception)
            {
                return default(string);
            }
        }
    }
}
