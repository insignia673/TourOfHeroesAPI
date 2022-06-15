using ORMF23.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMF23.Data
{
    public class OrmF23ConnectionFactory : IDbConnectionFactory
    {
        private string _connectionString;
        public OrmF23ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection OpenDbConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
