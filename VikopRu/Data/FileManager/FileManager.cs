using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Data.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly string _profilePicturePath;
        private readonly string _findingPicturePath;

        public FileManager(IConfiguration configuration)
        {
            _profilePicturePath = configuration["Path:ProfilePictures"];
            _findingPicturePath = configuration["Path:FindingImages"];
        }

        public FileStream FindingPictureStream(string image)
            => new FileStream(Path.Combine(_findingPicturePath, image), FileMode.Open, FileAccess.Read);

        public FileStream ProfilePictureStream(string image)
            => new FileStream(Path.Combine(_profilePicturePath, image), FileMode.Open, FileAccess.Read);

        private async Task<string> SaveImage(IFormFile image, string savePath)
        {
            try
            {
                if (!Directory.Exists(_findingPicturePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var fileName = $"img_{Guid.NewGuid()}{mime}";

                using (var stream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }
        }

        public async Task<string> SaveFindingPicture(IFormFile image) => await SaveImage(image, _findingPicturePath);
        public async Task<string> SaveProfilePicture(IFormFile image) => await SaveImage(image, _profilePicturePath);
    }
}
