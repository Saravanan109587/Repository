using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseRepoCore
{
    public interface IDataContext:IDisposable
    {
        #region Sync Methods
        void Insert<T>(T item, IDbTransaction transaction = null) where T : class;
        int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class;
       
        T Find<T>(Expression<Func<T, bool>> expression) where T : class;
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression) where T : class;
        int Execute(string commandText, object parameters = null, IDbTransaction transaction = null);
        IDataReader ExecuteReader(string commandText, object parameters = null, IDbTransaction transaction = null);
        T ExecuteScalar<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : class;
        int ExecuteProcedure(string storedProcedureName, object parameters = null, IDbTransaction transaction = null);
        IDataReader ExecuteReaderProcedure(string storedProcedureName, object parameters = null, IDbTransaction transaction = null);
        T ExecuteScalarProcedure<T>(string storedProcedureName, object parameters = null, IDbTransaction transaction = null) where T : class;
        #endregion
 
        IEnumerable<T> ExecuteProcedureSingleResult<T>(string storedProcedureName, DynamicParameters parameters = null, IDbTransaction transaction = null)  ;
        SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, IDbTransaction transaction = null);
        IDbConnection Connection { get; }
        void OpenConnection();
        IDbTransaction BeginTransaction();
    }
}
