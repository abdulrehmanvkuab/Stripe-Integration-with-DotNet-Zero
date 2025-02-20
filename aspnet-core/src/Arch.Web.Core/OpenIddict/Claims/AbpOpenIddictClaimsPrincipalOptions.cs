using Abp.Collections;

namespace Arch.Web.OpenIddict.Claims;

public class AbpOpenIddictClaimsPrincipalOptions
{
    public ITypeList<IAbpOpenIddictClaimsPrincipalHandler> ClaimsPrincipalHandlers { get; }

    public AbpOpenIddictClaimsPrincipalOptions()
    {
        ClaimsPrincipalHandlers = new TypeList<IAbpOpenIddictClaimsPrincipalHandler>();
    }
}