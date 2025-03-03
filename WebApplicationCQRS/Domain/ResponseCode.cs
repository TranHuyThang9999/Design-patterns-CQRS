namespace WebApplicationCQRS.Domain;

public enum ResponseCode
{
    Success = 0,
    ValidationError = 1,
    NotFound = 2,
    InternalError = 3,
    Conflict = 4
}