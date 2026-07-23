
namespace Application.Features.Email.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmail(String Email);
}
