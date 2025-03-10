using System;
using Hatt.Data;
using Hatt.Dtos;
using Hatt.Models;
using Microsoft.EntityFrameworkCore;

namespace Hatt.Repositories;
public interface IUserRepository
{
    Task<User> CreateUserAsync(UserCreateDto userDto, string hashed_password);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
}
public class UserRepository(HattDbContext context) : IUserRepository
{
    private readonly HattDbContext _context = context;

    //Method to add a new user to the database
    public async Task<User> CreateUserAsync(UserCreateDto userDto, string hashed_password)
    {
        User newUser = new()
        {
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Email = userDto.Email,
            Username = userDto.Username,
            HashedPassword = hashed_password
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

    //Method to get specific user by username
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(n => n.Username == username);
        return existingUser;
    }
}
