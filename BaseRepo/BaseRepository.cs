using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace BaseRepo
{
    #region With Model Entity
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

        #region Sync Methods
        public void Insert(T item)
        {
            try
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    _dataContext.Insert(item);
                }
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void InsertBulk(IEnumerable<T> items)
        {
            try
            {
                using (IDataContext _dataContext = new DataContext(_connectionstring))
                {
                    _dataContext.InsertBulk(items);
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

        public int Execute(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.Execute(commandText, parameters, transaction);
            }
        }

        public IEnumerable<T1> ExecuteProcedureSingleResult<T1>(string storedProcedureName, object parameters = null, IDbTransaction transaction = null) where T1 : class
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.ExecuteProcedureSingleResult<T1>(storedProcedureName, parameters, transaction);
            }
        }

        public SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, object parameters = null, IDbTransaction transaction = null)
        {
                 IDataContext _dataContext = new DataContext(_connectionstring);
                 return _dataContext.ExecuteProcedureMultipleResult(storedProcedureName, parameters, transaction);
           
        }
        #endregion
    }
    #endregion

    #region Without Modal Entity

    public class Repository 
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

        public int Execute(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.Execute(commandText, parameters, transaction);
            }
        }

        public IEnumerable<T1> ExecuteProcedureSingleResult<T1>(string storedProcedureName, object parameters = null, IDbTransaction transaction = null) where T1 : class
        {
            using (IDataContext _dataContext = new DataContext(_connectionstring))
            {
                return _dataContext.ExecuteProcedureSingleResult<T1>(storedProcedureName, parameters, transaction);
            }
        }

        public SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, object parameters = null, IDbTransaction transaction = null)
        {
            IDataContext _dataContext = new DataContext(_connectionstring);
            return _dataContext.ExecuteProcedureMultipleResult(storedProcedureName, parameters, transaction);

        }
        #endregion
    }
    #endregion
}
