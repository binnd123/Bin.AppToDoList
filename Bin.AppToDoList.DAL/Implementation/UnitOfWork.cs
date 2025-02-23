using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bin.AppToDoList.DAL.Contract;
using Bin.AppToDoList.DAL.Data;

namespace Bin.AppToDoList.DAL.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppToDoListDBContext _context;

        public UnitOfWork(AppToDoListDBContext context)
        {
            _context = context;
        }


        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
