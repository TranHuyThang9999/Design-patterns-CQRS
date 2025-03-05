using Microsoft.AspNetCore.Mvc;

namespace WebApplicationCQRS.Application.Resources;

public class Resources
{
    public static ActionResult MapResponse<T>(ControllerBase controller, Result<T> result)
    {
        return controller.StatusCode((int)result.StatusCode, new
        {
            result.Code,
            result.Message,
            result.Data
        });
    }
}