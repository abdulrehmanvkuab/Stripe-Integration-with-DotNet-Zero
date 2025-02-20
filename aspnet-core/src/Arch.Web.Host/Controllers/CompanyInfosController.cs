using System;
using System.IO;
using System.Linq;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Arch.Storage;

namespace Arch.Web.Controllers
{
    [Authorize]
    public class CompanyInfosController : ArchControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxCompanyLogoLength = 5242880; //5MB
        private const string MaxCompanyLogoLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] CompanyLogoAllowedFileTypes = { "jpeg", "jpg", "png" };

        public CompanyInfosController(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public FileUploadCacheOutput UploadCompanyLogoFile()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > MaxCompanyLogoLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxCompanyLogoLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (CompanyLogoAllowedFileTypes != null && CompanyLogoAllowedFileTypes.Length > 0 && !CompanyLogoAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", CompanyLogoAllowedFileTypes));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                return new FileUploadCacheOutput(fileToken);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

        public string[] GetCompanyLogoFileAllowedTypes()
        {
            return CompanyLogoAllowedFileTypes;
        }

    }
}