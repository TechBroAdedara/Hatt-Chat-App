using System;
using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;
using BCrypt.Net;

namespace Hatt.Services;

//User Service Interface
public interface IUserService
{
    Task<UserDisplayDto> GetUserByEmailAsync(string email);
    Task<UserDisplayDto?> CreateUserAsync(UserCreateDto newUser);
    Task<UserDisplayDto?> GetUserByUsernameAsync(string username);
}

//User Service Implementation
public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    //Method to create user
    public async Task<UserDisplayDto?> CreateUserAsync(UserCreateDto newUser)
    {
 
        //If the passed argument is empty
        ArgumentNullException.ThrowIfNull(newUser);
            
        //Check if the user already exists
        var existingUser = await _userRepository.GetUserByUsernameAsync(newUser.Username) ?? await _userRepository.GetUserByEmailAsync(newUser.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with username '{newUser.Username}' or email '{newUser.Email}' already exists");
        }
        var hashed_password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
        var createdUser = await _userRepository.CreateUserAsync(newUser, hashed_password);
        return createdUser.ToUserDisplayDto();
 
    }

    //Getting user by Email
    public async Task<UserDisplayDto> GetUserByEmailAsync(string email)
    {
        //Check if the user exists
        var user = await _userRepository.GetUserByEmailAsync(email) ?? throw new KeyNotFoundException("User not found");
        return user.ToUserDisplayDto();

    }

    //Getting user by Username
    public async Task<UserDisplayDto?> GetUserByUsernameAsync(string username)
    {
        
        var existingUser = await _userRepository.GetUserByUsernameAsync(username) ?? throw new KeyNotFoundException("User not found");
        return existingUser.ToUserDisplayDto();

    }

}
