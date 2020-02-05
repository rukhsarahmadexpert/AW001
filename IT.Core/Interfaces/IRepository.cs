using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetWhere(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
        TEntity GetFirstOrDefault(int recordId);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> ReadStoredProcedure(string sql, params object[] parameters);
        int WriteStoredProcedure(string sql, params object[] parameters);

    }
}
