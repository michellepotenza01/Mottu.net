using Microsoft.AspNetCore.Mvc;
using MottuApi.Models.Common;
using Swashbuckle.AspNetCore.Annotations;
using MottuApi.Services;

namespace MottuApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
     public abstract class BaseController : ControllerBase
    {
        protected string RequestedApiVersion => HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        
        protected ActionResult HandleServiceResponse<T>(ServiceResponse<T> response)
        {
            if (!response.Success)
            {
                var errorResponse = new ErrorResponse 
                { 
                    Message = response.Message,
                    Path = $"{Request.Method} {Request.Path}",
                    Timestamp = DateTime.Now
                };
                return BadRequest(errorResponse);
            }

            if (response.Data == null)
                return NotFound(new ErrorResponse 
                { 
                    Message = "Recurso não encontrado",
                    Path = $"{Request.Method} {Request.Path}",
                    Timestamp = DateTime.Now
                });

            if (response.Data is System.Collections.IEnumerable enumerable && !enumerable.GetEnumerator().MoveNext())
                return NoContent();

            return Ok(new
            {
                response.Data,
                response.Message,
                Timestamp = DateTime.Now,
                Version = RequestedApiVersion
            });
        }

        protected ActionResult HandleCreatedResponse<T>(string actionName, object routeValues, T data, string message = "Recurso criado com sucesso")
        {
            return CreatedAtAction(actionName, routeValues, new
            {
                Data = data,
                Message = message,
                Timestamp = DateTime.Now,
                Version = RequestedApiVersion,
                Links = new[]
                {
                    new { Rel = "self", Href = Url.Action(actionName, routeValues), Method = "GET" },
                    new { Rel = "update", Href = Url.Action("Update", routeValues), Method = "PUT" },
                    new { Rel = "delete", Href = Url.Action("Delete", routeValues), Method = "DELETE" }
                }
            });
        }

        protected ErrorResponse CreateErrorResponse(string message, List<string>? errors = null)
        {
            if (errors == null && !ModelState.IsValid)
            {
                errors = ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    .ToList();
            }

            return new ErrorResponse 
            { 
                Message = message, 
                Errors = errors ?? new List<string>(),
                Path = $"{Request.Method} {Request.Path}",
                Timestamp = DateTime.Now
            };
        }

        protected bool IsCurrentUser(string username) => User.Identity?.Name == username;
    }
}