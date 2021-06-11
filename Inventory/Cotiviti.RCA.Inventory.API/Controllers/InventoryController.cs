using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Inventory.API.Models;

namespace Test.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        private static List<InventoryItem> inventoryList = new List<InventoryItem>()
        { new InventoryItem{ Name= "Apples", Quantity = 3, CreatedOn = DateTime.Parse("01-01-2020") },
          new InventoryItem{ Name= "Oranges", Quantity = 7, CreatedOn = DateTime.Parse("01-02-2020") },
          new InventoryItem{ Name= "Pomegranates", Quantity = 55, CreatedOn = DateTime.Parse("01-10-2020") }
        };

      
        // GET: api/Inventory 
        [HttpGet]
        public IEnumerable<InventoryItem> Get()
        {
            
            return inventoryList.ToArray();
        }

        // GET: api/Inventory/Apple
        [HttpGet("{Item}", Name = "Get")]
        public InventoryItem Get(string Item)
        {
            return inventoryList.Find(x => x.Name.ToLower() == Item.ToLower());
        }

        // POST: api/Inventory
     
        [HttpPost("PostFirst")]
        public string PostFirst([FromBody] List<InventoryItem> inventoryItems)
        {
            string Item = string.Empty;
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                inventoryList.Insert(inventoryList.Count, inventoryItem);
                Item += inventoryItem.Name + ",";
            }
            return Item + " is inserted in the list";
        }

        [HttpPost("PostSecond")]
        public string PostSecond([FromBody] List<InventoryItem> inventoryItems)
        {
            string Item = string.Empty;
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                inventoryList.Insert(inventoryList.Count, inventoryItem);
                Item += inventoryItem.Name + ",";
            }
            return Item + " is inserted in the list";
        }

        // PUT: api/Inventory/Apple
        [HttpPut]
        public string Put([FromBody] InventoryItem inventoryItem)
        {
            var index = inventoryList.FindIndex(x => x.Name.ToLower() == inventoryItem.Name.ToLower());
            if (index >= 0)
            {
                inventoryList[index] = inventoryItem;
                return inventoryItem.Name + " is updated in the list";
            }
            else
            {
                inventoryList.Insert(inventoryList.Count, inventoryItem);
                return inventoryItem.Name + " is inserted in the list";
            }
        }

        // DELETE: api/ApiWithActions/Apple
        [HttpDelete("{Item}, Delete")]
        public string Delete(string Item)
        {
            var ItemToDelete = inventoryList.Find(x => x.Name.ToLower() == Item.ToLower());
            if (ItemToDelete != null)
            {
                inventoryList.Remove(ItemToDelete);
                return Item + " is Deleted from the list";
            }
            else
            {
                return Item + " is not found in the list";
            }
        }


        // DELETE: api/ApiWithActions/Apple
        [HttpDelete("DeleteAll")]
        public string DeleteAll()
        {
            inventoryList = null;
            return " All items in the list is deleted";
           
        }

        /// <summary>
        /// Business Functionality
        /// </summary>

        // GET: api/Inventor/GetHighestQty 
        [HttpGet("GetHighestQty")]
        public InventoryItem GetHighestQty()
        {
            return inventoryList.OrderByDescending(x => x.Quantity).First();
        }

        // GET: api/Inventor/GetLowestQty 
        [HttpGet("GetLowestQty")]
        public InventoryItem GetLowestQty()
        {
            return inventoryList.OrderByDescending(x => x.Quantity).Last();
        }

        // GET: api/Inventor/GetOldestItem 
        [HttpGet("GetOldestItem")]
        public InventoryItem GetOldestItem()
        {
            return inventoryList.OrderByDescending(x => x.CreatedOn).First();
        }

        // GET: api/Inventor/GetNewestItem 
        [HttpGet("GetNewestItem")]
        public InventoryItem GetNewestItem()
        {
            return inventoryList.OrderByDescending(x => x.CreatedOn).Last();
        }

    }
}
