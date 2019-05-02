using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models
{
    //Context class for mapping ToDoItems to database
    public class ItemsContext : DbContext
    {
        public ItemsContext(DbContextOptions<ItemsContext> options)
            : base(options)
        {
        }
        //Collection of ToDo items
        public DbSet<ToDoItem> ToDoItems { get; set; }

    }
}