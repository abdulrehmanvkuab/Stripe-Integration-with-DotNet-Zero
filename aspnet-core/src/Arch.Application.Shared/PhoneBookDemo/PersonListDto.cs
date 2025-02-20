using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Arch.PhoneBookDemo
{
    public class GetPeopleInput
    {
        public string Filter { get; set; }
    }

    public class PersonListDto : FullAuditedEntityDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }
    }

}
