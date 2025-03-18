using System;
using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Hatt.Services;

//User Service Interface
public interface IUserService
{
    Task<UserDisplayDto> GetUserByEmailAsync(string email);
    Task<UserDisplayDto?> GetUserByUserNameAsync(string UserName);
    Task<ICollection<FriendRequestDisplayDto>> GetUserFriends(string username);
    Task<FriendRequestDisplayDto> SendFriendRequest(string senderId, string recieverUsername);
    Task<FriendshipStatus> HandleFriendRequest(FriendshipStatusMotive status, int requestId);
    Task<ICollection<FriendRequest>> GetPendingSentRequests(string mySenderId);
    Task<ICollection<FriendRequest>> GetPendingRecievedRequests(string mySenderId);

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

    //Getting user friends
    public async Task<ICollection<FriendRequestDisplayDto>> GetUserFriends(string username)
    {
        var friends = await _userRepository.GetUserFriends(username);

        return friends.Select(f => f.AsDisplayDto()).ToList();
    }

    //Sending friend request to specific user with username
    public async Task<FriendRequestDisplayDto> SendFriendRequest(string mySenderId, string recieverUsername)
    {
        var reciever = await _userRepository.GetUserByUserNameAsync(recieverUsername) 
            ?? throw new KeyNotFoundException("User not found");

        if(mySenderId == reciever.Id)
        {
            throw new InvalidOperationException(
                "You cannot send a friend request to yourself silly. Go make some friends");
        }

        var existingFriend = await _userRepository.GetUserFriends(recieverUsername);
        if (existingFriend.Any(fr =>
            (fr.SenderId == mySenderId && fr.RecieverId == reciever.Id ||
             fr.SenderId == reciever.Id && fr.RecieverId == mySenderId)
            && fr.Status == FriendshipStatus.Accepted))
        {
            throw new InvalidOperationException("You are already friends with this user");
        }


        var pendingRecievedRequest = await _userRepository.GetPendingRecievedRequests(mySenderId);
        //If the recieverId field in the friend request points to you
        if (pendingRecievedRequest.Any(n => (n.RecieverId == mySenderId) && (n.SenderId == reciever.Id)))
        {
            throw new InvalidOperationException(
                "This user already sent you a friend request. Head over to the request page to accept it.");
        }

        var pendingSentRequest = await _userRepository.GetPendingSentRequests(mySenderId);
        //If you sent a request to this user already
        if (pendingSentRequest.Any(n => (n.RecieverId == reciever.Id) && (n.SenderId == mySenderId)))
        {
            throw new InvalidOperationException("Already sent a request to this user");
        }

        var sentRequest = await _userRepository.SendFriendRequest(mySenderId, reciever.Id);
        return sentRequest.AsDisplayDto();
    }

    //Handling friend request: Accepting or Declining
    public async Task<FriendshipStatus> HandleFriendRequest(FriendshipStatusMotive motive, int requestId)
    {
        var request = await _userRepository.GetFriendRequestById(requestId) ?? throw new KeyNotFoundException("Friend request not found");

        await _userRepository.UpdateFriendRequest(motive, request);
        return request.Status;
    }

    //Get all recieved requests not yet accepted or declined
    public async Task<ICollection<FriendRequest>> GetPendingRecievedRequests(string mySenderId)
    {
        var pendingRecievedRequests = await _userRepository.GetPendingRecievedRequests(mySenderId);
        return pendingRecievedRequests;
    }

    //Get all sent request not yet accepted or declined
    public async Task<ICollection<FriendRequest>> GetPendingSentRequests(string mySenderId)
    {
        var pendingSentRequests = await _userRepository.GetPendingSentRequests(mySenderId);
        return pendingSentRequests;
    }
}

