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
    public class ExpensesController : ArchControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxAttachmentLength = 5242880; //5MB
        private const string MaxAttachmentLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] AttachmentAllowedFileTypes = { "jpeg", "jpg", "png" };

        public ExpensesController(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public FileUploadCacheOutput UploadAttachmentFile()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > MaxAttachmentLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxAttachmentLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (AttachmentAllowedFileTypes != null && AttachmentAllowedFileTypes.Length > 0 && !AttachmentAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", AttachmentAllowedFileTypes));
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

        public string[] GetAttachmentFileAllowedTypes()
        {
            return AttachmentAllowedFileTypes;
        }

    }
}