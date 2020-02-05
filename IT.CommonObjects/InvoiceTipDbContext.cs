namespace IT.CommonObjects
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class InvoiceTipDbContext : DbContext
    {
        public InvoiceTipDbContext()
            : base("name=InvoiceTipConnectionString")
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .IsUnicode(false);
        }
    }
}
