using System;
using System.Collections.Generic;
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
        void Update(T item);
        void UpdateBulk(IEnumerable<T> items);
        void Delete(T item);
        void DeleteBulk(IEnumerable<T> items);
        T Find(int Id);
        T Find(Expression<Func<T, bool>> expression);
        IList<T> FindAll(Expression<Func<T, bool>> expression);
    }
}
