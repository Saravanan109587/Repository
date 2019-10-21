using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseRepo
{
    public interface IDataContext:IDisposable
    {
        #region Sync Methods

        void Insert<T>(T item, IDbTransaction transaction = null, int? timeout = null) where T : class;
        bool Delete<T>(T item, IDbTransaction transaction = null, int? timeout = null) where T : class;
        bool DeleteBulk<T>(IEnumerable<T> item, IDbTransaction transaction = null, int? timeout = null) where T : class;
        bool Update<T>(T item, IDbTransaction transaction = null, int? timeout = null) where T : class;
        int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null, int? timeout = null) where T : class;
        IEnumerable<T> FindAll<T>() where T : class;
        T Find<T>(Expression<Func<T, bool>> expression) where T : class;
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression) where T : class;
        int Execute(string commandText, DynamicParameters parameters = null,int? timeout=null, IDbTransaction transaction = null);
        IDataReader ExecuteReader(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        T ExecuteScalar<T>(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null) where T : class;
        int ExecuteProcedure(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        IDataReader ExecuteReaderProcedure(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        T ExecuteScalarProcedure<T>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null) where T : class;
        #endregion
 
        IEnumerable<T> ExecuteProcedureSingleResult<T>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)  ;
        SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        IDbConnection Connection { get; }
        void OpenConnection();
        IDbTransaction BeginTransaction();
          bool BulkInsert(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null);
        List<Dictionary<string, string>> ExecuteProcWithUnknownModal(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        DataTable ExecuteExportToDataTable(string storedProcedureName, SqlParameter[] parameters = null, int? timeout = null, IDbTransaction transaction = null);
    }
}
