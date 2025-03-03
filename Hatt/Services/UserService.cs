using System;
using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;

namespace Hatt.Services;
public interface IUserService{
    Task<DisplayUserDto> GetUserByIdAsync(int userId);
    Task<DisplayUserDto?> AddUserAsync(AddUserDto userDto);
}
public class UserService(IUserRepository userRepository): IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<DisplayUserDto> GetUserByIdAsync(int userId)
    {
        try{
            var user = await _userRepository.GetUserByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            return user.ToDisplayUserDto();
        }
        catch (Exception)
        {
            throw;
        }
        
    }
    public async Task<DisplayUserDto?> GetUserByUsernameAsync(string username)
    {
        try{
            var existingUser = await _userRepository.GetUserByUsernameAsync(username) ?? throw new KeyNotFoundException("User not found");
            return existingUser.ToDisplayUserDto();
        }
        catch (Exception)
        {
            throw;
        }
        
    }
    public async Task<DisplayUserDto?> AddUserAsync(AddUserDto userDto)
    {
        try{
            ArgumentNullException.ThrowIfNull(userDto);

            var existingUser = await GetUserByUsernameAsync(userDto.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with username '{userDto.Username}' already exists");
            }

            var newUser = await _userRepository.AddUserAsync(userDto);
            return newUser.ToDisplayUserDto();
        }
        catch (Exception)
        {
            throw;
        }
        
    }
}
