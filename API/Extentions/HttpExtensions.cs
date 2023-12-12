using API.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;

namespace API.Extentions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response,PaginationHeader paginationHeader)
        {
            response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
