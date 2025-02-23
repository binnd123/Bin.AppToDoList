using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bin.AppToDoList.DAL.Models
{
    public class ToDoList 
    {
        [Key]
        public Guid ToDoListId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime CompleteAt { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; } = null!;
    }
}
