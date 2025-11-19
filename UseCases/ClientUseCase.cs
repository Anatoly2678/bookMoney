using BookMoney.Services;
using CSharpFunctionalExtensions;

namespace BookMoney.UseCases;

public interface IClientUseCase
{
    Task<UnitResult<string>> AddPhoneNumberAsync(string phone);
}

public class ClientUseCase(ILoginService loginService, IConfirmSmsService confirmSmsService, IClientInfoService clientInfo, ClientStateService clientStateService) : IClientUseCase
{
    public async Task<UnitResult<string>> AddPhoneNumberAsync(string phone)
    {
        var clientId = await loginService.GetIdByPhoneAsync(phone);

        if (clientId.HasNoValue)
        {
            var result = await loginService.AddLoginAsync(phone);
            if (result.HasValue)
                clientId = result.Value;
        }

        clientStateService.SetClientId(clientId.Value);
        await confirmSmsService.CreateAsync(clientId.Value, ConfirmSmsServiceExtensions.GenerateFourNumbers());

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