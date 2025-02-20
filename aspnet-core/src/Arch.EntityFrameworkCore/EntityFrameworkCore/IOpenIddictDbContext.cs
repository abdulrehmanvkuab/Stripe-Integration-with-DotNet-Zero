using Microsoft.EntityFrameworkCore;
using Arch.OpenIddict.Applications;
using Arch.OpenIddict.Authorizations;
using Arch.OpenIddict.Scopes;
using Arch.OpenIddict.Tokens;

namespace Arch.EntityFrameworkCore
{
    public interface IOpenIddictDbContext
    {
        DbSet<OpenIddictApplication> Applications { get; }

        DbSet<OpenIddictAuthorization> Authorizations { get; }

        DbSet<OpenIddictScope> Scopes { get; }

        DbSet<OpenIddictToken> Tokens { get; }
    }

}