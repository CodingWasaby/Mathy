using Mathy.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Repository.Repo
{
    public class UesFileRepo : BaseRepo
    {
        public UesFileRepo(DbBase dbBase) : base(dbBase)
        {

        }

        public int AddFile(UesFile file)
        {
            return ExcuteScalar(@"INSERT INTO [dbo].[UesFile]
                                                                       ([FileBytes]
                                                                       ,[FileType]
                                                                       ,[DeleteFlag])
                                                                 VALUES
                                                                       (@FileBytes
                                                                       ,@FileType
                                                                       ,0) 
                                               SELECT @@IDENTITY ", file);
        }

        public List<UesFile> GetFiles(List<int> fileIds)
        {
            return Query<UesFile>(@"SELECT FileID,
                                                                           FileBytes,
                                                                           FileType
                                                                    FROM dbo.UesFile (NOLOCK)
                                                                    WHERE FileID IN  @FileIDs ", new { FileIDs = fileIds });
        }
    }
}
