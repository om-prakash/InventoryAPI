using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Test.Inventory.API.Controllers;
using Test.Inventory.API.Models;

namespace TestAPI.Template.Test
{
    [TestClass]
    public class Inventory
    {

        readonly InventoryController inventoryController = new InventoryController();


        [TestMethod]
        public void TestGetApple()
        {
            var result = inventoryController.Get("Oranges");
            Assert.AreEqual("Oranges", result.Name);
        }


        [TestMethod]
        public void TestDelete()
        {
            var result = inventoryController.Delete("Apples");
            Assert.AreEqual("Apples is Deleted from the list", result);
        }

        [TestMethod]
        public void TestPutInsert()
        {
            InventoryItem inventoryItem = new InventoryItem(){ Name = "Kiwies", Quantity = 7, CreatedOn = DateTime.Parse("01-15-2020") };

             var result = inventoryController.Put(inventoryItem);
            Assert.AreEqual("Kiwies is inserted in the list", result);
        }
        [TestMethod]
        public void TestPutUpdate()
        {
            InventoryItem inventoryItem = new InventoryItem() { Name = "Kiwies", Quantity = 10, CreatedOn = DateTime.Parse("01-15-2020") };

            var result = inventoryController.Put(inventoryItem);
            Assert.AreEqual("Kiwies is updated in the list", result);
        }

        [TestMethod]
        public void TestPost()
        {
            List<InventoryItem> inventoryItem = new List<InventoryItem>() { new InventoryItem { Name = "Apples", Quantity = 2, CreatedOn = DateTime.Parse("01-15-2020") } };

            var result = inventoryController.PostFirst(inventoryItem);
            Assert.AreEqual("Apples, is inserted in the list", result);
        }
    }
}