using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Helper;
using ySite.Core.StaticFiles;
using ySite.Service.Interfaces;

namespace ySite.Service.Services
{
    public class StaticService : IStaticService
    {

        public ValidationResult AllowUplaod(IFormFile ClientFile)
        {
            if (ClientFile.Length > FilesSettings.MaxFileSizeInBytes)

                return ValidationResult.Fail($"The Uplaoded image exceeds the maximum allowed size of {FilesSettings.MaxFileSizeInBytes / (1024 * 1024)} MB.");

            var allowedExtensions = FilesSettings.AllowedExtensions.Split(',');
            var fileExtension = Path.GetExtension(ClientFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                return ValidationResult.Fail($"Invalid file extension. Allowed extensions are {FilesSettings.AllowedExtensions}.");

            return ValidationResult.Success();
        }
    }
}
