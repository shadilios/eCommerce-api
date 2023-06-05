namespace eCommerce.Data.Auth
{
    public class AppUserDto
    {
        public string Username { get; set; }

        public DateTime AccountCreationDate { get; set; }
        public List<string> Roles { get; set; }
    }
}
