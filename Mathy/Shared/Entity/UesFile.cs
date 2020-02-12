using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mathy.Shared.Entity
{
    public class UesFile
    {
        public int FileID { get; set; }
        public byte[] FileBytes { get; set; }
        public string FileType { get; set; }
        public int DeleteFlag { get; set; } = 0;
    }
}
