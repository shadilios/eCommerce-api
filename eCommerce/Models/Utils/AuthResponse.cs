namespace eCommerce.Models.Utils
{
    public class AuthResponse
    {
        public object Token { get; set; }
        public object ExpiryDate { get; set; }
        public object UserId { get; set; }
        public object UserName { get; set; }
        public object Roles { get; set; }
    }
}
