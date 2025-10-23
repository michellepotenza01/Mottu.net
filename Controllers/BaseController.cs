using Microsoft.AspNetCore.Mvc;
using MottuApi.Models.Common;
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
                return BadRequest(CreateErrorResponse(response.Message));
            }

            if (response.Data == null)
                return NotFound(CreateErrorResponse("Recurso não encontrado"));

            if (response.Data is System.Collections.IEnumerable enumerable && !enumerable.GetEnumerator().MoveNext())
                return NoContent();

            return Ok(new
            {
                Data = response.Data,
                Message = response.Message,
                Timestamp = DateTime.Now,
                Version = RequestedApiVersion
            });
        }

        protected ActionResult HandlePagedResponse<T>(List<T> data, int page, int pageSize, int totalCount, string message = "Dados recuperados com sucesso")
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var links = CreatePaginationLinks(baseUrl, page, pageSize, totalCount);
            
            var pagedResponse = new PagedResponse<T>(data, page, pageSize, totalCount, links, message);

            return Ok(new
            {
                pagedResponse.Data,
                pagedResponse.Page,
                pagedResponse.PageSize,
                pagedResponse.TotalCount,
                pagedResponse.TotalPages,
                pagedResponse.Links,
                Message = pagedResponse.Message,
                Timestamp = DateTime.Now,
                Version = RequestedApiVersion
            });
        }

        protected ActionResult HandleCreatedResponse<T>(string actionName, object routeValues, T data, string message = "Recurso criado com sucesso")
        {
            var links = new List<Link>
            {
                new Link("self", Url.Action(actionName, routeValues) ?? "", "GET"),
                new Link("update", Url.Action("Update", routeValues) ?? "", "PUT"),
                new Link("delete", Url.Action("Delete", routeValues) ?? "", "DELETE")
            };

            return CreatedAtAction(actionName, routeValues, new
            {
                Data = data,
                Message = message,
                Links = links,
                Timestamp = DateTime.Now,
                Version = RequestedApiVersion
            });
        }

        private List<Link> CreatePaginationLinks(string baseUrl, int page, int pageSize, int totalCount)
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var links = new List<Link>
            {
                new Link("self", $"{baseUrl}?pageNumber={page}&pageSize={pageSize}", "GET")
            };
            
            if (page > 1)
                links.Add(new Link("prev", $"{baseUrl}?pageNumber={page - 1}&pageSize={pageSize}", "GET"));
            
            if (page < totalPages)
                links.Add(new Link("next", $"{baseUrl}?pageNumber={page + 1}&pageSize={pageSize}", "GET"));

            return links;
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