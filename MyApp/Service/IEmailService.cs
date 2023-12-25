using MyApp.Models;

namespace MyApp.Service
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(UserEmailOptions userEmailOptions);
        Task SendForgetEmail(UserEmailOptions userEmailOptions);
    }
}