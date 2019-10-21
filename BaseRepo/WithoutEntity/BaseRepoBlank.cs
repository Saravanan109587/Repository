using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;

namespace BaseRepo
{
    #region Without Modal Entity

    public class Repository : IRepository
    {
        #region Fields
        private readonly string _connectionstring;
        #endregion

        #region Ctor
        public Repository(string connectionstring)
        {
            _connectionstring = connectionstring;
        }
        #endregion

        #region Sync Methods 

        public int Execute(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.Execute(commandText, parameters, timeout, transaction);
            }
        }

        public IEnumerable<T1> ExecuteProcedureSingleResult<T1>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)  
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.ExecuteProcedureSingleResult<T1>(storedProcedureName, parameters, timeout, transaction);
            }
        }

        public SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            IDataContext _dataContext = new DataContext(_connectionstring);
            return _dataContext.ExecuteProcedureMultipleResult(storedProcedureName, parameters, timeout, transaction);

        }
        public bool BulkInsert(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null)
        {
            bool res = false;
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                res = _dataContext.BulkInsert(table, tableName, timeout);
            }
            return res;
        }

        public async Task<bool> BulkInsertAsync(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null)
        {
            bool res = false;
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                res = _dataContext.BulkInsert(table, tableName, timeout);
            }
            return res;
        }

        public bool BulkInsert(Dictionary<string, DataTable> Datatables, int? timeout = null, IDbTransaction transaction = null)
        {
            bool res = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                                   new System.TimeSpan(0, 15, 0)))
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    Datatables.ToList().ForEach(q =>
                    {
                        _dataContext.BulkInsert(q.Value, q.Key, timeout);
                    });
                }
                scope.Complete();
            }

            return res;
        }

        public async Task<bool> BulkInsertAsync(Dictionary<string, DataTable> Datatables, int? timeout = null, IDbTransaction transaction = null)
        {
            bool res = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                                   new System.TimeSpan(0, 15, 0)))
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    Datatables.ToList().ForEach(q =>
                    {
                        _dataContext.BulkInsert(q.Value, q.Key, timeout);
                    });
                }
                scope.Complete();
                res = true;
            }

            return res;
        }
        /// <summary>
        /// Execute and return as Key value pair 
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> ExecuteProcedureUnKnownModal(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.ExecuteProcWithUnknownModal(storedProcedureName, parameters, timeout, transaction);
            }
        }

        /// <summary>
        /// Exceute using Standard Sql.Data.client and return as DatTable
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public DataTable ExecuteExportToDataTable(string storedProcedureName, SqlParameter[] parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.ExecuteExportToDataTable(storedProcedureName, parameters, timeout, transaction);
            }
        }
         
        #endregion
    }
    #endregion
}
