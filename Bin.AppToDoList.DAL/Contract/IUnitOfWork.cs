﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bin.AppToDoList.DAL.Contract
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangeAsync();
    }
}
