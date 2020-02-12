using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using System.Linq;
using Mathy.Shared.Page;
using System.Data;
using System.Reflection;

namespace Mathy.Repository
{
    public abstract class BaseRepo
    {
        private readonly DbBase _DbBase;
        public BaseRepo(DbBase dbBase)
        {
            _DbBase = dbBase;
        }

        protected virtual List<T> Query<T>(string sql, object param = null, SqlTransaction transaction = null)
        {
            if (transaction != null)
            {
                return transaction.Connection.Query<T>(sql, param, transaction).AsList();
            }
            else
            {
                using var conn = _DbBase.GetDbConnection();
                return conn.Query<T>(sql, param).AsList();
            }
        }

        protected virtual T QueryFirst<T>(string sql, object param = null, SqlTransaction transaction = null)
        {
            return Query<T>(sql, param, transaction).FirstOrDefault();
        }

        protected virtual bool Excute(string sql, object param = null, SqlTransaction transaction = null)
        {
            if (transaction != null)
            {
                return transaction.Connection.Execute(sql, param, transaction) >= 0;
            }
            else
            {
                using var conn = _DbBase.GetDbConnection();
                return conn.Execute(sql, param, transaction) >= 0;
            }
        }

        protected virtual int ExcuteScalar(string sql, object param = null, SqlTransaction transaction = null)
        {
            if (transaction != null)
            {
                return transaction.Connection.ExecuteScalar<int>(sql, param, transaction);
            }
            else
            {
                using var conn = _DbBase.GetDbConnection();
                return conn.ExecuteScalar<int>(sql, param, transaction);
            }
        }

        protected virtual bool BulkToDB<T>(List<T> entitys, string tableName, SqlTransaction tran = null)
        {
            DataTable dt = GetTable(entitys);
            using var conn = _DbBase.GetDbConnection(false);
            SqlBulkCopy bulkCopy;
            if (tran != null)
            {
                bulkCopy = new SqlBulkCopy(tran.Connection, SqlBulkCopyOptions.Default, tran);
            }
            else
            {
                bulkCopy = new SqlBulkCopy(conn);
            }
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.BatchSize = dt.Rows.Count;
            try
            {
                if (dt != null && dt.Rows.Count != 0)
                    bulkCopy.WriteToServer(dt);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual PageList<T> QueryPage<T>(string sql, object param, PageInfo pageInfo = null)
        {
            if (sql.EndsWith(";"))
                sql.Substring(0, sql.Length - 2);
            pageInfo = pageInfo ?? new PageInfo();
            var countstr = @" SELECT  COUNT(*) totalcount
                                FROM    ( {0}
                                        ) CountTable ";
            var sqlCount = string.Format(countstr, sql);
            var sqlPage = sql + " Order By " + pageInfo.OrderField + " " + pageInfo.DescStr;
            sqlPage += " OFFSET " + pageInfo.OffSet + " ROWS FETCH NEXT " + pageInfo.PageSize + " ROWS ONLY";

            using var conn = _DbBase.GetDbConnection();
            var page = new PageList<T>
            {
                PageData = Query<T>(sqlPage, param)
            };
            pageInfo.TotalCount = conn.QueryFirst<int>(sqlCount, param);
            page.PageInfo = pageInfo;
            return page;
        }

        private DataTable GetTable<T>(IEnumerable<T> entitys)
        {
            DataTable dt = new DataTable();
            Type t = typeof(T);
            PropertyInfo[] propertys = t.GetProperties();
            foreach (var n in propertys)
            {
                var column = new DataColumn(n.Name);
                column.AllowDBNull = true;
                dt.Columns.Add(column);
            }
            foreach (var n in entitys)
            {
                object[] entityValues = new object[propertys.Length];
                for (int i = 0; i < propertys.Length; i++)
                {
                    entityValues[i] = propertys[i].GetValue(n, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
