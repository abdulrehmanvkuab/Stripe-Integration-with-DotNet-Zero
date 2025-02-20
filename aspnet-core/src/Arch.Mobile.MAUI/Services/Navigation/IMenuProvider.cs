using Arch.Models.NavigationMenu;

namespace Arch.Services.Navigation
{
    public interface IMenuProvider
    {
        List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}