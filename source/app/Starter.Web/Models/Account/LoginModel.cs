namespace DQF.Web.Models.Account
{
    public class LoginModel
    {
        public string ReturnUrl { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}