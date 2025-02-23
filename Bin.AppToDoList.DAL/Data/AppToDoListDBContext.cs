using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bin.AppToDoList.DAL.Data
{
    public class AppToDoListDBContext : DbContext
    {
        public AppToDoListDBContext() { }

        public AppToDoListDBContext(DbContextOptions<AppToDoListDBContext> options) : base(options)
        {
        }
        public DbSet<Models.ToDoList> ToDoLists { get; set; } = null!;
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=BINND;database=ToDoListDB;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }
    }
}
