using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BaseRepo
{
    public interface IRepository<T> where T : class
    {
        int Insert(T item, IDbTransaction transaction = null, int? timeout = null);
        int Update(T item, IDbTransaction transaction = null, int? timeout = null);
        int Delete(T item, IDbTransaction transaction = null, int? timeout = null);
        IEnumerable<T> FindALL();
        bool BulkDelete(T item, IDbTransaction transaction = null, int? timeout = null);
        void InsertBulk(IEnumerable<T> items, IDbTransaction transaction = null, int? timeout = null);

        bool BulkInsert(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null);
        T Find(Expression<Func<T, bool>> expression);
        IList<T> FindAll(Expression<Func<T, bool>> expression);
        int Execute(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        IEnumerable<T> ExecuteProcedureSingleResult<T>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);

        List<Dictionary<string, string>> ExecuteProcedureUnKnownModal(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        DataTable ExecuteExportToDataTable(string storedProcedureName, SqlParameter[] parameters = null, int? timeout = null, IDbTransaction transaction = null);
    }
 
}
