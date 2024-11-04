namespace CarSearchAPI.DTOs.Users
{
    public class NewUserInfoDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email {  get; set; }
        public DateOnly birthDate { get; set; }
        public DateOnly licenceDate {  get; set; }
    }
}
