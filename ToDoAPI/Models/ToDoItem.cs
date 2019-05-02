﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPI.Models
{
    //Model class to represent ToDo items in database
    public class ToDoItem
    {
        //ID column (autogenerated)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        //UserName column
        public string UserName { get; set; }
        //TaskName column
        public string TaskName { get; set; }
        //Priority column
        public string Priority { get; set; }
        //Due Date column
        public DateTime DueDate { get; set; }
        //Status column
        public string Status { get; set; }
        //isCheckedColumn
        public byte isChecked { get; set; }
    }
}
