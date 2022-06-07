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
    }
}
