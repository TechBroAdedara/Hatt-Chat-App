using System;
using Hatt.Data;
using Hatt.Dtos;
using Hatt.Models;
using Microsoft.EntityFrameworkCore;

namespace Hatt.Repositories;
public interface IUserRepository{
    Task<User> AddUserAsync(AddUserDto userDto);
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string username);
}
public class UserRepository(HattDbContext context) : IUserRepository
{
    private readonly HattDbContext _context = context;

    //Method to add a new user to the database
    public async Task<User> AddUserAsync(AddUserDto userDto)
    {
        User newUser = new() {
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Email = userDto.Email,
            Username = userDto.Username,
            HashedPassword =userDto.Password
        } ;
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    //Method to get specific user by ID
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var newUser = await _context.Users.FindAsync(userId);
        return newUser;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(n => n.Username == username);
        return existingUser;
    }
}
