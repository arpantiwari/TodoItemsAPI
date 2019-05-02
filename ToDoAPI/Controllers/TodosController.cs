using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoAPI.Models;

namespace TodoApi.Controllers
{
    //Enable token based authorization for this controller
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ItemsContext _context;

        public TodosController(ItemsContext context)
        {
            _context = context;
        }

        //GET methods
        [HttpGet]
        //Get all items (based on filter criteria passed)
        public ActionResult<List<ToDoItem>> GetAllItems(string username = null, string priority = null, string status = null, string date = null, bool sortbydate = false)
        {
            var items = _context.ToDoItems.ToList();
            //Filter by username
            items = items.Where(i => i.UserName.ToUpper() == username.Trim().ToUpper()).ToList();
            //Filter by priority if it is passed
            if (priority != null)
                items = items.Where(i => i.Priority.ToUpper() == priority.Trim().ToUpper()).ToList();
            //Filter by status if it is passed
            if (status != null)
                items = items.Where(i => i.Status.ToUpper() == status.Trim().ToUpper()).ToList();
            //Filter by due date if it is passed
            if (date != null)
            {
                var dueDate = DateTime.MinValue;
                //If input string is in an incorrect format
                if (!DateTime.TryParse(date, out dueDate))
                    return NoContent();                
                //Fetch all tasks matching the passed priority
                items = items.Where(i => i.DueDate.Date == dueDate).ToList();
            }
            //Return 204 if no records exist
            if(items.Count == 0)
                return NoContent();
            //Sort results by date if the sorting flag is true
            if(sortbydate)
                items = items.OrderBy(todoitem => todoitem.DueDate).ToList();
            //return items with 200 status
            return Ok(items);
        }

        //Get item by ID
        [HttpGet("{id}", Name = "GetTodos")]
        public ActionResult<ToDoItem> GetById(long id)
        {
            //Find item by task ID
            var item = _context.ToDoItems.Find(id);
            if (item == null)
            {
                //If not found, return 404
                return NotFound();
            }
            //Return item with status 200
            return Ok(item);
        }

        //POST method for ToDo items
        [HttpPost]
        public IActionResult Create(ToDoItem item)
        {
            //Ignore ID value as it is generated automatically in the database
            item.ID = 0;
            //Add the new todo item to the db
            _context.ToDoItems.Add(item);
            _context.SaveChanges();
            //Returning status 201 upon successful creation along with the created item
            return CreatedAtRoute("GetTodos", new { id = item.ID }, item);
        }

        //PUT method for ToDo items
        [HttpPut("{id}")]
        public IActionResult Update(long id, ToDoItem item)
        {
            //Find the passed ID in db for update
            var todo = _context.ToDoItems.Find(id);
            //If not found, return status 404
            if (todo == null)
            {
                return NotFound();
            }
            //Update the found ToDo item with new values (except ID)
            todo.TaskName = item.TaskName;
            todo.Priority = item.Priority;
            todo.DueDate = item.DueDate;
            todo.Status = item.Status;
            todo.isChecked = item.isChecked;
            //Perform the update in db
            _context.ToDoItems.Update(todo);
            _context.SaveChanges();
            //Return status 204 upon successful update
            return NoContent();
        }

        //DELETE method for ToDo items
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            //Find the passed ID in db for deletion
            var todo = _context.ToDoItems.Find(id);
            //If not found, return status 404
            if (todo == null)
            {
                return NotFound();
            }
            //Delete the item from DB
            _context.ToDoItems.Remove(todo);
            _context.SaveChanges();
            //Return status 204 upon successful deletion
            return NoContent();
        }
    }
}