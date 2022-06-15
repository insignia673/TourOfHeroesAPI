using ORMF23.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMF23.Services
{
    public class SqlManager
    {
        private IDbConnection connection;

        public SqlManager(IDbConnection connection)
        {
            this.connection = connection;
        }
        public IEnumerable<T> Select<T>(string value)
        {
            return connection.Exec<T>(value);
        }
        public T? Single<T>(string value)
        {
            return connection.Exec<T>(value).FirstOrDefault();
        }
        public int Insert<T>(string value, bool selectId)
        {
            var result = connection.Exec<T>(value);
            if (selectId)
            {
                return (int)GetObjectIdentifier(result.FirstOrDefault());
            }
            return result.Count();
        }

        public int Update<T>(string value, bool selectId)
        {
            var result = connection.Exec<T>(value);
            if (selectId)
            {
                return (int)GetObjectIdentifier(result.First());
            }
            return result.Count();
        }

        public int Delete<T>(string value)
        {
            return connection.Exec<T>(value).Count();
        }

        public bool Exist<T>(string value)
        {
            return connection.Exec<T>(value).Count() > 0 ? true : false;
        }

        public void DropOrCreateTable(string value)
        {
            connection.Exec(value);
        }

        private object GetObjectIdentifier(object obj)
        {
            return obj?.GetType().GetProperty("Id").GetValue(obj);
        }
    }
}
