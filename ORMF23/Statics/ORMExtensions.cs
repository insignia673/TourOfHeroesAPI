using Newtonsoft.Json;
using ORMF23.Data;
using ORMF23.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMF23.Statics
{
    public static class ORMExtensions
    {
        //TODO
        //Method returns instance of SqlProcessor
        public static async Task<IEnumerable<T>> SelectAsync<T>(this IDbConnection connection, Expression<Func<T, bool>> predicate = null)
        {
            return await Task.Run(() => connection.GetManager().Select<T>(QueryBuilder.SelectFromPredicate(predicate)));
        }

        public static async Task<T> SingleAsync<T>(this IDbConnection connection, Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => connection.GetManager().Single<T>(QueryBuilder.SingleFromPredicate(predicate)));
        }

        public static async Task<int> InsertAsync<T>(this IDbConnection connection, T entity, bool selectIdentity = false)
        {
            return await Task.Run(() => connection.GetManager().Insert<T>(QueryBuilder.InsertQuery(entity), selectIdentity));
        }

        public static async Task<int> UpdateAsync<T>(this IDbConnection connection, T entity, bool selectIdentity = false)
        {
            return await Task.Run(() => connection.GetManager().Update<T>(QueryBuilder.UpdateQuery(entity), selectIdentity));
        }

        public static async Task<int> DeleteAsync<T>(this IDbConnection connection, Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => connection.GetManager().Delete<T>(QueryBuilder.DeleteFromPredicate(predicate)));
        }

        public static async Task<bool> ExistsAsync<T>(this IDbConnection connection, Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => connection.GetManager().Exist<T>(QueryBuilder.CountFromPredicate(predicate)));
        }

        public static async Task DropAndCreateTableAsync<T>(this IDbConnection connection)
        {
            await Task.Run(() => connection.GetManager().DropOrCreateTable(QueryBuilder.DropAndCreate<T>()));
        }

        public static async Task DropTableAsync<T>(this IDbConnection connection)
        {
            await Task.Run(() => connection.GetManager().DropOrCreateTable(QueryBuilder.DeleteTable<T>()));
        }

        public static SqlManager GetManager(this IDbConnection conn)
        {
            return new SqlManager(conn);
        }
    }
}
