namespace Hatt.Models
{
    public class RegisterModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class LoginModel
    { 
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
