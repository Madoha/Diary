using Diary.Domain.Dto.Role;
using Diary.Domain.Entity;
using Diary.Domain.Result;

namespace Diary.Domain.Interfaces.Services;

public interface IRoleService
{
    Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto);
    Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto);
    Task<BaseResult<RoleDto>> DeleteRoleAsync(long id);
    Task<BaseResult<UserRoleDto>> AddRoleToUserAsync(UserRoleDto dto);
    Task<BaseResult<UserRoleDto>> DeleteRoleFromUserAsync(UserRoleDto dto);
}