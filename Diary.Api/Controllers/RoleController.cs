using System.Net.Mime;
using Diary.Domain.Dto.Role;
using Diary.Domain.Entity;
using Diary.Domain.Enum;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Api.Controllers;

[Consumes(MediaTypeNames.Application.Json)]
[Authorize(Roles = nameof(Roles.User))]
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<BaseResult<RoleDto>>> Create([FromBody] CreateRoleDto dto)
    {
        var response = await _roleService.CreateRoleAsync(dto);
        if (response.IsSuccess)
            return Ok(response);
        
        return BadRequest(response);
    }
    
    [HttpDelete("delete")]
    public async Task<ActionResult<BaseResult<RoleDto>>> Delete(long id)
    {
        var response = await _roleService.DeleteRoleAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        
        return BadRequest(response);
    }
    
    [HttpPut("update")]
    public async Task<ActionResult<BaseResult<RoleDto>>> Update([FromBody] RoleDto dto)
    {
        var response = await _roleService.UpdateRoleAsync(dto);
        if (response.IsSuccess)
            return Ok(response);
        
        return BadRequest(response);
    }
    
    [HttpPost("user/add-role")]
    public async Task<ActionResult<BaseResult<UserRoleDto>>> AddRoleTouser([FromBody] UserRoleDto dto)
    {
        var response = await _roleService.AddRoleToUserAsync(dto);
        if (response.IsSuccess)
            return Ok(response);
        
        return BadRequest(response);
    }
    
    [HttpDelete("user/delete-role")]
    public async Task<ActionResult<BaseResult<UserRoleDto>>> Delete(UserRoleDto dto)
    {
        var response = await _roleService.DeleteRoleFromUserAsync(dto);
        if (response.IsSuccess)
            return Ok(response);
        
        return BadRequest(response);
    }
}