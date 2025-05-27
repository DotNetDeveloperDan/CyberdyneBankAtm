namespace CyberdyneBankAtmClient.Services
{
    public class NotificationService
    {
        public event Action<string, string, int> OnNotify;

        public void Show(string message, string css = "alert-danger")
        {
            OnNotify?.Invoke(message, css, 3000);
        }
    }

}
