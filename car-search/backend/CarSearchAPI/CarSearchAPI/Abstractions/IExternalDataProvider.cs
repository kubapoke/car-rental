namespace CarSearchAPI.Abstractions;

public interface IExternalDataProvider
{
    // This interface exists in order to create a generalized structure for getting data from different APIs
    public Task<string> GetDataAsync(string endpoint, Dictionary<string, string> parameters);
}