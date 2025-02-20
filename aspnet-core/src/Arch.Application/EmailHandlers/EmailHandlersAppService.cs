using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Arch.EmailHandlers.Dtos;
using Arch.Dto;
using Abp.Application.Services.Dto;
using Arch.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Arch.Storage;

namespace Arch.EmailHandlers
{
    [AbpAuthorize(AppPermissions.Pages_EmailHandlers)]
    public class EmailHandlersAppService : ArchAppServiceBase, IEmailHandlersAppService
    {
        private readonly IRepository<EmailHandler, long> _emailHandlerRepository;

        public EmailHandlersAppService(IRepository<EmailHandler, long> emailHandlerRepository)
        {
            _emailHandlerRepository = emailHandlerRepository;

        }

        public async Task<PagedResultDto<GetEmailHandlerForViewDto>> GetAll(GetAllEmailHandlersInput input)
        {

            var filteredEmailHandlers = _emailHandlerRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Content.Contains(input.Filter) || e.EmailAddress.Contains(input.Filter) || e.Subject.Contains(input.Filter) || e.FileName.Contains(input.Filter) || e.CcComaSeperated.Contains(input.Filter) || e.BccComaSeperated.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content == input.ContentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailAddressFilter), e => e.EmailAddress == input.EmailAddressFilter)
                        .WhereIf(input.IsSentFilter.HasValue && input.IsSentFilter > -1, e => (input.IsSentFilter == 1 && e.IsSent) || (input.IsSentFilter == 0 && !e.IsSent))
                        .WhereIf(input.IsWaitingFilter.HasValue && input.IsWaitingFilter > -1, e => (input.IsWaitingFilter == 1 && e.IsWaiting) || (input.IsWaitingFilter == 0 && !e.IsWaiting))
                        .WhereIf(input.MinRetryCountFilter != null, e => e.RetryCount >= input.MinRetryCountFilter)
                        .WhereIf(input.MaxRetryCountFilter != null, e => e.RetryCount <= input.MaxRetryCountFilter)
                        .WhereIf(input.MinSentOnFilter != null, e => e.SentOn >= input.MinSentOnFilter)
                        .WhereIf(input.MaxSentOnFilter != null, e => e.SentOn <= input.MaxSentOnFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter), e => e.Subject == input.SubjectFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName == input.FileNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CcComaSeperatedFilter), e => e.CcComaSeperated == input.CcComaSeperatedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BccComaSeperatedFilter), e => e.BccComaSeperated == input.BccComaSeperatedFilter)
                        .WhereIf(input.MinEmailSentDateFilter != null, e => e.EmailSentDate >= input.MinEmailSentDateFilter)
                        .WhereIf(input.MaxEmailSentDateFilter != null, e => e.EmailSentDate <= input.MaxEmailSentDateFilter);

            var pagedAndFilteredEmailHandlers = filteredEmailHandlers
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var emailHandlers = from o in pagedAndFilteredEmailHandlers
                                select new
                                {

                                    o.Content,
                                    o.EmailAddress,
                                    o.IsSent,
                                    o.IsWaiting,
                                    o.RetryCount,
                                    o.SentOn,
                                    o.Subject,
                                    o.FileName,
                                    o.CcComaSeperated,
                                    o.BccComaSeperated,
                                    o.EmailSentDate,
                                    Id = o.Id
                                };

            var totalCount = await filteredEmailHandlers.CountAsync();

            var dbList = await emailHandlers.ToListAsync();
            var results = new List<GetEmailHandlerForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetEmailHandlerForViewDto()
                {
                    EmailHandler = new EmailHandlerDto
                    {

                        Content = o.Content,
                        EmailAddress = o.EmailAddress,
                        IsSent = o.IsSent,
                        IsWaiting = o.IsWaiting,
                        RetryCount = o.RetryCount,
                        SentOn = o.SentOn,
                        Subject = o.Subject,
                        FileName = o.FileName,
                        CcComaSeperated = o.CcComaSeperated,
                        BccComaSeperated = o.BccComaSeperated,
                        EmailSentDate = o.EmailSentDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmailHandlerForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_EmailHandlers_Edit)]
        public async Task<GetEmailHandlerForEditOutput> GetEmailHandlerForEdit(EntityDto<long> input)
        {
            var emailHandler = await _emailHandlerRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmailHandlerForEditOutput { EmailHandler = ObjectMapper.Map<CreateOrEditEmailHandlerDto>(emailHandler) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmailHandlerDto input)
        {
            if (input.Id == null)
            {
                input.CreationTime = UtilityServices.TimeUtils.Now();
                await Create(input);
            }
            else
            {
                input.LastModificationTime = UtilityServices.TimeUtils.Now();
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_EmailHandlers_Create)]
        protected virtual async Task Create(CreateOrEditEmailHandlerDto input)
        {
            var emailHandler = ObjectMapper.Map<EmailHandler>(input);

            if (AbpSession.TenantId != null)
            {
                emailHandler.TenantId = (int?)AbpSession.TenantId;
            }

            await _emailHandlerRepository.InsertAsync(emailHandler);

        }

        [AbpAuthorize(AppPermissions.Pages_EmailHandlers_Edit)]
        protected virtual async Task Update(CreateOrEditEmailHandlerDto input)
        {
            var emailHandler = await _emailHandlerRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, emailHandler);

        }

        [AbpAuthorize(AppPermissions.Pages_EmailHandlers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _emailHandlerRepository.DeleteAsync(input.Id);
        }

    }
}