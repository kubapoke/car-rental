using CarRentalAPI.Abstractions;

namespace CarRentalAPI.Services.PaginationServices
{
    public class PaginationService : IPaginationService
    {
        private const int DefaultPageSize = 6;
        
        public List<T> TrimToPage<T>(IEnumerable<T> source, int? page, int? pageSize)
        {
            if(page is null && pageSize is null)
                return source.ToList();
            
            int pageInt = Math.Max(page ?? 0, 0);
            int pageSizeInt = Math.Max(pageSize ?? DefaultPageSize, 1);
            
            var trimmedEnumerable = source
                .Skip(pageInt * pageSizeInt)
                .Take(pageSizeInt)
                .ToList();
            
            return trimmedEnumerable;
        }
    }
}