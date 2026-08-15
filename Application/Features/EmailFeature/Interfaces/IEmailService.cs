
namespace Application.Features.EmailFeature.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmail(String Email);
}
