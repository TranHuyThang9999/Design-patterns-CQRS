public static class HttpContextHelper
{
    public static int? GetUserId(HttpContext httpContext)
    {
        if (httpContext.Items["userID"] is string userIdStr && int.TryParse(userIdStr, out int userId))
        {
            return userId;
        }
        return null;
    }
}