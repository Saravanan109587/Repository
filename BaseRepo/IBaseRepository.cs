using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseRepo
{
    public interface IRepository<T> where T : class
    {
        void Insert(T item);
        void InsertBulk(IEnumerable<T> items);
     
        T Find(Expression<Func<T, bool>> expression);
        IList<T> FindAll(Expression<Func<T, bool>> expression);
        int Execute(string commandText, object parameters = null, IDbTransaction transaction = null);
        IEnumerable<T> ExecuteProcedureSingleResult<T>(string storedProcedureName, object parameters = null, IDbTransaction transaction = null) where T : class;
        SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, object parameters = null, IDbTransaction transaction = null);
    }
}
