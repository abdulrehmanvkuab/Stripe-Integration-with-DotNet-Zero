using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Arch.EmailHandlers.Dtos
{
    public class CreateOrEditEmailHandlerDto : AuditedEntityDto<long?>
    {

        public string Content { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public bool IsSent { get; set; }

        public bool IsWaiting { get; set; }

        public int? RetryCount { get; set; }

        public long? SentOn { get; set; }

        public string Subject { get; set; }

        public string FileName { get; set; }

        public string CcComaSeperated { get; set; }

        public string BccComaSeperated { get; set; }

        public DateTime? EmailSentDate { get; set; }

    }
}