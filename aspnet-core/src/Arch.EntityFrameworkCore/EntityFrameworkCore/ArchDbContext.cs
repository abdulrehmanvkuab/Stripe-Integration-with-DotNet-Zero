

using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Arch.Authorization.Delegation;
using Arch.Authorization.Roles;
using Arch.Authorization.Users;
using Arch.Chat;
using Arch.Editions;
using Arch.Friendships;
using Arch.MultiTenancy;
using Arch.MultiTenancy.Accounting;
using Arch.MultiTenancy.Payments;
using Arch.OpenIddict.Applications;
using Arch.OpenIddict.Authorizations;
using Arch.OpenIddict.Scopes;
using Arch.OpenIddict.Tokens;
using Arch.Storage;
using Arch.Authorization;


using Arch.ConfigurationSetting;

using Arch.TenantInfos;

using Arch.TanentDemographicInfos;
using Arch.EmailHandlers;

using Arch.ExtraProperties;
using System.Text.Json;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using NsJson = Newtonsoft.Json;
using Arch.Website.Acme.PhoneBookDemo.PhoneBook;
using Arch.Merchants;

namespace Arch.EntityFrameworkCore
{
    public class ArchDbContext : AbpZeroDbContext<Tenant, Role, User, ArchDbContext>, IOpenIddictDbContext
    {

        public DbSet<Merchant> Merchants { get; set; }

        public virtual DbSet<EmailHandler> EmailHandlers { get; set; }

        public virtual DbSet<TanentDemographicInfo> TanentDemographicInfos { get; set; }

        public virtual DbSet<TenantInfo> TenantInfos { get; set; }

       

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<OpenIddictApplication> Applications { get; }

        public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

        public virtual DbSet<OpenIddictScope> Scopes { get; }

        public virtual DbSet<OpenIddictToken> Tokens { get; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        // 

        public virtual DbSet<Person> Persons { get; set; }


        public ArchDbContext(DbContextOptions<ArchDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            var listOfLongConverter = new ValueConverter<List<long>, string>(
                v => NsJson.JsonConvert.SerializeObject(v),
                v => NsJson.JsonConvert.DeserializeObject<List<long>>(v)
            );

            var listOfStringConverter = new ValueConverter<List<string>, string>(
                v => NsJson.JsonConvert.SerializeObject(v),
                v => NsJson.JsonConvert.DeserializeObject<List<string>>(v)
            );




            modelBuilder.Entity<SubscriptionPayment>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });



            modelBuilder.Entity<EmailHandler>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });

            });



            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigureOpenIddict();
        }
    }
}