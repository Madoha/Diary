using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Diary.Application.Resources;
using Diary.Domain.Dto.User;
using Diary.Domain.Entity;
using Diary.Domain.Interfaces.Repositories;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Diary.Domain.Dto;
using Diary.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;
using Role = Diary.Domain.Entity.Role;

namespace Diary.Application.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserToken> _userTokenRepository;
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    
    public AuthService(IBaseRepository<User> userRepository,
        IBaseRepository<UserToken> userTokenRepository,
        ITokenService tokenService,
        ILogger logger,
        IMapper mapper, 
        IBaseRepository<Role> roleRepository, 
        IBaseRepository<UserRole> userRoleRepository, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userTokenRepository = userTokenRepository;
        _tokenService = tokenService;
        _logger = logger;
        _mapper = mapper;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
    {
        if (dto.Password != dto.PasswordConfirm)
        {
            return new BaseResult<UserDto>()
            {
                ErrorMessage = ErrorMessage.PasswordNotEqualToPasswordConfirm,
                ErrorCode = (int)ErrorCodes.PasswordNotEqualToPasswordConfirm
            };
        }
        var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Login == dto.Login); 
        if (user != null) 
        { 
            return new BaseResult<UserDto>() 
            { 
                ErrorMessage = ErrorMessage.UserAlreadyExists, 
                ErrorCode = (int)ErrorCodes.UserAlreadyExists
            };
        }
        
        var hashUserPassword = HashPassword(dto.Password);

        await using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                user = new User()
                {
                    Login = dto.Login,
                    Password = hashUserPassword

                };

                await _unitOfWork.Users.CreateAsync(user);
                
                await _unitOfWork.SaveChangesAsync();
                
                var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == nameof(Roles.User));
                if (role == null)
                {
                    return new BaseResult<UserDto>()
                    {
                        ErrorMessage = ErrorMessage.RoleNotFound,
                        ErrorCode = (int)ErrorCodes.RoleNotFound
                    };
                }
                
                UserRole userRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = role!.Id
                };

                await _unitOfWork.UserRoles.CreateAsync(userRole);

                await _unitOfWork.SaveChangesAsync();
                
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }
        }
        
        return new BaseResult<UserDto>()
        { 
            Data = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
    {
        try
        {
            var user = await _userRepository.GetAll()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Login == dto.Login);
            if (user == null)
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }
            
            if (!IsVerifyPassword(user.Password, dto.Password))
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordIsWrong,
                    ErrorCode = (int)ErrorCodes.PasswordIsWrong
                };
            }

            var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);

            var userRoles = user.Roles;
            var claims = userRoles.Select<Role, Claim>(x => new Claim(ClaimTypes.Role, x.Name)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));
            
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            if (userToken == null)
            {
                userToken = new UserToken()
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
                };
                
                await _userTokenRepository.CreateAsync(userToken);
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                
                await _userTokenRepository.UpdateAsync(userToken);
            }

            await _userTokenRepository.SaveChangesAsync();
            
            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    AccessToken = accessToken,
                    Refreshtoken = refreshToken
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new BaseResult<TokenDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes); // .Replace("-", "")
    }

    private bool IsVerifyPassword(string userPasswordHash, string userPassword)
    {
        var hashPassword = HashPassword(userPassword);
        if (userPasswordHash == hashPassword)
            return true;
        
        return false;
    }
}