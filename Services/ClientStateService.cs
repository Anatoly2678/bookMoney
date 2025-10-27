namespace BookMoney.Services;

public class ClientStateService
{
    public Guid? CurrentClientId { get; set; }
    public event Action OnStateChanged;

    public void SetClientId(Guid clientId)
    {
        CurrentClientId = clientId;
        NotifyStateChanged();
    }

    public void ClearClientId()
    {
        CurrentClientId = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
