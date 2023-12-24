using BnB.Api.Data;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        var users = await _db.Users.ToListAsync();
        return users;
    }

    public async Task<User> GetUserById(string userId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        return user;
    }

    public async Task<User> UpdateRefreshToken(string userId, string token)
    {
        var user = await this.GetUserById(userId);
        user.RefreshToken = token;
        _db.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(string userId, User updateUser)
    {
        var user = await _db.Users.FirstOrDefaultAsync(p => p.Id == userId);

        if (user is null)
        {
            return null;
        }
        
        updateUser.UpdatedAt = DateTime.Now;
        _db.Users.Update(updateUser);
        await _db.SaveChangesAsync();
        
        return updateUser;
    }

    public async Task<User> UpdateUserStatus(string userId, bool activeStatus)
    {
        var user = _db.Users.FirstOrDefault(p => p.Id == userId);

        if (user is null)
        {
            return null;
        }
        
        user.UpdatedAt = DateTime.Now;
        user.IsActive = activeStatus;
        await _db.SaveChangesAsync();
        
        return user;
    }
}