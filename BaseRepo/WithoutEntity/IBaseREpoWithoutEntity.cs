using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseRepo 
{
    public interface IRepository
    {
        bool BulkInsert(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null);
        int Execute(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        IEnumerable<T> ExecuteProcedureSingleResult<T>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        Task<bool> BulkInsertAsync(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null);
        bool BulkInsert(Dictionary<string, DataTable> Datatables, int? timeout = null, IDbTransaction transaction = null);
        Task<bool> BulkInsertAsync(Dictionary<string, DataTable> Datatables, int? timeout = null, IDbTransaction transaction = null);
        List<Dictionary<string, string>> ExecuteProcedureUnKnownModal(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        DataTable ExecuteExportToDataTable(string storedProcedureName, SqlParameter[] parameters = null, int? timeout = null, IDbTransaction transaction = null);
    }
}
