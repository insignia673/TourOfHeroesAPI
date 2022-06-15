using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMF23.Statics
{
    internal static class SQLExpression
    {
        #region KEYWORDS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columNames">leave null to select all (SQL will read as 'SELECT *')</param>
        /// <returns>Select Sql expression</returns>
        public static string SELECT(string fromTable, string[] columNames = null, bool distinct = false, int? top = null)
        {
            string columns = string.Empty;
            if (distinct)
            {
                columns = DISTINCT(columNames);
            }
            else
            {
                columns = columNames == null ? "*" : string.Join(',', columNames);
            }

            return $"SELECT {(top > 0? $"TOP {top} " : "")}{columns} FROM {fromTable}\n";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns>Where Sql expression</returns>
        public static string WHERE(string conditions)
        {
            return $"WHERE {conditions}\n";
        }

        //string values must be surrounded with '
        public static string INSERT(string tableName, string[] columns, string[] values)
        {
            return $"INSERT INTO {tableName} ({string.Join(',', columns)})\nOUTPUT Inserted.*\nVALUES({string.Join(',', values)})\n";
        }

        public static string UPDATE(string tableName, string[] columns, string[] values)
        {
            string pairs = "";

            for (int i = 0; i < columns.Length; i++)
            {
                pairs += columns[i] + " = " + values[i] + (columns.Length > i + 1 ? ", " : "");
            }

            return UPDATE(tableName, pairs);
        }

        public static string UPDATE(string tableName, string pairs)
        {
            return $"UPDATE {tableName}\nSET {pairs} OUTPUT Inserted.*\n";
        }

        public static string DELETE(string tableName)
        {
            return $"DELETE FROM {tableName} OUTPUT Deleted.*\n";
        }

        public static string COUNT(string fromTable, int amount = 0)
        {
            return $"SELECT COUNT({(amount > 0 ? $"{amount}" : "*")}) FROM {fromTable}\n";
        }

        public static string CREATETABLE(string tableName, string[] columns)
        {
            return $"CREATE TABLE {tableName} ({string.Join(',', columns)});\n";
        }

        public static string DROPTABLE(string tableName)
        {
            return $"DROP TABLE {tableName};\n";
        }

        #endregion
        public static string SqlType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return "BIT";
                case TypeCode.Char:
                    return "CHAR(MAX)";
                case TypeCode.Byte:
                    return "TINYINT";
                case TypeCode.Int16:
                    return "SMALLINT";
                case TypeCode.Int32:
                    return "INT";
                case TypeCode.Int64:
                    return "BIGINT";
                case TypeCode.Double:
                    return "FLOAT";
                case TypeCode.Decimal:
                    return "DECIMAL";
                case TypeCode.DateTime:
                    return "DATETIME";
                case TypeCode.String:
                    return "NVARCHAR(MAX)";
                default:
                    return "VARCHAR(MAX)";
            }
        }
        /// <summary>
        /// used to return an expression or conditions in parenthesis to group them
        /// </summary>
        /// <param name="singleExpression"></param>
        /// <returns></returns>
        public static string Group(string singleExpression)
        {
            return $"({singleExpression})";
        }
        public static string DISTINCT(string[] collumNames = null)
        {
            var collums = collumNames == null ? "*" : string.Join(',', collumNames);
            return $"DISTINCT {collums} ";
        }

        public static string LIKE(string column, string subString)
        {
            return $"{column} LIKE {subString}";
        }

        public static string AND()
        {
            return "AND ";
        }
        public static string OR()
        {
            return "OR ";
        }
        public static string NOT()
        {
            return "NOT ";
        }
    }
}
