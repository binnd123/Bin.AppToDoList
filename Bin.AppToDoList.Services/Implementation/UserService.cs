using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Bin.AppToDoList.Common.DTO;
using Bin.AppToDoList.Common.DTO.BusinessCode;
using Bin.AppToDoList.Common.DTO.Request;
using Bin.AppToDoList.DAL.Contract;
using Bin.AppToDoList.DAL.Models;
using Bin.AppToDoList.Services.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Bin.AppToDoList.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IGenericRepository<Role> roleRepository, IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<ResponseDTO> RefreshToken(string refreshToken)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var userDb = await _userRepository.GetByExpression(a => a.RefreshToken == refreshToken, a => a.Role);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.INVALID_REFRESHTOKEN;
                    dto.IsSucess = false;
                }
                else if (userDb.ExpiredRefreshToken > DateTime.Now)
                {
                    var roleDB = await _roleRepository.GetByExpression(x => x.Id == userDb.RoleId, null);
                    var tokenSign = GenerateAccessToken(userDb, roleDB);
                    userDb.RefreshToken = "";
                    await _userRepository.Update(userDb);
                    await _unitOfWork.SaveChangeAsync();
                    dto.Data = new
                    {
                        AccessToken = tokenSign
                    };
                    dto.IsSucess = true;
                    dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                }
                else
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXPIRED_REFRESHTOKEN;
                }
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> SignUp(SignUpDTO request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var userDb = await _userRepository.GetByExpression(a => a.Email == request.Email, null);
                if (userDb != null)
                {
                    dto.BusinessCode = BusinessCode.EXISTED_USER;
                }
                else
                {
                    var roleDb = await _roleRepository.GetByExpression(x => x.Name.ToLower().Equals("user"));
                    string passWordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 12);

                    var user = new User()
                    {
                        UserId = Guid.NewGuid(),
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        PasswordHash = passWordHash,
                        RoleId = roleDb.Id
                    };
                    await _userRepository.Insert(user);
                    await _unitOfWork.SaveChangeAsync();
                    dto.BusinessCode = BusinessCode.SIGN_UP_SUCCESSFULLY;
                }
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> Login(LoginRequestDTO request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var userDb = await _userRepository.GetByExpression(a => a.Email == request.Email, a => a.Role);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                }
                else
                {
                    var roleDB = await _roleRepository.GetByExpression(x => x.Id == userDb.RoleId, null);
                    var isValid = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, userDb.PasswordHash);
                    if (!isValid)
                    {
                        dto.BusinessCode = BusinessCode.WRONG_PASSWORD;
                        dto.IsSucess = false;
                    }
                    else
                    {
                        var tokenSign = GenerateAccessToken(userDb, roleDB);
                        var refreshToken = GenerateRefreshToken();
                        var expiredRefreshToken = DateTime.Now.AddDays(double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
                        userDb.RefreshToken = refreshToken;
                        userDb.ExpiredRefreshToken = expiredRefreshToken;
                        await _userRepository.Update(userDb);
                        await _unitOfWork.SaveChangeAsync();
                        dto.Data = new
                        {
                            AccessToken = tokenSign,
                            RefreshToken = refreshToken
                        };
                        dto.IsSucess = true;
                        dto.BusinessCode = BusinessCode.SIGN_IN_SUCCESSFULLY;
                    }
                }
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        private string GenerateRefreshToken()
        {
            int byteLength = 30;
            var randomBytes = new byte[byteLength];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            string base64Token = Convert.ToBase64String(randomBytes)
                                      .Replace("+", "-")
                                      .Replace("/", "_")
                                      .TrimEnd('=');
            return base64Token.Length > 40 ? base64Token.Substring(0, 40) : base64Token;
        }
        private string GenerateAccessToken(User userDb, Role roleDb)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                     {
                         new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         new("AccountId", userDb.UserId.ToString()),
                          new Claim(ClaimTypes.Role,roleDb.Name.ToLower() )
                     };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
