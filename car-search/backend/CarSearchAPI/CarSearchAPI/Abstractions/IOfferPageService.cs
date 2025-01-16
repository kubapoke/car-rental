using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;

namespace CarSearchAPI.Abstractions
{
    public interface IOfferPageService
    {
        public GetOfferAmountParametersDto GetGetOfferAmountParameters
            (string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string? email);
        public (int page, int pageSize) GetPageAndPageSize(int? providedPage, int? providedPageSize);
        public GetOfferListParametersDto GetGetOfferListParameters
            (string? brand, string? model, DateTime startDate, DateTime endDate, string? location, string email, int pageSize);
        public Task<OfferPageDto> GetOfferPageAsync
            (GetOfferAmountParametersDto offerAmountParametersDto, GetOfferListParametersDto offerListParametersDto,
            int page, int pageSize);

    }
}
