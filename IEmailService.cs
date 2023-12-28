namespace MusicLibraryApp
{
    public interface IEmailService
    {
    
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailAsyncWithQueue(string email, string subject, string message);


    }
}
