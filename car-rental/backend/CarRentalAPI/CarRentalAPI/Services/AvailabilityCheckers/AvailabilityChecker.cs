using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Services.AvailabilityCheckers
{
    public class AvailabilityChecker : IAvailabilityChecker
    {
        public List<int> GetNotAvailableCarIds(List<CarIdRentDatesDto> pairs, DateTime startDate, DateTime endDate)
        {
            List<int> notAvailableCarIds = new List<int>();
            foreach (var pair in pairs)
            {
                if (DoIntervalsCollide(pair.StartDate, pair.EndDate, startDate, endDate)) notAvailableCarIds.Add(pair.CarId);
            }
            return notAvailableCarIds;
        }

        private bool DoIntervalsCollide(DateTime start1,  DateTime end1, DateTime start2, DateTime end2)
        {
            if ((end2 <= end1) && (end2 >= start1)) return true;
            if ((start2 >= start1) && (start2 <= end1)) return true;
            return false;
        }
    }
}
