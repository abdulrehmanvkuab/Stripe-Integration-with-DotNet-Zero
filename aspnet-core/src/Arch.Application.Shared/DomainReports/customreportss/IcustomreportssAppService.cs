using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Arch.CustomReportss.Dtos;
using Arch.Dto;

namespace Arch.CustomReportss
{
    public interface ICustomReportssAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomReportsForViewDto>> GetAll(GetAllCustomReportssInput input);

        Task<GetCustomReportsForViewDto> GetCustomReportsForView(long id);

        //	Task<GetCustomReportsForEditOutput> GetCustomReportsForEdit(EntityDto<long> input);

        Task<GetCustomReportsForEditOutput> GetCustomReportsForEdit(long? id);

        Task CreateOrEditCustomReports(CreateOrEditCustomReportsDto input);

        Task<CreateOrEditCustomReportsDto> CreateOrEdit(CreateOrEditCustomReportsDto input);

        Task DeleteFinal(EntityDto<long> input);

       

        Task<CreateOrEditCustomReportsDto> Delete(ProcessActionDto input);

        Task<object> ProcessActionsExec(ProcessActionDto input);

        Task DeleteCustomReports(ProcessActionDto input);

        Task<Boolean> GetPermission(string subaccess, string baseobject, string mainmodule_processheader);

        Task<CreateOrEditCustomReportsDto> GetCustomReportsForCreate();

        Task<FileDto> GetCustomReportssToExcel(GetAllCustomReportssForExcelInput input);

        /*

        */
    }
}