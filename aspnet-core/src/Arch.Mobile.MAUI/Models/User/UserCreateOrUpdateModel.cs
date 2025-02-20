using Abp.AutoMapper;
using Arch.Authorization.Users.Dto;

namespace Arch.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
