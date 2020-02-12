using Mathy.Repository.Repo;
using Mathy.Shared.Domain;
using Mathy.Shared.Entity;
using Mathy.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mathy.Server.Services
{
    public class FileService
    {
        private readonly UesFileRepo fileRepo;
        private readonly FileUtil fileUtil;
        public FileService(UesFileRepo fileRepo, FileUtil fileUtil)
        {
            this.fileRepo = fileRepo;
            this.fileUtil = fileUtil;
        }

        public List<LocalFile> GetFiles(List<string> fileNames)
        {
            var noCacheFIles = new List<int>();
            var result = new List<LocalFile>();
            foreach (var n in fileNames)
            {
                var fileID = Convert.ToInt32(n.Split('_')[0]);
                var fileType = "." + n.Split('.')[1];
                if (fileUtil.IsFileExit(n))
                    result.Add(new LocalFile { FileID = fileID, FileType = fileType, LocalPath = fileUtil.GetSaveFolder() + n });
                else
                    noCacheFIles.Add(fileID);
            }
            if (noCacheFIles.Count > 0)
            {
                var uf = fileRepo.GetFiles(noCacheFIles);
                uf.ForEach(m =>
                {
                    var path = SaveFileToLocal(m.FileBytes, m.FileID, m.FileType);
                    result.Add(new LocalFile { FileID = m.FileID, FileType = m.FileType, LocalPath = fileUtil.GetSaveFolder() + path });
                });
            }
            return result;
        }

        public string AddFile(Stream stream, string fileType)
        {
            var bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes);
            return AddFile(bytes, fileType);
        }

        public string AddFile(byte[] fileBytes, string fileType)
        {
            var id = fileRepo.AddFile(new UesFile
            {
                FileBytes = fileBytes,
                FileType = fileType
            });
            return SaveFileToLocal(fileBytes, id, fileType);
        }

        private string SaveFileToLocal(byte[] bytes, int fileID, string fileType)
        {
            var fileName = fileID + "_local" + fileType;
            fileUtil.SaveFile(bytes, fileName);
            return fileName;
        }
    }
}
