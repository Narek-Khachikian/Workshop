using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WS.Models;

namespace WS.Data
{
    public class WSDataContext : DbContext
    {
        public WSDataContext(DbContextOptions<WSDataContext> options): base(options)
        {

        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ProductMaterial>().HasKey(pm => new { pm.WSMaterialId, pm.WSProductId });
            modelBuilder.Entity<ProductMaterial>().HasKey(pm => pm.Id);

            modelBuilder.Entity<WSSupplier>().Property("Status").HasDefaultValueSql("((1))");

            modelBuilder.Entity<WSMaterial>().Property("Status").HasDefaultValueSql("((1))");

            modelBuilder.Entity<WSProduct>().Property("Status").HasDefaultValueSql("((1))");

            modelBuilder.Entity<ProductMaterial>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<WSMaterial>().HasOne<WSSupplier>(m => m.Supplier).WithMany(s => s.WSMaterials).HasForeignKey(m => m.WSSuplierId);

        }


        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<WSSupplier> Suppliers { get; set; }
        public DbSet<WSProduct> Products { get; set; }
        public DbSet<WSMaterial> Materials { get; set; }
        
    }
}
