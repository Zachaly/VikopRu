﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Data.FileManager
{
    public interface IFileManager
    {
        FileStream ProfilePictureStream(string image);
        FileStream FindingPictureStream(string image);
        FileStream PostPictureStream(string image);

        Task<string> SaveFindingPicture(IFormFile image);
        Task<string> SaveProfilePicture(IFormFile image);
        Task<string> SavePostPicture(IFormFile image);
    }
}
