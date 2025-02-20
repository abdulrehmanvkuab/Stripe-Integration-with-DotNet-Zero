using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore;
using Arch.Website.Acme.PhoneBookDemo.PhoneBook;

namespace Arch.Migrations.Seed.Host
{
    namespace Acme.PhoneBookDemo.Migrations.Seed.Host
    {
        public class InitialPeopleCreator
        {
            private readonly ArchDbContext _context;

            public InitialPeopleCreator(ArchDbContext context)
            {
                _context = context;
            }

            public void Create()
            {
                var douglas = _context.Persons.FirstOrDefault(p => p.EmailAddress == "douglas.adams@fortytwo.com");
                if (douglas == null)
                {
                    _context.Persons.Add(
                        new Person
                        {
                            Name = "Douglas",
                            Surname = "Adams",
                            EmailAddress = "douglas.adams@fortytwo.com"
                        });
                }

                var asimov = _context.Persons.FirstOrDefault(p => p.EmailAddress == "isaac.asimov@foundation.org");
                if (asimov == null)
                {
                    _context.Persons.Add(
                        new Person
                        {
                            Name = "Isaac",
                            Surname = "Asimov",
                            EmailAddress = "isaac.asimov@foundation.org"
                        });
                }
            }
        }
    }

}
