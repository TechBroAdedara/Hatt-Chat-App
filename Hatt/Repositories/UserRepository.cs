using System;
using Hatt.Data;
using Hatt.Dtos;
using Hatt.Models;
using Microsoft.EntityFrameworkCore;

namespace Hatt.Repositories;
public interface IUserRepository
{
    Task<User> CreateUserAsync(UserCreateDto userDto);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUserNameAsync(string UserName);
}
public class UserRepository(HattDbContext context) : IUserRepository
{
    private readonly HattDbContext _context = context;

    //Method to add a new user to the database
    public async Task<User> CreateUserAsync(UserCreateDto userDto)
    {
        User newUser = new()
        {
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Email = userDto.Email,
            UserName = userDto.UserName,
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    //Method to get specific user by ID
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(n => n.Email == email);
        return existingUser;
    }

    //Method to get specific user by UserName
    public async Task<User?> GetUserByUserNameAsync(string UserName)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(n => n.UserName == UserName);
        return existingUser;
    }
}
