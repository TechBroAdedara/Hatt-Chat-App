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

    //Friend Requests Methods Below
    Task<FriendRequest?> GetFriendRequestById(int requestId); //TODO: Implement
    Task<ICollection<FriendRequest>> GetUserFriends(string myUsername); //Getting Users friends
    Task<FriendRequest> SendFriendRequest(string senderId, string recieverId); //Sending Friend Request
    Task UpdateFriendRequest(FriendshipStatusMotive motive, FriendRequest request ); //Handling a friend request - Accept or Decline
    Task<ICollection<FriendRequestDisplayDto>> GetPendingRecievedRequests(string mySenderId); //Get all pending friend requests recieved by user
    Task<ICollection<FriendRequestDisplayDto>> GetPendingSentRequests(string mySenderId); //Get all pending friend requests sent by user

}

//IMPLEMENTATION
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
        await _context.Users.AddAsync(newUser);
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

    //Get all user friends
    public async Task<ICollection<FriendRequest>> GetUserFriends(string myUsername)
    {
        var user = await GetUserByUserNameAsync(myUsername) ?? throw new KeyNotFoundException("User not found");

        var friends = await _context.FriendRequests
            .Where(
                n => (n.SenderId == user.Id || n.RecieverId == user.Id) //If your Id is the sender or reciever...
                && n.Status == FriendshipStatus.Accepted // As long as request is accepted, you are friends
            )
            .Include(n => n.Sender)
            .Include(n => n.Reciever)
            .ToListAsync();

        return friends;
    }

    //Send a friend request to a specific user
    public async Task<FriendRequest> SendFriendRequest(string senderId, string recieverId)
    {        
        var friendRequest = new FriendRequest
        {
            SenderId = senderId, //You, the sender
            RecieverId = recieverId,
            Status = FriendshipStatus.Pending,
            SentAt = DateTime.Now
        };
        await _context.FriendRequests.AddAsync(friendRequest);
        await _context.SaveChangesAsync();

        var request = await GetFriendRequestById(friendRequest.Id);
        return request;
    }

    //Getting friend request by specific Id
    public async Task<FriendRequest?> GetFriendRequestById(int requestId)
    {
        var friendRequest = await _context.FriendRequests
            .Include(fr => fr.Sender)
            .Include(fr => fr.Reciever)
            .FirstOrDefaultAsync(fr => fr.Id == requestId);
        return friendRequest;
    }
    
    //Get pending requests recieved by user
    //Will be supplied specific user's Id to check if they have a friend request from them
    public async Task<ICollection<FriendRequestDisplayDto>> GetPendingRecievedRequests(string mySenderId)
    {
        var pendingRecievedRequests = await _context.FriendRequests
            .Where(n => n.RecieverId == mySenderId //You, the reciever
                    && n.Status == FriendshipStatus.Pending)
            .Include(n => n.Sender)
            .Include(n=> n.Reciever)
            .Select(n => n.AsDisplayDto())
            .ToListAsync();
        return pendingRecievedRequests;
    }

    //Get pending requests sent by user
    public async Task<ICollection<FriendRequestDisplayDto>> GetPendingSentRequests(string mySenderId)
    {
        var pendingSentRequests = await _context.FriendRequests
            .Where(n => n.SenderId== mySenderId
                    && n.Status == FriendshipStatus.Pending)
            .Include(n => n.Sender)
            .Include(n => n.Reciever)
            .Select (n => n.AsDisplayDto())
            .ToListAsync();
        return pendingSentRequests;
    }

    public async Task UpdateFriendRequest(FriendshipStatusMotive motive, FriendRequest request)
    {
        var friendRequest = await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.Id == request.Id);

        friendRequest.Status = motive switch
        {
            FriendshipStatusMotive.Accept => FriendshipStatus.Accepted,
            FriendshipStatusMotive.Decline => FriendshipStatus.Declined,
            FriendshipStatusMotive.Block => FriendshipStatus.Blocked,
            _ => throw new ArgumentOutOfRangeException(nameof(motive), "Invalid Motive"),
        };
        await _context.SaveChangesAsync();
    }
}
