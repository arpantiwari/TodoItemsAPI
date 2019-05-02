using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models
{
    //Context class for mapping User items to database
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }
        //Collection of User items
        public DbSet<UserItem> Users { get; set; }

    }
}