namespace CarSearchAPI.DTOs.Users
{
    public class NewUserInfoDto
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateOnly birthDate { get; set; }
        public DateOnly licenceDate {  get; set; }
    }
}
