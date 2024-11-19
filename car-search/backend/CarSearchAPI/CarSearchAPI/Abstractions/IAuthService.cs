namespace CarSearchAPI.Abstractions
{
    public interface IAuthService
    {
        Task<bool> VerifyToken(string idToken);
    }
}
