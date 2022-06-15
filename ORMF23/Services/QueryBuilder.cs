using System;
using System.Linq;
using System.Linq.Expressions;
using static ORMF23.Statics.QueryTools;
using static ORMF23.Statics.SQLExpression;

namespace ORMF23.Services
{
    public class QueryBuilder
    {
        public static string SelectFromPredicate<T>(Expression<Func<T, bool>> predicate = null)
        {
            var tableName = GetTableName<T>();
            var conditions = CreateFromPredicate(predicate);

            return SELECT(tableName) + (string.IsNullOrWhiteSpace(conditions) ? "" : WHERE(conditions));
        }
        public static string SingleFromPredicate<T>(Expression<Func<T, bool>> predicate)
        {
            var tableName = GetTableName<T>();
            var conditions = CreateFromPredicate(predicate);

            return SELECT(tableName, top: 1) + (string.IsNullOrWhiteSpace(conditions) ? "" : WHERE(conditions));
        }
        public static string InsertQuery<T>(T entity)
        {
            var tableName = GetTableName<T>();
            var entities = DisectEntity(entity);
            string[] columns = new string[entities.GetLength(0)];
            string[] values = new string[columns.Length];

            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = entities[i, 0];
                values[i] = entities[i, 1];
            }

            return INSERT(tableName, columns, values);
        }
        public static string UpdateQuery<T>(T entity)
        {
            var tableName = GetTableName<T>();
            var entities = DisectEntity(entity);
            string[] columns = new string[entities.GetLength(0)];
            string[] values = new string[columns.Length];

            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = entities[i, 0];
                values[i] = entities[i, 1];
            }
            var id = typeof(T).GetProperties().First(x => x.Name == "Id").GetValue(entity);

            return UPDATE(tableName, columns, values) + WHERE($"Id = {id}");
        }
        public static string DeleteFromPredicate<T>(Expression<Func<T, bool>> predicate)
        {
            var tableName = GetTableName<T>();
            var conditions = CreateFromPredicate(predicate);

            if (string.IsNullOrWhiteSpace(conditions))
            {
                throw new ArgumentNullException("Where Clause", 
                    $"Where clause found. If you would like to delete the table {tableName} please call DeleteTable");
            }

            return DELETE(tableName) + WHERE(conditions);
        }
        public static string CountFromPredicate<T>(Expression<Func<T, bool>> predicate)
        {
            var tableName = GetTableName<T>();
            var conditions = CreateFromPredicate(predicate);

            return SELECT(tableName, top: 1) + (string.IsNullOrWhiteSpace(conditions) ? "" : WHERE(conditions));
        }
        public static string DeleteTable<T>()
        {
            var tableName = GetTableName<T>();
            return DROPTABLE(tableName);
        }
        public static string CreateTable<T>()
        {
            var tableName = GetTableName<T>();
            var columns = EntityToSql<T>();

            var newColumns = new string[columns.GetLength(0)];
            for (int i = 0; i < newColumns.Length; i++)
            {
                newColumns[i] = columns[i, 0] + " " + columns[i, 1];
            }

            return CREATETABLE(tableName, newColumns);
        }
        public static string DropAndCreate<T>()
        {
            string output = "";
            output += DeleteTable<T>();
            output += CreateTable<T>();

            return output;
        }
    }
}
