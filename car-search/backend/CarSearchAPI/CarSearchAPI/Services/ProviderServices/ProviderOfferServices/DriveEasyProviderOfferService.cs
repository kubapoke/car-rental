using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.DriveEasy;
using CarSearchAPI.DTOs.ForwardingParameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace CarSearchAPI.Services.ProviderServices.ProviderOfferServices
{
    public class DriveEasyProviderOfferService : IProviderOfferService
    {
        private const string OfferListEndpoint = "/api/Offer";

        public async Task<int> GetOfferAmountAsync(HttpClient client, string url, GetOfferAmountParametersDto parameters,
            string? customerId)
        {
            url += OfferListEndpoint;
            
            var queryParameters = new Dictionary<string, string?>()
            {
                { "model", parameters.Model },
                { "brand", parameters.Brand },
                { "startDate", parameters.StartDate.ToString("o") },
                { "endDate", parameters.EndDate.ToString("o") },
                { "clientId", customerId },
            };
            
            url = QueryHelpers.AddQueryString(url, 
                queryParameters.Where(p => p.Value != null));
            
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var offers = JsonConvert.DeserializeObject<List<DriveEasyOfferDto>>(responseContent);

                if (parameters.Location != null)
                {
                    offers = offers
                        .Where(o => o.Car.Location.Name.Equals(parameters.Location))
                        .ToList();
                }
                
                return offers.Count;
            }
            else
            {
                throw new Exception($"Error fetching data from \"/api/Offer\" at Drive Easy API");
            }
        }

        public async Task<List<OfferDto>> GetOfferListAsync(HttpClient client, string url,
            GetOfferListParametersDto parameters, string? customerId)
        {
            url += OfferListEndpoint;
            
            var queryParameters = new Dictionary<string, string?>()
            {
                { "model", parameters.Model },
                { "brand", parameters.Brand },
                { "startDate", parameters.StartDate.ToString("o") },
                { "endDate", parameters.EndDate.ToString("o") },
                { "clientId", customerId },
            };
            
            url = QueryHelpers.AddQueryString(url, 
                queryParameters.Where(p => p.Value != null));
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var driveEasyOffers = JsonConvert.DeserializeObject<List<DriveEasyOfferDto>>(responseContent);

                if (parameters.Location != null)
                {
                    driveEasyOffers = driveEasyOffers
                        .Where(o => o.Car.Location.Name.Equals(parameters.Location))
                        .ToList();
                }

                var offers = driveEasyOffers.Select(o => new OfferDto()
                {
                    OfferId = o.Id,
                    Brand = o.Car.Brand,
                    Model = o.Car.Model,
                    Email = parameters.Email,
                    Price = o.Price * DaysRented(o.StartDate, o.EndDate),
                    Conditions = "{}",
                    CompanyName = "DriveEasy",
                    Location = o.Car.Location.Name,
                    StartDate = o.StartDate,
                    EndDate = o.EndDate,
                }).ToList();

                if (parameters.Page != null || parameters.PageSize != null)
                {
                    var page = parameters.Page ?? 0;
                    var pageSize = parameters.PageSize ?? 6;
                    
                    offers = offers.Skip((page) * pageSize).Take(pageSize).ToList();
                }
                
                return offers;
            }
            else
            {
                throw new Exception($"Error fetching data from \"/api/Offer\" at Drive Easy API");
            }
        }
        
        public string GetProviderName()
        {
            return "DriveEasy";
        }

        private int DaysRented(DateTime startDate, DateTime endDate)
        {
            DateTime startDateOnly = startDate.Date;
            DateTime endDateOnly = endDate.Date;

            return (endDateOnly - startDateOnly).Days + 1;
        }
    }
}
