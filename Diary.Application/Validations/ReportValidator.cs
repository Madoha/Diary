using Diary.Application.Resources;
using Diary.Domain.Entity;
using Diary.Domain.Interfaces.Validations;
using Diary.Domain.Result;
using Diary.Domain.Enum;

namespace Diary.Application.Validations;

public class ReportValidator : IReportValidator
{
    public BaseResult ValidateOnNull(Report? entity)
    {
        if (entity == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.ReportNotFound,
                ErrorCode = (int)ErrorCodes.ReportNotFound
            };
        }

        return new BaseResult();
    }

    public BaseResult CreateValidator(Report? report, User? user)
    {
        if (report != null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.ReportAlreadyExists,
                ErrorCode = (int)ErrorCodes.ReportAlreadyExists
            };
        }

        if (user == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }

        return new BaseResult();
    }
}