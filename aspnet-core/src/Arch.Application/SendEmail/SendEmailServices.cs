using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Arch.Authorization.Users;
using Arch.Configuration;
using Arch.Editions;
using Arch.MultiTenancy;
using Arch.Net.Emailing;
using Arch.Url;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Arch.SendEmail
{
    [Authorize]
    public class SendEmailServices : ArchAppServiceBase //, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Tenant> _tenantRepository;
        //private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly ISettingManager _settingManager;
        //private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;
        private readonly IAppConfigurationAccessor _configurationAccessor;
        // used for styling action links on email messages.
        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";
        protected readonly IWebUrlService _WebUrlService;
        public SendEmailServices(
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IRepository<Tenant> tenantRepository,
            //ICurrentUnitOfWorkProvider unitOfWorkProvider,
            //IUnitOfWorkManager unitOfWorkManager,
            //ISettingManager settingManager,
            //EditionManager editionManager,
            UserManager userManager,
            IAbpSession abpSession,
            IWebUrlService WebUrlService,
              IAppConfigurationAccessor configurationAccessor
            )
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _tenantRepository = tenantRepository;
            //_unitOfWorkProvider = unitOfWorkProvider;
            //_unitOfWorkManager = unitOfWorkManager;
            //_settingManager = settingManager;
            //_editionManager = editionManager;
            _userManager = userManager;
            _abpSession = abpSession;
            _WebUrlService = WebUrlService;
            _configurationAccessor = configurationAccessor;
        }
        private string _emailButtonColor = "#00bb77";

        public  async Task<string> SendEmail([FromForm] SendEmailRequestModel request)
        {

         //   var sendnow =  SendEmailViaWebApi(request);


            await  SendEmailViaWebApi(request);

            return "Mail Sent Successfully";

        }

        private  async Task SendEmailViaWebApiold(SendEmailRequestModel request)
        {
            //Using MimeMessage
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse("wnmtest6@gmail.com");
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;



          
            //Create Html For Email Body
            var builder = new BodyBuilder();

            string s = request.Body;

            builder.HtmlBody = s;

            email.Body = builder.ToMessageBody();

            //Send Attachments With Email... 
            if (!request.File.IsNullOrEmpty())
            {
                byte[] fileBytes;
                foreach (var file in request.File)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms)
;
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                smtp.Connect("smtp.gmail.com", 587);
                smtp.Authenticate("wnmtest6@gmail.com", "aobkwrwzpeodwyfb");

                await smtp.SendAsync(email);
            }
            catch (Exception)
            {

                throw new Exception("Email not Sent");
            }

        }


        public  async Task  SendEmailHtml(string emailAddress, string subject, 
            StringBuilder mailMessage,string title = "", string subTitle = "", List<IFormFile> files = null)
        {
            var tenantId = AbpSession.TenantId;
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            string tenancyname = null;
            var server = _WebUrlService.GetServerRootAddress();
         emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString()).Replace("{ServerRootAddress}", server);

            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            var email = new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true

            };
            


            if (!files.IsNullOrEmpty())
            {
                byte[] fileBytes;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var ms = new MemoryStream();
                        
;
                           
                        var attachment = new Attachment( ms, file.FileName, ContentType.Parse(file.ContentType).MediaType);

                        email.Attachments.Add(attachment);
                    }
                }

            }

            await SendEmailwithRetry(email);

        }



        public  async Task SendEmailViaWebApi(SendEmailRequestModel request)
        {

            //        public override string WebSiteRootAddressFormatKey => "App:ClientRootAddress";

            //public override string ServerRootAddressFormatKey => "App:ServerRootAddress";

            var server = _WebUrlService.GetServerRootAddress(); //_configurationAccessor.Configuration["App:ServerRootAddress"].ToString();

            var tenantId = AbpSession.TenantId;
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            string tenancyname = null;
        
            emailTemplate.Replace("{EMAIL_BODY}", request.Body.ToString());
            emailTemplate.Replace("{ServerRootAddress}", server);
            emailTemplate.Replace("{EMAIL_TITLE}", request.Title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", request.SubTitle);

          
            var email = new MailMessage
            {
                To = { request.ToEmail },
                Subject = request.Subject,
                Body = request.Body.ToString(),
                IsBodyHtml = true

            };



            if (!request.File.IsNullOrEmpty())
            {
                byte[] fileBytes;
                foreach (var file in request.File)
                {
                    if (file.Length > 0)
                    {
                        var ms = new MemoryStream();

                        ;

                        var attachment = new Attachment(ms, file.FileName, ContentType.Parse(file.ContentType).MediaType);

                        email.Attachments.Add(attachment);
                    }
                }

            }

            await SendEmailwithRetry(email);

        }

        private async Task SendEmailwithRetry(MailMessage email, int retry = 5)
        {
            
           int count = 0;

               // intentionally run a thread and not wait for it.


                   Task.Run(async () =>
                 {
                     while (count++ < retry)
                     {
                         try
                         {
                             await _emailSender.SendAsync(email).ContinueWith(task => {
                                 count = retry;
                                 }) ;
                             return;
                             
                         }
                         catch (Exception e)
                         {
                             
                         }
                     }
        });

            return;


        }

    }
}
