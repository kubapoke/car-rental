namespace CarRentalAPI.Abstractions
{
    public interface ISessionTokenManager
    {
        public string GetSessionToken(string userName);
    }
}