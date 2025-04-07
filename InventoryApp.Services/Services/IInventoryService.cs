using InventoryApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Services.Services
{
    public interface IInventoryService
    {
        Task<List<Item>> GetAllItems();
        Task AddItem(Item item);
        Task UpdateItem(Item item);
    }

}
