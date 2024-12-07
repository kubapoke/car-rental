﻿using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Services
{
    public class AvailabilityChecker
    {
        public List<int> CheckForAvailableCars(List<CarIdRentDatesDto> pairs, DateTime startDate, DateTime endDate)
        {
            List<int> availableCarIds = new List<int>();
            foreach (var pair in pairs)
            {
                if (IsIntervalsCollide(pair.StartDate, pair.EndDate, startDate, endDate)) continue;
                else availableCarIds.Add(pair.CarId);
            }
            return availableCarIds;
        }

        private bool IsIntervalsCollide(DateTime start1,  DateTime end1, DateTime start2, DateTime end2)
        {
            // Check if interval 2 collides with interval 1
            if ((end2 <= end1) && (end2 >= start1)) return true;
            if ((start2 >= start1) && (start2 <= end1)) return true;
            return false;
        }
    }
}
