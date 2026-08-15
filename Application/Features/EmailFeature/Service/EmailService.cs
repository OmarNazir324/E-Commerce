
using Application.Features.EmailFeature.Interfaces;

namespace Application.Features.Email.Service;

public class EmailService:IEmailService
{
    public Task SendWelcomeEmail(String Email)
    {
        return Task.CompletedTask;
    }
}
