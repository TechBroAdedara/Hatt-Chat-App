using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hatt.Authentication
{
    public interface IAuthService
    {
        Task<UserDisplayDto?> CreateUserAsync(RegisterModel userModel);
        Task<string> Login(LoginModel model);
    }
    public class AuthService(
        IUserRepository userRepository,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration
        ) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;

        //REGISTER METHOD
        public async Task<UserDisplayDto?> CreateUserAsync(RegisterModel userModel)
        {
            //Check if the user already exists
            var existingUser = await _userRepository.GetUserByUserNameAsync(userModel.UserName)
                ?? await _userRepository.GetUserByEmailAsync(userModel.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with UserName '{userModel.UserName}' or email '{userModel.Email}' already exists");
            }
            var newUser = new User
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                Firstname = userModel.Firstname,
                Lastname = userModel.Lastname,
            };
            var result = await _userManager.CreateAsync(newUser, userModel.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return newUser.ToUserDisplayDto();
        }

        //LOGIN METHOD
        public async Task<string> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username) ?? await _userRepository.GetUserByUserNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = await GenerateJwtToken(user);
            return token;
        }

        //HELPER METHOD TO GENERATE JWT TOKEN
        private async Task<string> GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),

                        new Claim("username", user.UserName),
                        new Claim("firstname", user.Firstname),
                        new Claim("Lastname", user.Lastname)
                        
                        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        } 
    }
    
}
