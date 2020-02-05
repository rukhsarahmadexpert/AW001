using IT.CommonObjects;
using IT.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Repository
{
    public class InvoiceTipRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private bool isDisposed;

        private DbSet<TEntity> dbSet;
        public DbSet<TEntity> DbSet
        {
            get { return dbSet; }
        }

        InvoiceTipDbContext invoiceTipDbContext;

        public InvoiceTipRepository(InvoiceTipDbContext invoiceTipDbContext)
        {
            this.invoiceTipDbContext = new InvoiceTipDbContext();
            if (this.dbSet == null)
                this.dbSet = invoiceTipDbContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet;
        }
        public IEnumerable<TEntity> GetWhere(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }
        public TEntity GetFirstOrDefault(int recordId)
        {
            return dbSet.Find(recordId);
        }
        public TEntity GetFirstOrDefault(string recordId)
        {
            return dbSet.Find(recordId);
        }
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);

        }
        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
        }
        public void Delete(TEntity entity)
        {
            if (invoiceTipDbContext.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
        }
        public IEnumerable<TEntity> ReadStoredProcedure(string sql, params object[] parameters)
        {
            return invoiceTipDbContext.Database.SqlQuery<TEntity>(sql, parameters).ToList<TEntity>();
        }
        public int WriteStoredProcedure(string sql, params object[] parameters)
        {
            return invoiceTipDbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        public virtual void Dispose(bool isManuallyDisposing)
        {
            if (!isDisposed)
            {
                if (isManuallyDisposing)
                    invoiceTipDbContext.Dispose();
            }

            isDisposed = true;
        }
        public void Dispose()
        {
            invoiceTipDbContext.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~InvoiceTipRepository()
        {
            Dispose(false);
        }
    }
}
