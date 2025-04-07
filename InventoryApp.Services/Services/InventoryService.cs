using InventoryApp.Models.Context;
using InventoryApp.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Services.Services
{
    public class InventoryService : IInventoryService
    {
        public async Task<List<Item>> GetAllItems()
        {
            using var db = new InventoryDbContext();
            return await db.Items.ToListAsync();
        }

        public async Task AddItem(Item item)
        {
            using var db = new InventoryDbContext();
            db.Items.Add(item);
            await db.SaveChangesAsync();
        }

        public async Task UpdateItem(Item item)
        {
            using var db = new InventoryDbContext();
            item.LastUpdated = DateTime.Now;
            db.Items.Update(item);
            await db.SaveChangesAsync();
        }
    }

}
