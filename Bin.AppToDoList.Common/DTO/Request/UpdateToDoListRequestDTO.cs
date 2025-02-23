using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bin.AppToDoList.Common.DTO.Request
{
    public class UpdateToDoListRequestDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CompleteAt { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
    }
}
