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

        public FileManager(IConfiguration configuration)
        {
            _profilePicturePath = configuration["Path:ProfilePictures"];
        }

        public FileStream ProfilePictureStream(string image)
            => new FileStream(Path.Combine(_profilePicturePath, image), FileMode.Open, FileAccess.Read);
    }
}
