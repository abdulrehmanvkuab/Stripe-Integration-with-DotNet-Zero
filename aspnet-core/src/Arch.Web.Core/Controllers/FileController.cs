using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Extensions;
using Abp.MimeTypes;
using Microsoft.AspNetCore.Mvc;
using Arch.Dto;
using Arch.Storage;
using System.Threading;

namespace Arch.Web.Controllers
{
    public class FileController : ArchControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IMimeTypeMap _mimeTypeMap;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IMimeTypeMap mimeTypeMap
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _mimeTypeMap = mimeTypeMap;
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            //if (fileBytes == null)
            //{
            //    return NotFound(L("RequestedFileDoesNotExists"));
            //}


            if (fileBytes == null)
            {


                Thread.Sleep(5000);

                fileBytes = _tempFileCacheManager.GetFile(file.FileToken);

                var counter = 0;
                //Keep checking the filebytes is not empty for 2mins before giving an error, just to give the system time to finish up any processes
                while (fileBytes == null && counter++ < 500 && !string.IsNullOrWhiteSpace(file.FileToken))
                {



                    //     _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));
                    Thread.Sleep(2000 + 2000 * (int)(counter / 50)); //increment sleep as you wait
                    fileBytes = _tempFileCacheManager.GetFile(file.FileToken);


                }

                if (fileBytes == null)
                {
                    return NotFound(L("RequestedFileDoesNotExists"));
                }
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadTempFile2(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);


            if (fileBytes == null)
            {


                Thread.Sleep(5000);

                fileBytes = _tempFileCacheManager.GetFile(file.FileToken);

                var counter = 0;
                //Keep checking the filebytes is not empty for 2mins before giving an error, just to give the system time to finish up any processes
                while (fileBytes == null && counter++ < 500 && !string.IsNullOrWhiteSpace(file.FileToken))
                {



                    //     _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));
                    Thread.Sleep(2000 + 2000 * (int)(counter / 50)); //increment sleep as you wait
                    fileBytes = _tempFileCacheManager.GetFile(file.FileToken);


                }

                if (fileBytes == null)
                {
                    return NotFound(L("RequestedFileDoesNotExists"));
                }
            }

            return File(fileBytes, file.FileType, file.FileName);
        }


        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int) HttpStatusCode.NotFound);
            }

            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.Description.IsNullOrEmpty() &&
                    !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
                {
                    fileName = fileObject.Description;
                }
                else
                {
                    return StatusCode((int) HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int) HttpStatusCode.BadRequest);
                }
            }

            return File(fileObject.Bytes, contentType, fileName);
        }
    }
}
