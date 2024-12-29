namespace CarRentalAPI.Abstractions
{
    public interface IPaginationService
    {
        public List<T> TrimListToPage<T>(List<T> source, int? page, int? pageSize);
    }
}