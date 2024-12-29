namespace CarRentalAPI.Abstractions
{
    public interface IPaginationService
    {
        public List<T> TrimToPage<T>(IEnumerable<T> source, int? page, int? pageSize);
    }
}