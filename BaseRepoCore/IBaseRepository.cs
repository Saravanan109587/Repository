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
    public interface IRepository<T> where T : class
    {
        void Insert(T item);
        void InsertBulk(IEnumerable<T> items);
     
        T Find(Expression<Func<T, bool>> expression);
        IList<T> FindAll(Expression<Func<T, bool>> expression);
        int Execute(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
        IEnumerable<T> ExecuteProcedureSingleResult<T1>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null) ;
        SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null);
    }
}
