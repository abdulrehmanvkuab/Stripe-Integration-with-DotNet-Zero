using System.Threading.Tasks;
using Abp.Domain.Uow;

namespace Arch.OpenIddict
{
    public interface IOpenIddictDbConcurrencyExceptionHandler
    {
        Task HandleAsync(AbpDbConcurrencyException exception);
    }
}