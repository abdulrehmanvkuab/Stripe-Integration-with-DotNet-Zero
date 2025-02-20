using System.Threading.Tasks;

namespace Arch.Net.Emailing
{
    public interface IEmailSettingsChecker
    {
        bool EmailSettingsValid();

        Task<bool> EmailSettingsValidAsync();
    }
}