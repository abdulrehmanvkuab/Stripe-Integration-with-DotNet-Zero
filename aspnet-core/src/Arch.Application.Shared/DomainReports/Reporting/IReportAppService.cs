using Abp.Application.Services;
using Arch.Reports;
using System.Threading.Tasks;

namespace Arch.Reporting
{
    public interface IReportAppService : IApplicationService
    {
        string GetDefaultReportSchemaByName(string reportName);

        Task CreateOrUpdateCustomReport(CreateOrEditCustomReportDto input);
    }
}
