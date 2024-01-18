using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Helper;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace ySite.Core.StaticFiles
{
    public class FilesSettings
    {
        public const string ImagesPath = "/Images";
        public const string DefaultUserImagePath = "/Images/defaultUserImage/Male Image.jpg";
        public const string DefaultFemaleImagePath = "/Images/defaultUserImage/Female image.webp";
        public const string ImageAllowedExtensions = ".jpg,.jpeg,.png,.webp,.gif";
        public const string VideoAllowedExtensions = ".mp4,.mov,.avi,.mkv";
        public const int MaxFileSizeInMB = 5;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
       // public TimeSpan maxDuration = TimeSpan.FromMinutes(240);
        public const int MaxWidthInPX = 2500;
        public const int MaxHightInPX = 2500;
        
        public const int UserMaxWidthInPX = 400;
        public const int UserMaxHightInPX = 400;

        public const int UserMinWidthInPX = 160;
        public const int UserMinHightInPX = 160;
        
        public const int MinWidthInPX = 600;
        public const int MinHightInPX = 315;


        public static ValidationResult AllowUplaod(IFormFile ClientFile)
        {
            if (ClientFile.Length > FilesSettings.MaxFileSizeInBytes)

                return ValidationResult.Fail($"The Uplaoded image exceeds the maximum allowed size of {FilesSettings.MaxFileSizeInBytes / (1024 * 1024)} MB.");

            var allowedExtensions = FilesSettings.ImageAllowedExtensions.Split(',');
            var fileExtension = Path.GetExtension(ClientFile.FileName).ToLower();
            
            //var videoAllowedExtensions = FilesSettings.VideoAllowedExtensions.Split(',');
            //var fileExtension = Path.GetExtension(ClientFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                return ValidationResult.Fail($"Invalid file extension. Allowed extensions are {FilesSettings.ImageAllowedExtensions}.");

            using (var image = Image.Load(ClientFile.OpenReadStream()))
            {
                if (image.Width > MaxWidthInPX || image.Height > MaxHightInPX)
                {
                    return ValidationResult.Fail($"Image dimensions exceed the maximum allowed dimensions of {MaxWidthInPX}x{MaxHightInPX} pixels.");
                }
                if (image.Width < MinWidthInPX || image.Height < MinHightInPX)
                {
                    return ValidationResult.Fail($"Image dimensions exceed the minimum allowed dimensions of {MinWidthInPX}x{MinHightInPX} pixels.");
                }
            }

            //if (allowedExtensions.Contains(fileExtension))
            //{
            //    using (var image = Image.Load(ClientFile.OpenReadStream()))
            //    {
            //        if (image.Width > MaxWidthInPX || image.Height > MaxHightInPX)
            //        {
            //            return ValidationResult.Fail($"Image dimensions exceed the maximum allowed dimensions of {MaxWidthInPX}x{MaxHightInPX} pixels.");
            //        }
            //        if (image.Width < MinWidthInPX || image.Height < MinHightInPX)
            //        {
            //            return ValidationResult.Fail($"Image dimensions exceed the minimum allowed dimensions of {MinWidthInPX}x{MinHightInPX} pixels.");
            //        }
            //    }
            //}
            //else if (videoAllowedExtensions.Contains(fileExtension))
            //{
            //    using (var video = new VideoFile { Filename = ClientFile.FileName })
            //    {
            //        Engine engine = new Engine();

            //        var videoInfo = engine.GetVideoInfo(video);

            //        TimeSpan maxVideoDuration = TimeSpan.FromMinutes(240); // Facebook Pages video duration limit

            //        if (videoInfo.Duration > maxVideoDuration)
            //        {
            //            return ValidationResult.Fail($"The uploaded video exceeds the maximum allowed duration of {maxVideoDuration.TotalMinutes} minutes.");
            //        }
            //    }
            //}
            //else
            //{
            //    return ValidationResult.Fail($"Invalid file extension. Allowed extensions are {FilesSettings.ImageAllowedExtensions} For Images and {FilesSettings.VideoAllowedExtensions} For videos.");
            //}

            return ValidationResult.Success();
        }

        public static ValidationResult UserImageAllowUplaod(IFormFile ClientFile)
        {
            if (ClientFile.Length > FilesSettings.MaxFileSizeInBytes)

                return ValidationResult.Fail($"The Uplaoded image exceeds the maximum allowed size of {FilesSettings.MaxFileSizeInBytes / (1024 * 1024)} MB.");

            var allowedExtensions = FilesSettings.ImageAllowedExtensions.Split(',');
            var fileExtension = Path.GetExtension(ClientFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                return ValidationResult.Fail($"Invalid file extension. Allowed extensions are {FilesSettings.ImageAllowedExtensions}.");

            using (var image = Image.Load(ClientFile.OpenReadStream()))
            {
                if (image.Width > UserMaxWidthInPX || image.Height > UserMaxHightInPX)
                {
                    return ValidationResult.Fail($"Image dimensions exceed the maximum allowed dimensions of {UserMaxWidthInPX}x{UserMaxHightInPX} pixels.");
                }
                if (image.Width < UserMinWidthInPX || image.Height < UserMinHightInPX)
                {
                    return ValidationResult.Fail($"Image dimensions exceed the minimum allowed dimensions of {UserMinWidthInPX}x{UserMinHightInPX} pixels.");
                }
            }

            return ValidationResult.Success();
        }
    }
}
