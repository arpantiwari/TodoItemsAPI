using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPI.Models
{
    //Model class to represent User Items in database
    public class UserItem
    {
        //ID Column
        [Key]
        public long UserID { get; set; }
        //UserName column
        public string UserName { get; set; }
        //Password column
        public string Password { get; set; }
    }
}
