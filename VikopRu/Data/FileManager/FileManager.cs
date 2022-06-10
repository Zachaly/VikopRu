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
            _findingPicturePath = configuration["Path:FindingPictures"];
        }

        public FileStream FindingPictureStream(string image)
            => new FileStream(Path.Combine(_findingPicturePath, image), FileMode.Open, FileAccess.Read);

        public FileStream ProfilePictureStream(string image)
            => new FileStream(Path.Combine(_profilePicturePath, image), FileMode.Open, FileAccess.Read);
    }
}
