using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Arch.EmailHandlers.Dtos;
using Arch.Dto;

namespace Arch.EmailHandlers
{
    public interface IEmailHandlersAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmailHandlerForViewDto>> GetAll(GetAllEmailHandlersInput input);

        Task<GetEmailHandlerForEditOutput> GetEmailHandlerForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditEmailHandlerDto input);

        Task Delete(EntityDto<long> input);

    }
}