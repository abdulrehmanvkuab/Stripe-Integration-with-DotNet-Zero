using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Arch.Auditing.Dto;
using Arch.Auditing.Exporting;
using Arch.Authorization;
using Arch.Authorization.Users;
using Arch.Dto;
using Arch.EntityHistory;
using Abp.Application.Services;
using Arch.Website.Acme.PhoneBookDemo.PhoneBook;
using AutoMapper.Internal.Mappers;




namespace Arch.PhoneBookDemo
{
    public interface IPersonAppService : IApplicationService
    {
        ListResultDto<PersonListDto> GetPeople(GetPeopleInput input);
    }
    public class PersonAppService : ArchAppServiceBase, IPersonAppService
    {
        private readonly IRepository<Person> _personRepository;

        public PersonAppService(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public ListResultDto<PersonListDto> GetPeople(GetPeopleInput input)
        {
            var people = _personRepository
                .GetAll()
                .WhereIf(
                    !input.Filter.IsNullOrEmpty(),
                    p => p.Name.Contains(input.Filter) ||
                         p.Surname.Contains(input.Filter) ||
                         p.EmailAddress.Contains(input.Filter)
                )
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Surname)
                .ToList();

            return new ListResultDto<PersonListDto>(ObjectMapper.Map<List<PersonListDto>>(people));
        }
    }


}
