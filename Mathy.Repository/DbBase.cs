using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Mathy.Repository
{
    public class DbBase
    {
        private readonly string _ReadConnectionString;
        private readonly string _WriteConnectionString;

        public DbBase(IConfiguration configuration)
        {            
            var conns = configuration.GetSection("MSSQL_ConnectionStrings");
            _ReadConnectionString = conns.GetSection("ReadConnection").Value;
            _WriteConnectionString = conns.GetSection("WriteConnection").Value;
        }
        public SqlConnection GetDbConnection(bool isReadOnly = true)
        {
            string connectionStr = isReadOnly ? _ReadConnectionString : _WriteConnectionString;
            SqlConnection conn = new SqlConnection(connectionStr);
            conn.Open();
            return conn;
        }

        public SqlTransaction GetSqlTransaction()
        {
            return GetDbConnection(false).BeginTransaction();
        }
    }
}
