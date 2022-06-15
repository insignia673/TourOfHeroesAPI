using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ORMF23.Data
{
    public static class SqlProcessor
    {
        public static IEnumerable<T> Exec<T>(this IDbConnection connection, string value)
        {
            List<T> output = new();
            var cmd = default(IDbCommand);
            try
            {
                using (cmd = connection.CreateCommand())
                {
                    cmd.CommandText = value;
                    cmd.Connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        DataReaderMapToList(reader, ref output);
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Exception was thrown while executing command");
            }
            finally
            {
                cmd.Connection.Close();
            }

            return output;
        }

        public static void Exec(this IDbConnection connection, string value)
        {
            IDbCommand cmd = default(IDbCommand);
            try
            {
                using (cmd = connection.CreateCommand())
                {
                    cmd.CommandText = value;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {

            }
            catch (Exception ex)
            {
                //throw new Exception("Exception was thrown while executing command");
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        private static void DataReaderMapToList<T>(IDataReader dr, ref List<T> list)
        {
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
        }
    }
}
