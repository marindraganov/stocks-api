namespace StocksAPI.Controllers.Models
{
    public class UserLoginResponse
    {
        public int UserID { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int AvatarID { get; set; }
    }
}
