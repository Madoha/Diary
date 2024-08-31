using Asp.Versioning;
using Diary.Domain.Dto.Report;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Api.Controllers;

[Authorize]
[ApiController]
// [ApiVersion("1.0")]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    
    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Creates a new report.
    /// </summary>
    /// <param name="reportDto"></param>
    /// <remarks>
    /// <h3>Create a new report for the given user:</h3>
    /// 
    ///     POST
    ///     {
    ///         "name": "Report #1",
    ///         "description": "Test report",
    ///         "userId": 1
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">If the report is successfully created.</response>
    /// <response code="400">If there was an error during creation.</response>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> CreateReport([FromBody] CreateReportDto reportDto)
    {
        var response = await _reportService.CreateReportAsync(reportDto);
        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
    
    /// <summary>
    /// Deletes a report by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>
    /// <h3>Remove a report using its unique ID:</h3>
    /// 
    ///     DELETE
    ///     {
    ///         "id": 1
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">If the report is successfully deleted.</response>
    /// <response code="400">If the deletion fails.</response>
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> DeleteReport(long id)
    {
        var response = await _reportService.DeleteReportAsync(id);
        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
    
    /// <summary>
    /// Retrieves all reports for a specified user.
    /// </summary>
    /// <param name="userId"></param>
    /// <remarks>
    ///<h3>Returns a list of reports for a specific user:</h3>
    /// 
    ///     GET
    ///     {
    ///         "userId": 1
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">If reports are successfully retrieved.</response>
    /// <response code="400">If there's an error retrieving the reports.</response>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetUserReports(long userId)
    {
        var response = await _reportService.GetReportsAsync(userId);
        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
    
    /// <summary>
    /// Retrieves a report by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>
    ///<h3>Returns a single report using its unique ID:</h3>
    /// 
    ///     GET
    ///     {
    ///         "id": 1
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">If the report is successfully retrieved.</response>
    /// <response code="400">If there's an error retrieving the report.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetReportById(long id)
    {
        var response = await _reportService.GetReportByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
    
    /// <summary>
    /// Updates an existing report.
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    ///<h3>Update the details of an existing report:</h3>
    /// 
    ///     PUT
    ///     {
    ///         "id": 1,
    ///         "name": "report #1",
    ///         "description": "report desc"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">If the report is successfully updated.</response>
    /// <response code="400">If there's an error updating the report.</response>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> UpdateReport([FromBody] UpdateReportDto dto)
    {
        var response = await _reportService.UpdateReportAsync(dto);
        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
}