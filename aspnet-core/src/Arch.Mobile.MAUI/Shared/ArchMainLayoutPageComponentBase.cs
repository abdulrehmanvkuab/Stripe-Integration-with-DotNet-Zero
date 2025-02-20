using Arch.Core.Dependency;
using Arch.Mobile.MAUI.Services.UI;

namespace Arch.Mobile.MAUI.Shared
{
    public class ArchMainLayoutPageComponentBase : ArchComponentBase
    {
        protected PageHeaderService PageHeaderService { get; set; }

        protected DomManipulatorService DomManipulatorService { get; set; }

        public ArchMainLayoutPageComponentBase()
        {
            PageHeaderService = DependencyResolver.Resolve<PageHeaderService>();
            DomManipulatorService = DependencyResolver.Resolve<DomManipulatorService>();
        }

        protected async Task SetPageHeader(string title)
        {
            PageHeaderService.Title = title;
            PageHeaderService.ClearButton();
            await DomManipulatorService.ClearModalBackdrop(JS);
        }

        protected async Task SetPageHeader(string title, List<PageHeaderButton> buttons)
        {
            PageHeaderService.Title = title;
            PageHeaderService.SetButtons(buttons);
            await DomManipulatorService.ClearModalBackdrop(JS);
        }
    }
}
