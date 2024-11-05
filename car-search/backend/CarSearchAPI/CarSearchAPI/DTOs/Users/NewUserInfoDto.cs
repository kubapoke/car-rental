namespace CarSearchAPI.DTOs.Users
{
    // Used to get information from new-user-form from front-end, email is not here because it will be in the bearer token
    public class NewUserInfoDto
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateOnly birthDate { get; set; }
        public DateOnly licenceDate {  get; set; }
    }
}
