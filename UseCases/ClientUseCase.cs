using BookMoney.Services;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using static BookMoney.Pages.RegisterComponent;

namespace BookMoney.UseCases;

public interface IClientUseCase
{
    Task<UnitResult<string>> AddPhoneNumberAsync(string phone);
}

public class ClientUseCase(ILoginService loginService, IConfirmSmsService confirmSmsService, ClientStateService clientStateService) : IClientUseCase // v
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
        await confirmSmsService.CreateAsync(clientId.Value, "12345");

        return UnitResult.Success<string>();
    }
}
