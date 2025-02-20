using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Arch.EmailHandlers.Dtos
{
    public class GetEmailHandlerForEditOutput
    {
        public CreateOrEditEmailHandlerDto EmailHandler { get; set; }

    }
}