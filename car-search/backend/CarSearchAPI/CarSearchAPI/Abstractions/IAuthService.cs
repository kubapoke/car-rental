namespace CarSearchAPI.Abstractions
{
    public interface IAuthService
    {
        Task<bool> VerifyTokenAsync(string idToken);

        Task<(string token, bool isTemporary)> GetTokenAndFlagAsync(string idToken);
    }
}
