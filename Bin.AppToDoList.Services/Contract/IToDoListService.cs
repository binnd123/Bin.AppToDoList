using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bin.AppToDoList.Common.DTO;
using Bin.AppToDoList.Common.DTO.Request;

namespace Bin.AppToDoList.Services.Contract
{
    public interface IToDoListService
    {
        public Task<ResponseDTO> GetAllToDoList(int pageNumber, int pageSize);
        public Task<ResponseDTO> CreateToDoList(CreateToDoListRequestDTO request);
        public Task<ResponseDTO> UpdateToDoList(Guid toDoListId, UpdateToDoListRequestDTO request);
        public Task<ResponseDTO> DeleteToDoList(Guid toDoListId);
    }
}
