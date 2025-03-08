using Microsoft.AspNetCore.Mvc;

namespace WebApplicationCQRS.Application.Resources;

public class Resources
{
    public static ActionResult MapResponse<T>(ControllerBase controller, Result<T> result)
    {
        if (result == null) 
        {
            return controller.BadRequest("Invalid response from server");
        }
        return controller.StatusCode((int)result.StatusCode, new
        {
            result.Code,
            result.Message,
            result.Data
        });
    }
}