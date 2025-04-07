using InventoryApp.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Models.Context
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=InventoryApp;Trusted_Connection=True;TrustServerCertificate=True;");
    }

}
