using AutoMapper;
using Diary.Application.Resources;
using Diary.Domain.Dto.Role;
using Diary.Domain.Entity;
using Diary.Domain.Enum;
using Diary.Domain.Interfaces.Repositories;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace Diary.Application.Services;

public class RoleService : IRoleService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IMapper _mapper;

    public RoleService(IBaseRepository<User> userRepository,
        IBaseRepository<Role> roleRepository, IMapper mapper, 
        IBaseRepository<UserRole> userRoleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _userRoleRepository = userRoleRepository;
    }
    
    public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == dto.Name);
        if (role != null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleAlreadyExists,
                ErrorCode = (int)ErrorCodes.RoleAlreadyExists
            };
        }

        role = new Role()
        {
            Name = dto.Name,
        };
        await _roleRepository.CreateAsync(role);
        await _roleRepository.SaveChangesAsync();

        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == dto.Name);
        if (role != null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        role.Name = dto.Name;

        await _roleRepository.UpdateAsync(role);
        await _roleRepository.SaveChangesAsync();
        
        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Id == id);
        if (role != null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        await _roleRepository.DeleteAsync(role);
        await _roleRepository.SaveChangesAsync();

        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<UserRoleDto>> AddRoleToUserAsync(UserRoleDto dto)
    {
        var user = await _userRepository.GetAll().Include(x => x.Roles).FirstOrDefaultAsync(u => u.Login == dto.Login);
        if (user == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }
        
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == dto.RoleName);
        if (role == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        var roles = user.Roles.Select(x => x.Name).ToArray();
        if (roles.Any(x => x == dto.RoleName))
        {
            UserRole userRole = new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id
            };
            await _userRoleRepository.CreateAsync(userRole);
            await _userRoleRepository.SaveChangesAsync();
            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto()
                {
                    Login = user.Login,
                    RoleName = role.Name
                }
            };
        }

        return new BaseResult<UserRoleDto>()
        {
            ErrorMessage = ErrorMessage.UserAlreadyHasThisRole,
            ErrorCode = (int)ErrorCodes.UserAlreadyHasThisRole
        };
    }

    public async Task<BaseResult<UserRoleDto>> DeleteRoleFromUserAsync(UserRoleDto dto)
    {
        var user = await _userRepository.GetAll()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(u => u.Login == dto.Login);
        if (user == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }
        
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == dto.RoleName);
        if (role == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }
        
        var userRole = await _userRoleRepository.GetAll()
            .Where(x => x.RoleId == role.Id)
            .FirstOrDefaultAsync(x => x.UserId == user.Id);

        await _userRoleRepository.DeleteAsync(userRole);
        await _userRoleRepository.SaveChangesAsync();

        return new BaseResult<UserRoleDto>()
        {
            Data = new UserRoleDto()
            {
                Login = user.Login,
                RoleName = role.Name
            }
        };
    }
}