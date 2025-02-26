using Bin.AppToDoList.Common.DTO;
using Bin.AppToDoList.Common.DTO.Request;
using Bin.AppToDoList.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bin.AppToDoList.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("sign-up")]
        public async Task<ResponseDTO> SignUp(SignUpDTO request)
        {
            return await _service.SignUp(request);
        }

        [HttpPost("login")]
        public async Task<ResponseDTO> Login(LoginRequestDTO request)
        {
            return await _service.Login(request);
        }

        [HttpPost("refresh-token")]
        public async Task<ResponseDTO> RefreshToken(string refreshToken)
        {
            return await _service.RefreshToken(refreshToken);
        }
    }
}
