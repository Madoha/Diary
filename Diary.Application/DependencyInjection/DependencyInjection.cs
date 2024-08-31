using Diary.Application.Mapping;
using Diary.Application.Services;
using Diary.Application.Validations;
using Diary.Application.Validations.FluentValidations.Report;
using Diary.Domain.Dto.Report;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Interfaces.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Diary.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ReportMapping));
        services.InitServices();
    }

    private static void InitServices(this IServiceCollection services)
    {
        // FluentValidations
        services.AddScoped<IReportValidator, ReportValidator>();
        services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
        services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRoleService, RoleService>();
        
        services.AddScoped<IReportService, ReportService>();
    }
}