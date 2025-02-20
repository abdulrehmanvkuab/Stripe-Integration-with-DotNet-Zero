using Abp.AspNetCore.Mvc.Authorization;
using Arch.Authorization.Users.Profile;
using Arch.Graphics;
using Arch.Storage;

namespace Arch.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}