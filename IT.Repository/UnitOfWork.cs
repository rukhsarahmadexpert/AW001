using IT.CommonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Repository
{
    public class UnitOfWork
    {
        InvoiceTipDbContext invoiceTipDbContext = new InvoiceTipDbContext();
        public Type TheType { get; set; }
        public InvoiceTipRepository<T> GetRepositoryInstance<T>() where T : class
        {
            return new InvoiceTipRepository<T>(invoiceTipDbContext);
        }
        public int SaveChanges()
        {
            return invoiceTipDbContext.SaveChanges();
        }
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (invoiceTipDbContext == null) return;

            invoiceTipDbContext.Dispose();
            invoiceTipDbContext = null;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
