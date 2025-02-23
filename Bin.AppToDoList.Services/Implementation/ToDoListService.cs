using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bin.AppToDoList.Common.DTO;
using Bin.AppToDoList.Common.DTO.BusinessCode;
using Bin.AppToDoList.Common.DTO.Request;
using Bin.AppToDoList.DAL.Contract;
using Bin.AppToDoList.DAL.Models;
using Bin.AppToDoList.Services.Contract;

namespace Bin.AppToDoList.Services.Implementation
{
    public class ToDoListService : IToDoListService
    {
        private IGenericRepository<ToDoList> _toDoListRepository;
        private IUnitOfWork _unitOfWork;

        public ToDoListService(IGenericRepository<ToDoList> toDoListRepository, IUnitOfWork unitOfWork)
        {
            _toDoListRepository = toDoListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CreateToDoList(CreateToDoListRequestDTO request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var toDoList = new ToDoList
                {
                    ToDoListId = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    CreatedAt = DateTime.Now,
                    CompleteAt = request.CompleteAt,
                    IsActive = request.IsActive,
                    Status = request.Status,
                    UserName = request.UserName,
                };
                await _toDoListRepository.Insert(toDoList);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = toDoList;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> DeleteToDoList(Guid toDoListId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var toDoList = await _toDoListRepository.GetById(toDoListId);
                if (toDoList == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    return dto;
                }
                dto.Data = toDoList;
                await _toDoListRepository.DeleteById(toDoListId);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetAllToDoList(int pageNumber, int pageSize)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                dto.Data = await _toDoListRepository.GetAllDataByExpression(null, pageNumber, pageSize);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> UpdateToDoList(Guid toDoListId, UpdateToDoListRequestDTO request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var toDoList = await _toDoListRepository.GetById(toDoListId);
                if(toDoList == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    return dto;
                }
                if (!string.IsNullOrEmpty(request.Title))
                {
                    toDoList.Title = request.Title;
                }
                if (!string.IsNullOrEmpty(request.Description))
                {
                    toDoList.Description = request.Description;
                }
                if (!string.IsNullOrEmpty(request.Status))
                {
                    toDoList.Status = request.Status;
                }
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    toDoList.UserName = request.UserName;
                }
                if (request.CompleteAt != null)
                {
                    toDoList.CompleteAt = request.CompleteAt;
                }
                toDoList.IsActive = request.IsActive;
                await _toDoListRepository.Update(toDoList);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = toDoList;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }
    }
}
