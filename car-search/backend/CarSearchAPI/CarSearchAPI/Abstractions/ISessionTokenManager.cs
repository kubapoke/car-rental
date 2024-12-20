namespace CarSearchAPI.Abstractions
{
    public interface ISessionTokenManager
    {
        public string GetSessionToken(string email, bool isTemporary);
    }
}
