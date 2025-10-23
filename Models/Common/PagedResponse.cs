using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models.Common
{
    [SwaggerSchema("Resposta paginada para listagens")]
    public class PagedResponse<T>
    {
        [SwaggerSchema("Dados da página atual")]
        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new List<T>();

        [SwaggerSchema("Número da página atual (base 1)")]
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [SwaggerSchema("Quantidade de itens por página")]
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [SwaggerSchema("Total de registros encontrados")]
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [SwaggerSchema("Total de páginas disponíveis")]
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [SwaggerSchema("Links de navegação HATEOAS")]
        [JsonPropertyName("links")]
        public List<Link> Links { get; set; } = new List<Link>();

        [SwaggerSchema("Mensagem descritiva")]
        [JsonPropertyName("message")]
        public string Message { get; set; } = "Dados paginados recuperados com sucesso";

        public PagedResponse() { }

        public PagedResponse(List<T> data, int page, int pageSize, int totalCount, List<Link> links, string message = "Dados paginados recuperados com sucesso")
        {
            Data = data;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Links = links;
            Message = message;
        }
    }

    [SwaggerSchema("Link HATEOAS para navegação na API")]
    public class Link
    {
        [SwaggerSchema("Relação do link (self, next, prev, create, etc)")]
        [JsonPropertyName("rel")]
        public string Rel { get; set; } = string.Empty;

        [SwaggerSchema("URL do recurso")]
        [JsonPropertyName("href")]
        public string Href { get; set; } = string.Empty;

        [SwaggerSchema("Método HTTP permitido")]
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;

        public Link() { }

        public Link(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }
    }
}