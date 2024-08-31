using Diary.Domain.Dto.Report;
using Diary.Domain.Result;

namespace Diary.Domain.Interfaces.Services;

/// <summary>
/// Service for report 
/// </summary>
public interface IReportService
{
    /// <summary>
    /// get all user reports
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<CollectionResult<ReportDto>> GetReportsAsync(long userId);
    /// <summary>
    /// get report by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> GetReportByIdAsync(long id);
    
    /// <summary>
    /// create a report
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto);
    
    /// <summary>
    /// update the report
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto);
    
    /// <summary>
    /// delele the report
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> DeleteReportAsync(long id);
}