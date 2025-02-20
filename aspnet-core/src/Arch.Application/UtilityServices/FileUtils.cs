using Abp.Runtime.Session;
using Abp.UI;

using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Abp.Application.Services.Dto;

namespace Arch.UtilityServices
{
    public class FileUtils: ArchAppServiceBase {
        public FileUtils()
        {

        }

        public static async Task<string> FileUpload(IFormFile file, FilePathEnum filePathEnum, String tenantId = "0")
        {
            string fileName;
            string filePath;

            try
            {
                var extension = System.IO.Path.GetExtension(file.FileName);
                fileName = DateTime.Now.Ticks + extension;

               


             //   var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\\Tenant"+ tenantId+ @"\\" + filePathEnum);
                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\\" + filePathEnum);

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                filePath = Path.Combine(pathBuilt, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch
            {
                try {
                    fileName = await FileUploadBase64(file.ToString(), filePathEnum);
                }
                catch (Exception ex) { 
                    throw new UserFriendlyException("Can't Upload File");
                }
            }
            return fileName;
        }

        public static async Task<string> FileUploadBase64(string imageBase64, FilePathEnum filePathEnum)
        {
            string imageName;
            string extension;
            string img64;
            try
            {
                var base64Arr = imageBase64.Split(';');

                extension = "." + base64Arr[0].Split('/')[1];

                img64 = base64Arr[1].Split(',')[1];
            }
            catch (Exception) {

                //For Order signature files
                extension = "png";
                img64 = imageBase64;

            }
            

            byte[] bytes = Convert.FromBase64String(img64);
            string DefaultImagePath = "wwwroot\\"+ filePathEnum;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), DefaultImagePath);

            bool exists = System.IO.Directory.Exists(filePath);

            try {
                if (!exists) System.IO.Directory.CreateDirectory(filePath);
            }
            catch (IOException ioex) {
                Console.WriteLine(ioex.Message);
            }

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Image pic = Image.FromStream(ms);

                long unixMilliseconds = TimeUtils.ConvertMilliseconds(DateTime.Now);

                imageName = "image-" + unixMilliseconds + extension;

                pic.Save(DefaultImagePath + "/" + imageName);

            }
            return imageName;
        }

        public static async Task<string> ImageToBase64(string imageName)
        {
            string base64String = null;
            try
            {
                Bitmap image = new Bitmap(imageName);
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                    base64String = "\"data:image/png;base64," + base64String;
                }
            }
            catch (Exception)
            {
            }
            return base64String;
        }

        public enum FilePathEnum
        {
            ActivityAttachments,
            ProductImages,
            BackOfficeUserImages,
            ExpenseImages,
            QuestionAttachments,
            CategoryImages,
            SignatureFiles,
            TeamMemberImages,
            ImageAnswer,
            CompanyLogo,
            Performances,
            Leagues,
            Achievements
        }
    }
}
