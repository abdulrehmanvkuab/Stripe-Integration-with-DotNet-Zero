using Abp.Application.Services.Dto;
using System;

namespace Arch.EmailHandlers.Dtos
{
    public class GetAllEmailHandlersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ContentFilter { get; set; }

        public string EmailAddressFilter { get; set; }

        public int? IsSentFilter { get; set; }

        public int? IsWaitingFilter { get; set; }

        public int? MaxRetryCountFilter { get; set; }
        public int? MinRetryCountFilter { get; set; }

        public long? MaxSentOnFilter { get; set; }
        public long? MinSentOnFilter { get; set; }

        public string SubjectFilter { get; set; }

        public string FileNameFilter { get; set; }

        public string CcComaSeperatedFilter { get; set; }

        public string BccComaSeperatedFilter { get; set; }

        public DateTime? MaxEmailSentDateFilter { get; set; }
        public DateTime? MinEmailSentDateFilter { get; set; }

    }
}