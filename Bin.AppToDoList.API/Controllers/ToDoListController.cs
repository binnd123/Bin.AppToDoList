using Bin.AppToDoList.Common.DTO;
using Bin.AppToDoList.Common.DTO.Request;
using Bin.AppToDoList.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Bin.AppToDoList.API.Controllers
{
    [Route("api/toDoList")]
    [ApiController]
    public class ToDoListController
    {
        private IToDoListService service;

        public ToDoListController(IToDoListService service)
        {
            this.service = service;
        }
        [HttpGet("get-all-to-do-list")]
        public async Task<ResponseDTO> GetAllToDoList(int pageNumber, int pageSize)
        {
            return await service.GetAllToDoList(pageNumber, pageSize);
        }
        [HttpPost("create-to-do-list")]
        public async Task<ResponseDTO> CreateToDoList(CreateToDoListRequestDTO request)
        {
            return await service.CreateToDoList(request);
        }
        [HttpPut("update-to-do-list/{id}")]
        public async Task<ResponseDTO> UpdateToDoList(Guid id, UpdateToDoListRequestDTO request)
        {
            return await service.UpdateToDoList(id, request);
        }
        [HttpDelete("delete-to-do-list/{id}")]
        public async Task<ResponseDTO> DeleteToDoList(Guid id)
        {
            return await service.DeleteToDoList(id);
        }
    }
}
