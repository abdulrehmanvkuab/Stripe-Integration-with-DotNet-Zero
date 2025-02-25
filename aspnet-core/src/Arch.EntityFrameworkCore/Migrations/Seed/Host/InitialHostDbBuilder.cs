﻿using Arch.EntityFrameworkCore;
using Arch.Migrations.Seed.Host.Acme.PhoneBookDemo.Migrations.Seed.Host;

namespace Arch.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly ArchDbContext _context;

        public InitialHostDbBuilder(ArchDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new InitialPeopleCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
