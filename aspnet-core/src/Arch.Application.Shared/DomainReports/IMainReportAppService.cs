using Abp.Application.Services;
using Arch.Domain.MainReports.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arch.Domain.MainReports
{
    public interface IMainReportAppService : IApplicationService
    {
       
        Task<object> MainReportAsync<T>(T objectId, string objectName);

        Task<List<Dictionary<string, object>>> MainReportDictionaryAsync(string objectName);
        Task<List<object>> MainReportObjectAsync(string objectName);
        Task<List<Dictionary<string, object>>> RunReportDictionaryAsync(string objectName, List<Dictionary<string, string>> paramlist);
        Task<List<object>> RunReportObjectAsync(string objectName, List<Dictionary<string, string>> paramlist);
        Task<List<Dictionary<string, object>>> RunMainReportObjectAsync(string reportName, List<MainReportDto> paramlist);
        Task<List<MainReportDto>> GetMainReportEntityAsync(string reportName);

        

        Task<object> RunDefinedReportObjectAsync(string reportName, object selectedparameters, object paramtype = null);
        Task<object> RunDefinedReportAsync(string reportName, object selectedparameters, string paramtype = "financialreport");
    }
}


