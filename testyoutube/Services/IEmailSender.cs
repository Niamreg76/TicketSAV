using System.Threading.Tasks;

namespace testyoutube.Services
{
    public interface IEmailSender
    {
        Task SendMailAsync(string Email, string subject, string message);
    }
}
