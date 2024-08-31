using AutoMapper;
using Diary.Domain.Dto.Report;
using Diary.Domain.Entity;

namespace Diary.Application.Mapping;

public class ReportMapping : Profile
{
    public ReportMapping()
    {
        CreateMap<Report, ReportDto>()
            .ForCtorParam(ctorParamName: "Id", m => m.MapFrom(x => x.Id))
            .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(x => x.Name))
            .ForCtorParam(ctorParamName: "Description", m => m.MapFrom(x => x.Description))
            .ForCtorParam(ctorParamName: "DateCreated", m => m.MapFrom(x => x.CreatedAt))
            .ReverseMap();
    }    
}