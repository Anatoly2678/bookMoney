using BookMoney.Data;
using BookMoney.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookMoney.Services;

public interface ILoginService
{
    Task<List<LoginDBModel>> GetLoginsAsync();
    Task<LoginDBModel> GetLoginAsync(Guid id);
    Task<LoginDBModel> AddLoginAsync(LoginDBModel user);
    Task<LoginDBModel> UpdateLoginAsync(LoginDBModel user);
    Task<bool> DeleteLoginAsync(Guid id);
    Task<bool> ActivateLoginAsync(Guid id);
    Task<Maybe<Guid>> GetIdByPhoneAsync(string phone);
    Task<Maybe<Guid>> AddLoginAsync(string phone);
}

public class LoginService(AppDbContext context) : ILoginService
{
    public async Task<LoginDBModel> AddLoginAsync(LoginDBModel user)
    {
        context.Logins.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<Maybe<Guid>> AddLoginAsync(string phone)
    {
        var login = new Models.LoginDBModel
        {
            IsActive = false,
            Login = phone,
        };

        try
        {
            await context.Logins.AddAsync(login);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException err)
            when (err.InnerException is PostgresException pgEx && pgEx.SqlState == "23505" && pgEx.Message.Contains("login_unique"))
        {
            var id = await GetIdByPhoneAsync(phone);
            return id.Value;
        }

        return login.Id;
    }

    public async Task<bool> ActivateLoginAsync(Guid id)
    {
        var user = await context.Logins.FindAsync(id);
        if (user == null) return false;

        user.IsActive = true;
        context.Logins.Update(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteLoginAsync(Guid id)
    {
        var user = await context.Logins.FindAsync(id);
        if (user == null) return false;

        user.IsActive = false;
        context.Logins.Update(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<LoginDBModel> GetLoginAsync(Guid id)
    {
        return await context.Logins!.FindAsync(id)!;
    }

    public async Task<Maybe<Guid>> GetIdByPhoneAsync(string phone) =>
    (await context.Logins!
        .SingleOrDefaultAsync(w => w.Login == phone))
        ?.Id ?? Maybe<Guid>.None;

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
