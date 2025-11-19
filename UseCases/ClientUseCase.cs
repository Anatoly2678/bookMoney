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
        await confirmSmsService.CreateAsync(clientId.Value, "12345");

        return UnitResult.Success<string>();
    }
}
