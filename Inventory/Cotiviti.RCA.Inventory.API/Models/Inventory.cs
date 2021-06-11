using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Inventory.API.Models
{
    public class InventoryItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }

    }

}

