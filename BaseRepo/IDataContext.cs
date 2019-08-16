using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseRepo
{
    public interface IDataContext
    {
        #region Sync Methods
        void Insert<T>(T item, IDbTransaction transaction = null) where T : class;
        int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class;
        int Update<T>(T item, IDbTransaction transaction = null) where T : class;
        int UpdateBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class;
        int Delete<T>(T item, IDbTransaction transaction = null) where T : class;
        int DeleteBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class;
        T Find<T>(int Id) where T : class;
        T Find<T>(Expression<Func<T, bool>> expression) where T : class;
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression) where T : class;
        int Execute(string commandText, object parameters = null, IDbTransaction transaction = null);
        IDataReader ExecuteReader(string commandText, object parameters = null, IDbTransaction transaction = null);
        T ExecuteScalar<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : class;
        int ExecuteProcedure(string storedProcedureName, object parameters = null, IDbTransaction transaction = null);
        IDataReader ExecuteReaderProcedure(string storedProcedureName, object parameters = null, IDbTransaction transaction = null);
        T ExecuteScalarProcedure<T>(string storedProcedureName, object parameters = null, IDbTransaction transaction = null) where T : class;
        #endregion

        #region Async Methods
        Task InsertAsync<T>(T item, IDbTransaction transaction = null) where T : class;
        Task<int> InsertBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class;
        Task<int> UpdateAsync<T>(T item, IDbTransaction transaction = null) where T : class;
        Task<int> UpdateBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class;
        Task<int> DeleteAsync<T>(T item, IDbTransaction transaction = null) where T : class;
        Task<int> DeleteBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class;
        Task<T> FindAsync<T>(int Id) where T : class;
        Task<T> FindAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<int> ExecuteAsync(string commandText, object parameters = null, IDbTransaction transaction = null);
        Task<IDataReader> ExecuteReaderAsync(string commandText, object parameters = null, IDbTransaction transaction = null);
        Task<T> ExecuteScalarAsync<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : class;
        Task<int> ExecuteProcedureAsync(string storedProcedureName, object parameters = null, IDbTransaction transaction = null);
        Task<IDataReader> ExecuteReaderProcedureAsync(string storedProcedureName, object parameters = null, IDbTransaction transaction = null);
        Task<T> ExecuteScalarProcedureAsync<T>(string storedProcedureName, object parameters = null, IDbTransaction transaction = null) where T : class;
        #endregion

        IDbConnection Connection { get; }
        void OpenConnection();
        IDbTransaction BeginTransaction();
    }
}
