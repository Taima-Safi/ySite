using MediaToolkit;
using VideoLibrary;
using MediaToolkit.Options;
using Microsoft.AspNetCore.Http;
using ySite.Core.Helper;
using MediaToolkit.Model;
using System;
using System.IO;
using Image = SixLabors.ImageSharp.Image;

namespace ySite.Core.StaticFiles
{
    public class FilesSettings
    {
        public const string ImagesPath = "/Images";
        public const string VideosPath = "/Videos";
        public const string DefaultUserImagePath = "/Images/defaultUserImage/Male Image.jpg";
        public const string DefaultFemaleImagePath = "/Images/defaultUserImage/Female image.webp";
        public const string AllowedImageExtensions = ".jpg,.jpeg,.png,.webp,.gif,.bmp";
        public const string AllowedVideoExtensions = ".mp4,.mov,.avi,.mkv,.wmv";
        public const int MaxFileSizeInMB = 5;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
        public const int MaxVideoSizeInBytes = 50 * 1024 * 1024;

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
            
            var allowedImageExtensions = FilesSettings.AllowedImageExtensions.Split(',');
            var allowedVideoExtensions = FilesSettings.AllowedVideoExtensions.Split(',');
            var fileExtension = Path.GetExtension(ClientFile.FileName).ToLower();

            if (allowedImageExtensions.Contains(fileExtension))
            {
                if (ClientFile.Length > FilesSettings.MaxFileSizeInBytes)

                    return ValidationResult.Fail($"The Uplaoded image exceeds the maximum allowed size of {FilesSettings.MaxFileSizeInBytes / (1024 * 1024)} MB.");

                using (var image = Image.Load(ClientFile.OpenReadStream()))
                {
                    if (image.Width > MaxWidthInPX || image.Height >  MaxHightInPX)
                    {
                        return ValidationResult.Fail($"Image dimensions exceed the maximum allowed dimensions of {MaxWidthInPX}x{MaxHightInPX} pixels.");
                    }
                    if (image.Width < MinWidthInPX || image.Height < MinHightInPX)
                    {
                        return ValidationResult.Fail($"Image dimensions exceed the minimum allowed dimensions of {MinWidthInPX}x{MinHightInPX} pixels.");
                    }
                }
                return ValidationResult.Success("Image");
            }
            else if (allowedVideoExtensions.Contains(fileExtension))
            {
                if (ClientFile.Length > FilesSettings.MaxVideoSizeInBytes)
                    return ValidationResult.Fail($"The uploaded video exceeds the maximum allowed size of {FilesSettings.MaxVideoSizeInBytes / (1024 * 1024)} MB.");

                //using (var video = new VideoFile { Filename = ClientFile.FileName })
                //{
                //var inputFile = new MediaFile { Filename = ClientFile.FileName };
                //using (var engine = new Engine())
                //{
                //    engine.GetMetadata(inputFile);

                //    if (inputFile.Metadata.Duration > TimeSpan.FromMinutes(240)) // Facebook Pages video duration limit
                //    {
                //        return ValidationResult.Fail($"The uploaded video exceeds the maximum allowed duration of 240 minutes.");
                //    }
                //}

                //}
                return ValidationResult.Success("video");
            }
            return ValidationResult.Fail($"Invalid file extension. Allowed extensions are {FilesSettings.AllowedImageExtensions} For Images and {FilesSettings.AllowedVideoExtensions} For videos.");
        }



        public static ValidationResult UserImageAllowUplaod(IFormFile ClientFile)
        {
            if (ClientFile.Length > FilesSettings.MaxFileSizeInBytes)

                return ValidationResult.Fail($"The Uplaoded image exceeds the maximum allowed size of {FilesSettings.MaxFileSizeInBytes / (1024 * 1024)} MB.");

            var allowedExtensions = FilesSettings.AllowedImageExtensions.Split(',');
            var fileExtension = Path.GetExtension(ClientFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                return ValidationResult.Fail($"Invalid file extension. Allowed extensions are {FilesSettings.AllowedImageExtensions}.");

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

            return ValidationResult.Success("video");
        }
    }
}
