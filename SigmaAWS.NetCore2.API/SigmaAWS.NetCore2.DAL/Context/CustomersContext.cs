using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SigmaAWS.NetCore2.Domain.Models;

namespace SigmaAWS.NetCore2.DAL.Context
{
    public partial class CustomersContext : DbContext
    {
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<States> States { get; set; }

        public CustomersContext(DbContextOptions options) : base (options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Customers;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasIndex(e => e.StateId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasIndex(e => e.CustomerId);

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId);
            });
        }
    }
}
