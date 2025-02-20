using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Arch.Authorization;
using Arch.Mechants.Dto;
using Arch.Mechants;
using Arch.Merchants;

namespace Arch
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(ArchApplicationSharedModule),
        typeof(ArchCoreModule)
        )]
    public class ArchApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);


            Configuration.Modules.AbpAutoMapper().Configurators.Add(
    cfg => cfg.CreateMap<Merchant, MerchantDto>()
);
            IocManager.Register<IMerchantAppService, MerchantAppService>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ArchApplicationModule).GetAssembly());
        }
    }
}