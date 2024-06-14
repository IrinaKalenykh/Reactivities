using System.Text.Json;

namespace API.Extentions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, 
        int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationalHeader = new
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationalHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}