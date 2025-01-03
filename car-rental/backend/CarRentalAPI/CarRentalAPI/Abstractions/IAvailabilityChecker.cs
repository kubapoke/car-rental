using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Abstractions
{
    public interface IAvailabilityChecker
    {
        public List<int> GetNotAvailableCarIds(List<CarIdRentDatesDto> pairs, DateTime startDate, DateTime endDate);
    }
}