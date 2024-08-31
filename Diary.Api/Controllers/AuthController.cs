using Diary.Domain.Dto.User;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Diary.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// User registration
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto dto)
    {
        var response = await _authService.Register(dto);
        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUserDto dto)
    {
        var response = await _authService.Login(dto);
        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
}