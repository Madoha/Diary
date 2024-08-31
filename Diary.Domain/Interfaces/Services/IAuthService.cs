using Diary.Domain.Dto.User;
using Diary.Domain.Result;
using Diary.Domain.Dto;

namespace Diary.Domain.Interfaces.Services;

/// <summary>
/// Service for authen/author
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// register user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<UserDto>> Register(RegisterUserDto dto);
    
    /// <summary>
    /// auth user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<TokenDto>> Login(LoginUserDto dto);
}