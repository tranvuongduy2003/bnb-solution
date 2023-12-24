using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers(); 
    Task<User> GetUserById(string userId);
    Task<User> GetUserByEmail(string email);
    Task<User> UpdateRefreshToken(string userId, string token);
    Task<User> UpdateUser(string userId, User updateUser);
    Task<User> UpdateUserStatus(string userId, bool activeStatus);

}