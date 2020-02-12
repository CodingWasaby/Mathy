using Mathy.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Domain
{
    public class LocalFile
    {
        public int FileID { get; set; }
        public string FileType { get; set; }
        public string LocalPath { get; set; }
    }
}
