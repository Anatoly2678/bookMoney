using BookMoney.Data;
using BookMoney.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;

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
            SmsCode = ConfirmSmsServiceExtensions.GenerateFourNumbers(),
            //DateCreate = DateTime.UtcNow
        };

        try
        {
            await context.ConfirmSms.AddAsync(sms);
            await context.SaveChangesAsync();
        }
        catch (Exception err)
        {
            var stop = err;
        }
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

file static class ConfirmSmsServiceExtensions
{
    public static string GenerateFourNumbers()
    {
        Random random = new Random();
        char[] digits = new char[4];

        for (int i = 0; i < 4; i++)
        {
            digits[i] = (char)('0' + random.Next(0, 10));
        }

        return new string(digits);
    }
}