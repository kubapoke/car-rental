using CarSearchAPI.Abstractions;

namespace CarSearchAPI.Services.ProviderServices
{
    public class ProviderServiceFactory : IProviderServiceFactory
    {
        private readonly Dictionary<string, IProviderCarService> _carServices;
        private readonly Dictionary<string, IProviderOfferService> _offerServices;
        private readonly Dictionary<string, IProviderRentService> _rentServices;

        public ProviderServiceFactory(
            IEnumerable<IProviderCarService> carServices,
            IEnumerable<IProviderOfferService> offerServices,
            IEnumerable<IProviderRentService> rentServices)
        {
            _carServices = carServices.ToDictionary(service => service.GetProviderName());
            _offerServices = offerServices.ToDictionary(service => service.GetProviderName());
            _rentServices = rentServices.ToDictionary(service => service.GetProviderName());
        }


        public IProviderCarService GetProviderCarService(string providerName)
        {
            return _carServices[providerName];
        }

        public IProviderOfferService GetProviderOfferService(string providerName)
        {
            return _offerServices[providerName];
        }

        public IProviderRentService GetProviderRentService(string providerName)
        {
            return _rentServices[providerName];
        }
    }
}
