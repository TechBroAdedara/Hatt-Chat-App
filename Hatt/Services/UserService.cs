using System;
using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace Hatt.Services;

//User Service Interface
public interface IUserService
{
    Task<UserDisplayDto> GetUserByEmailAsync(string email);
    Task<UserDisplayDto?> GetUserByUserNameAsync(string UserName);
}

//User Service Implementation
public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;



    //Getting user by Email
    public async Task<UserDisplayDto> GetUserByEmailAsync(string email)
    {
        //Check if the user exists
        var user = await _userRepository.GetUserByEmailAsync(email) ?? throw new KeyNotFoundException("User not found");
        return user.ToUserDisplayDto();

    }

    //Getting user by UserName
    public async Task<UserDisplayDto?> GetUserByUserNameAsync(string UserName)
    {
        
        var user = await _userRepository.GetUserByUserNameAsync(UserName) ?? throw new KeyNotFoundException("User not found");
        return user.ToUserDisplayDto();

    }

}
