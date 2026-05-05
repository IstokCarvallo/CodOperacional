namespace APISegura.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordReset(string email, string token);
    }
}
