using Diary.Domain.Entity;
using Diary.Domain.Result;

namespace Diary.Domain.Interfaces.Validations;

public interface IReportValidator : IBaseValidator<Report>
{
    /// <summary>
    /// Checks if report exists, if report exists then you can not create
    /// Checks if user exists, if user not exists then you can not create
    /// </summary>
    /// <param name="report"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    BaseResult CreateValidator(Report report, User user);
}