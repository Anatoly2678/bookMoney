using BookMoney.Data;
using BookMoney.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMoney.Services;

public interface IClientInfoService
{
    /// <summary>
    /// Создать нового клиента
    /// </summary>
    Task<ClientInfoDBModel> CreateClientAsync(ClientInfoDBModel clientInfo);

    /// <summary>
    /// Проверить существование клиента по email
    /// </summary>
    Task<bool> ClientExistsByEmailAsync(string email);

    /// <summary>
    /// Проверить существование клиента по паспортным данным
    /// </summary>
    Task<bool> ClientExistsByPassportAsync(string passportSeries, string passportNumber);
}

public class ClientInfoService : IClientInfoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ClientInfoService> _logger;

    public ClientInfoService(AppDbContext context, ILogger<ClientInfoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Создать нового клиента
    /// </summary>
    public async Task<ClientInfoDBModel> CreateClientAsync(ClientInfoDBModel clientInfo)
    {
        try
        {
            // Проверка уникальности email
            if (await ClientExistsByEmailAsync(clientInfo.Email))
                throw new InvalidOperationException($"Клиент с email '{clientInfo.Email}' уже существует");

            // Проверка уникальности паспортных данных
            if (await ClientExistsByPassportAsync(clientInfo.PassportSeries, clientInfo.PassportNumber))
                throw new InvalidOperationException($"Клиент с паспортом {clientInfo.PassportSeries} {clientInfo.PassportNumber} уже существует");


            await _context.ClientInfos.AddAsync(clientInfo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Создан новый клиент с ID: {ClientId}", clientInfo.Id);

            return clientInfo;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка при создании клиента в базе данных");
            throw new InvalidOperationException("Ошибка при сохранении данных клиента", ex);
        }
    }

    /// <summary>
    /// Проверить существование клиента по email
    /// </summary>
    public async Task<bool> ClientExistsByEmailAsync(string email)
    {
        return await _context.ClientInfos
            .AsNoTracking()
            .AnyAsync(c => c.Email == email);
    }

    /// <summary>
    /// Проверить существование клиента по паспортным данным
    /// </summary>
    public async Task<bool> ClientExistsByPassportAsync(string passportSeries, string passportNumber)
    {
        return await _context.ClientInfos
            .AsNoTracking()
            .AnyAsync(c => c.PassportSeries == passportSeries && c.PassportNumber == passportNumber);
    }
}
