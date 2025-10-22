using BookMoney.Data;
using BookMoney.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMoney.Services;

public interface ILoginService
{
    Task<List<LoginDBModel>> GetLoginsAsync();
    Task<LoginDBModel> GetLoginAsync(int id);
    Task<LoginDBModel> AddLoginAsync(LoginDBModel user);
    Task<LoginDBModel> UpdateLoginAsync(LoginDBModel user);
    Task<bool> DeleteLoginAsync(int id);
    Task<bool> ActivateLoginAsync(int id);
}

public class LoginService(AppDbContext context) : ILoginService
{
    public async Task<LoginDBModel> AddLoginAsync(LoginDBModel user)
    {
        context.Logins.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ActivateLoginAsync(int id)
    {
        var user = await context.Logins.FindAsync(id);
        if (user == null) return false;

        user.IsActive = true;
        context.Logins.Update(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteLoginAsync(int id)
    {
        var user = await context.Logins.FindAsync(id);
        if (user == null) return false;

        user.IsActive = false;
        context.Logins.Update(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<LoginDBModel> GetLoginAsync(int id)
    {
        return await context.Logins.FindAsync(id);
    }

    public async Task<List<LoginDBModel>> GetLoginsAsync()
    {
        return await context.Logins.ToListAsync();
    }

    public async Task<LoginDBModel> UpdateLoginAsync(LoginDBModel user)
    {
        context.Logins.Update(user);
        await context.SaveChangesAsync();
        return user;
    }
}
