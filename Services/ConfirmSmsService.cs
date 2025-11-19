using BookMoney.Data;
using BookMoney.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace BookMoney.Services;

public interface IConfirmSmsService
{
    Task<UnitResult<string>> ConfirmAsync(Guid id);
    Task CreateAsync(Guid loginId, string smsCode);
}

public class ConfirmSmsService(AppDbContext context) : IConfirmSmsService
{
    public async Task CreateAsync(Guid loginId, string smsCode)
    {
        var sms = new ConfirmSmsDBModel
        {
            LoginId = loginId,
            SmsCode = smsCode,
        };

        await context.ConfirmSms.AddAsync(sms);
        await context.SaveChangesAsync();
    }

    public async Task<UnitResult<string>> ConfirmAsync(Guid id)
    {
        var sms = await context.ConfirmSms.SingleOrDefaultAsync(w => w.Id == id);
        if (sms == null)
            return $"Confirm id={id} not found";

        context.ConfirmSms.Update(sms);
        await context.SaveChangesAsync();

        return UnitResult.Success<string>();
    }
}