using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.StaticFiles
{
    public class FilesSettings
    {
        public const string ImagesPath = "/Images";
        public const string DefaultUserImagePath = "/Images/defaultUserImage/DefaultUserImage.png";
        public const string AllowedExtensions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMB = 5;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;


        public string AllowUplaod(IFormFile file)
        {
            if(file.Length > MaxFileSizeInBytes)
            return $"The profile image exceeds the maximum allowed size of {MaxFileSizeInBytes / (1024 * 1024)} MB.";

            var allowedExtensions = AllowedExtensions.Split(',');
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return $"Invalid file extension. Allowed extensions are {FilesSettings.AllowedExtensions}.";
            }
                return "Success";
        }
    }
}
