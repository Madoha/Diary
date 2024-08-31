namespace Diary.Domain.Enum;

public enum ErrorCodes
{
    // 0 - 9 Report
    ReportsNotFound = 0,
    ReportNotFound = 1,
    ReportAlreadyExists = 2,
    
    UserNotFound = 11,
    UserAlreadyExists = 12,
    UserUnauthorizedAccess = 13,
    UserAlreadyHasThisRole = 14,
    
    PasswordNotEqualToPasswordConfirm = 21,
    PasswordIsWrong = 22,
    
    RoleAlreadyExists = 31,
    RoleNotFound = 32,
    
    InternalServerError = 10
}