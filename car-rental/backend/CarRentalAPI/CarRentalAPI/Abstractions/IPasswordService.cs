namespace CarRentalAPI.Abstractions
{
    public interface IPasswordService
    {
        public (string hash, string salt) HashPassword(string password);

        public bool VerifyPassword(string password, string hash, string salt);
    }
}