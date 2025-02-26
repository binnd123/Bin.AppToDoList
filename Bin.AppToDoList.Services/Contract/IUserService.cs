using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bin.AppToDoList.Common.DTO.Request;
using Bin.AppToDoList.Common.DTO;

namespace Bin.AppToDoList.Services.Contract
{
    public interface IUserService
    {
        public Task<ResponseDTO> SignUp(SignUpDTO request);
        public Task<ResponseDTO> Login(LoginRequestDTO request);
        public Task<ResponseDTO> RefreshToken(string refreshToken);
    }
}
