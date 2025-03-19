namespace Hatt.Models
{
    public class RegisterModel
    {
        public string Firstname { get; set; } = string.Empty!;   
        public string Lastname { get; set; } = string.Empty!;
        public string UserName { get; set; } = string.Empty!;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty!;

    }

    public class LoginModel
    { 
        public string Username { get; set; } = string.Empty!;
        public string Password { get; set; } = string.Empty!;    
    }
}
