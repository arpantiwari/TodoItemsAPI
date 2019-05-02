using System;
using Xunit;
using ToDoAPI;
using System.Linq;
using TodoApi.Controllers;
using ToDoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoAPI.Tests
{
    //Unit Test class for TodosController
    public class TodosTests
    {
        [Fact]
        public void ShouldReturnAllToDoItems()
        {
            //Arrange
            //Defining in memory db context for mocking
            //Note: This won't ensure db referential integrity so we can actually use different techniques for mocking if database referential integrity is important
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldReturnAllToDoItems").Options;            
            var context = new ItemsContext(options);
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, UserName="Test", TaskName = "Running", DueDate = DateTime.Now.Date, Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 2, UserName="Test", TaskName = "Running", DueDate = DateTime.Now.Date, Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 3, UserName="Test", TaskName = "Running", DueDate = DateTime.Now.Date, Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 4, UserName="Test", TaskName = "Running", DueDate = DateTime.Now.Date, Priority = "High", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            //Initialize controller with mock context created above
            var controller = new TodosController(context);
            //Act
            var result = controller.GetAllItems("Test");
            var status = result.Result as OkObjectResult;
            var value = status.Value as List<ToDoItem>;
            //Assert
            //All 4 items should be returned
            Assert.Equal(4, value.Count);
            //Status code should be 200
            Assert.Equal(200, status.StatusCode);
        }

        [Fact]
        public void ShouldReturnEmptyList()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "GetZeroItems").Options;
            //Defining db context for mocking with no items
            var context = new ItemsContext(options);
            //Initialize controller with mock context created above
            var controller = new TodosController(context);
            //Act
            var result = controller.GetAllItems(string.Empty);
            var status = result.Result as NoContentResult;
            var value = result.Value;
            //Assert
            //Value should be null
            Assert.Null(value);
            //Status code should be 204
            Assert.Equal(204, status.StatusCode);
        }

        [Fact]
        public void ShouldSortAllItemsByDate()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldSortAllItemsByDate").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Running", DueDate = new DateTime(2018,11,10), Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 2, TaskName = "Running", DueDate = new DateTime(2018,11,01), Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Running", DueDate = new DateTime(2018,11,20), Priority = "High", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            //Initiliaze controller with mock context created above
            var controller = new TodosController(context);
            //Act
            var result = controller.GetAllItems(sortbydate: true);
            var status = result.Result as OkObjectResult;
            var values = status.Value as List<ToDoItem>;
            //Assert
            //The items should be sorted by ascending order of dates
            //First item
            Assert.Equal("2018-11-01", values[0].DueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            //Last item
            Assert.Equal("2018-11-20", values[2].DueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void ShouldReturnNullIfIDNotFound()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldReturnNullIfIDNotFound").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Running", DueDate = new DateTime(2018,11,10), Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 2, TaskName = "Running", DueDate = new DateTime(2018,11,01), Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Running", DueDate = new DateTime(2018,11,20), Priority = "High", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            //Initiliaze controller with mock context created above
            var controller = new TodosController(context);
            //Act
            var result = controller.GetById(5);
            var statusCode = result.Result as StatusCodeResult;
            //Assert
            //Result should be null
            Assert.Null(result.Value);
            //Status code should be 404
            Assert.Equal(404, statusCode.StatusCode);
        }

        [Fact]
        public void ShouldReturnItemByID()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldReturnItemByID").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Running", DueDate = new DateTime(2018,11,10), Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 2, TaskName = "Running", DueDate = new DateTime(2018,11,01), Priority = "High", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Running", DueDate = new DateTime(2018,11,20), Priority = "High", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            //Initiliaze controller with mock context created above
            var controller = new TodosController(context);
            //Act
            //Passing in valid ID
            var result = controller.GetById(2);
            var status = result.Result as OkObjectResult;
            var value = status.Value as ToDoItem;
            //Assert
            //Result should have ID 2
            Assert.Equal(2, value.ID);
            //Status code will be 200
            Assert.Equal(200, status.StatusCode);
        }

        [Fact]
        public void ShouldReturnItemsByPriority()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldReturnItemsByPriority").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Running", DueDate = new DateTime(2018,11,10), Priority = "Medium", Status = "Completed"},
                new ToDoItem { ID = 2, TaskName = "Running", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Running", DueDate = new DateTime(2018,11,20), Priority = "Medium", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            //Initialize controller with mock context created above
            var controller = new TodosController(context);
            //a. Passing priority not present in list
            //Act
            var result = controller.GetAllItems(priority: "High");
            var status = result.Result as NoContentResult;
            var value = result.Value as List<ToDoItem>;
            //Assert
            //Result should have no records
            Assert.Null(value);
            //Status code will be 204
            Assert.Equal(204, status.StatusCode);
            //b. Passing priority present in list
            //Act
            result = controller.GetAllItems(priority: "medium");
            var stat = result.Result as OkObjectResult;
            var val = stat.Value as List<ToDoItem>;
            //Assert
            //Result should have 3 records
            Assert.Equal(3, val.Count);
            //Status code will be 200
            Assert.Equal(200, stat.StatusCode);
        }

        [Fact]
        public void ShouldReturnItemsByStatus()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldReturnItemsByStatus").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Running", DueDate = new DateTime(2018,11,10), Priority = "Medium", Status = "New"},
                new ToDoItem { ID = 2, TaskName = "Running", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Running", DueDate = new DateTime(2018,11,20), Priority = "Medium", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            //Initialize controller with mock context created above
            var controller = new TodosController(context);
            //a. Passing status not present in list
            //Act
            var result = controller.GetAllItems(status: "In Progress");
            var status = result.Result as NoContentResult;
            var value = result.Value as List<ToDoItem>;
            //Assert
            //Result should have no records
            Assert.Null(value);
            //Status code will be 204
            Assert.Equal(204, status.StatusCode);
            //b. Passing status present in list
            //Act
            result = controller.GetAllItems(status: "completed");
            var stat = result.Result as OkObjectResult;
            var val = stat.Value as List<ToDoItem>;
            //Assert
            //Result should have 2 records
            Assert.Equal(2, val.Count);
            //Status code will be 200
            Assert.Equal(200, stat.StatusCode);
        }

        [Fact]
        public void ShouldReturnItemsByDate()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldReturnItemsByDate").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Running", DueDate = new DateTime(2018,11,10), Priority = "Medium", Status = "New"},
                new ToDoItem { ID = 2, TaskName = "Running", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Running", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            //Initiliaze controller with mock context created above
            var controller = new TodosController(context);
            //a. Passing invalid date
            //Act
            var result = controller.GetAllItems(date: "20181201");
            var status = result.Result as NoContentResult;
            var value = result.Value as List<ToDoItem>;
            //Assert
            //Result should have no records
            Assert.Null(value);
            //Status code will be 204
            Assert.Equal(204, status.StatusCode);
            //b. Passing date not present in list
            //Act
            result = controller.GetAllItems(date: "2018-12-01");
            status = result.Result as NoContentResult;
            value = result.Value as List<ToDoItem>;
            //Assert
            //Result should have no records
            Assert.Null(value);
            //Status code will be 204
            Assert.Equal(204, status.StatusCode);
            //c. Passing date present in list
            //Act
            result = controller.GetAllItems(date: "2018-11-01");
            var stat = result.Result as OkObjectResult;
            var val = stat.Value as List<ToDoItem>;
            //Assert
            //Result should have 2 records
            Assert.Equal(2, val.Count);
            //Status code will be 200
            Assert.Equal(200, stat.StatusCode);            
        }

        [Fact]
        public void ShouldCreateToDoItem()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldCreateToDoItem").Options;
            var context = new ItemsContext(options);            
            //Adding dummy todo items
            var toDoItem = new ToDoItem { ID = 1, TaskName = "Buy groceries", DueDate = new DateTime(2018, 11, 10), Priority = "Medium", Status = "New" };            
            //Initiliaze controller with mock context created above
            var controller = new TodosController(context);
            //Act
            var result = controller.Create(toDoItem);
            var status = result as CreatedAtRouteResult;
            var value = status.Value as ToDoItem;
            //Assert
            //DB should have 1 record
            Assert.Single(context.ToDoItems.ToList());
            //Should return the created record
            Assert.NotNull(value);
            //Status code will be 201
            Assert.Equal(201, status.StatusCode);            
        }

        [Fact]
        public void ShouldUpdateToDoItem()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldUpdateToDoItem").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Buy groceries", DueDate = new DateTime(2018,11,10), Priority = "Medium", Status = "New"},
                new ToDoItem { ID = 2, TaskName = "Deliver goods", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Book tickets", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();
            var toDoItem = new ToDoItem { TaskName = "Go shopping", DueDate = new DateTime(2018, 11, 10), Priority = "Low", Status = "New" };
            //Initialize controller with mock context created above
            var controller = new TodosController(context);
            //Act
            //a. Update item not existing in list (5)
            var result = controller.Update(5, toDoItem);
            var status = result as StatusCodeResult;            
            //Assert
            //Status code will be 404
            Assert.Equal(404, status.StatusCode);
            //b. Update item no. 3
            result = controller.Update(3,toDoItem);
            var stat = result as StatusCodeResult;
            //Assert
            //Check if record 3 was updated successfully in DB
            var item = context.ToDoItems.Where(val => val.ID == 3).Single();
            Assert.Equal("Go shopping", item.TaskName);
            //Status code will be 204
            Assert.Equal(204, stat.StatusCode);
        }

        [Fact]
        public void ShouldDeleteToDoItem()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<ItemsContext>().UseInMemoryDatabase(databaseName: "ShouldDeleteToDoItem").Options;
            var context = new ItemsContext(options);
            //Adding dummy todo items
            //Adding dummy todo items
            var ToDoItems = new[]
            {
                new ToDoItem { ID = 1, TaskName = "Buy groceries", DueDate = new DateTime(2018,11,10), Priority = "Medium", Status = "New"},
                new ToDoItem { ID = 2, TaskName = "Deliver goods", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
                new ToDoItem { ID = 3, TaskName = "Book tickets", DueDate = new DateTime(2018,11,01), Priority = "Medium", Status = "Completed"},
            };
            context.ToDoItems.AddRange(ToDoItems);
            context.SaveChanges();            
            //Initialize controller with mock context created above
            var controller = new TodosController(context);
            //Act
            //a. Delete item not existing in list (5)
            var result = controller.Delete(5);
            var statusCode = result as StatusCodeResult;
            //Assert
            //Status code will be 404
            Assert.Equal(404, statusCode.StatusCode);
            //The count should remain the same in the db (3)
            Assert.Equal(3, context.ToDoItems.ToList().Count);
            //b. Delete item no. 3
            result = controller.Delete(3);
            statusCode = result as StatusCodeResult;
            //Assert
            //Check if record 3 was deleted successfully
            var item = context.ToDoItems.Where(val => val.ID == 3).FirstOrDefault();
            //Item shouldn't be found
            Assert.Null(item);
            //The count should now be 2
            Assert.Equal(2, context.ToDoItems.ToList().Count);
            //Status code will be 204
            Assert.Equal(204, statusCode.StatusCode);
        }

    }
}
