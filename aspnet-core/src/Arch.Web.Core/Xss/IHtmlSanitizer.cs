using Abp.Dependency;

namespace Arch.Web.Xss
{
    public interface IHtmlSanitizer: ITransientDependency
    {
        string Sanitize(string html);
    }
}