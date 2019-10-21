using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace BaseRepo
{ 
    public class Repository<T> : IRepository<T> where T : class
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

        
        public void InsertBulk(IEnumerable<T> items, IDbTransaction transaction = null, int? timeout = null)
        {
            try
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    _dataContext.InsertBulk(items, transaction, timeout);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }


        public T Find(Expression<Func<T, bool>> expression)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.Find(expression);
            }
        }

        public IList<T> FindAll(Expression<Func<T, bool>> expression)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.FindAll(expression).ToList();
            }
        }

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

        public virtual bool BulkInsert(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null)
        {
            bool res = false;
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                res = _dataContext.BulkInsert(table, tableName, timeout);
            }
            return res;
        }


        public int Update(T item, IDbTransaction transaction = null, int? timeout = null)
        {

            try
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    _dataContext.Update<T>(item);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;

        }

        public int Delete(T item, IDbTransaction transaction = null, int? timeout = null)
        {
            try
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    _dataContext.Delete<T>(item);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        public IEnumerable<T> FindALL()
        {
            try
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    return _dataContext.FindAll<T>();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Insert(T item, IDbTransaction transaction = null, int? timeout = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                _dataContext.Insert<T>(item, transaction, timeout);
            }
            return 1;
        }

        public bool BulkDelete(T item, IDbTransaction transaction = null, int? timeout = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
               return  _dataContext.Delete<T>(item, transaction, timeout);
            }
        }

        public List<Dictionary<string, string>> ExecuteProcedureUnKnownModal(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.ExecuteProcWithUnknownModal(storedProcedureName, parameters, timeout, transaction);
            }
        }

        public DataTable ExecuteExportToDataTable(string storedProcedureName, SqlParameter[] parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.ExecuteExportToDataTable(storedProcedureName, parameters, timeout, transaction);
            }
        }
    }
    
}
 