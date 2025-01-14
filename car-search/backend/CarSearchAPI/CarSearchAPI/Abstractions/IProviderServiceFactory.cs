namespace CarSearchAPI.Abstractions
{
    public interface IProviderServiceFactory
    {
        public IProviderCarService GetProviderCarService(string providerName);
        public IProviderOfferService GetProviderOfferService(string providerName);
        public IProviderRentService GetProviderRentService(string providerName);
    }
}
