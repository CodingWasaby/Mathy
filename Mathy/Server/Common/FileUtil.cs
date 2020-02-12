using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mathy.Utils
{
    public class FileUtil
    {
        private readonly IConfiguration _Configuration;
        private readonly IWebHostEnvironment _Environment;
        public FileUtil(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _Configuration = configuration;
            _Environment = environment;
        }
        public bool IsFileExit(string fileName)
        {
            return File.Exists(GetSaveFolder() + fileName);
        }

        public void SaveFile(byte[] bytes, string fileName)
        {
            var sp = GetSaveFolder();
            if (!Directory.Exists(sp))
            {
                Directory.CreateDirectory(sp);
            }
            var fs = File.Create(sp + fileName);
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();
            fs.Close();
        }

        public string GetSaveFolder()
        {
            return _Environment.ContentRootPath + _Configuration.GetSection("LocalFileSavePath").Value;
        }
    }
}
