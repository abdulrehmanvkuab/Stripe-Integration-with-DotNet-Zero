using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Arch.ApiClient;
using Arch.Mobile.MAUI.Core.ApiClient;

namespace Arch
{
    [DependsOn(typeof(ArchClientModule), typeof(AbpAutoMapperModule))]

    public class ArchMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ArchMobileMAUIModule).GetAssembly());
        }
    }
}