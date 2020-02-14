using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mathy.Server.Common;
using Mathy.Shared;
using Mathy.Utils.Dandelion.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Petunia;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mathy.Server.Controllers
{
    [ApiController]
    [Route("Document/")]
    public class DocumentController : BaseController
    {
        private readonly IWebHostEnvironment hostEnvironment;
        public DocumentController(IWebHostEnvironment environment)
        {
            hostEnvironment = environment;
        }

        [HttpGet]
        public ResponseResult<List<FuncDoc>> Index()
        {
            {
                return ToResponse(() =>
                {
                    return GetDocs();
                });
            }
        }

        private List<FuncDoc> GetDocs()
        {
            var docs = LocalCache.Get<List<FuncDoc>>("FuncDoc");
            if (docs == null)
            {
                docs = (new JsonDeserializer().DeserializeString(System.IO.File.ReadAllText(hostEnvironment.ContentRootPath + "/Repository/Docs/Funcs.txt", System.Text.Encoding.UTF8), typeof(FuncDoc[])) as FuncDoc[]).ToList();
                foreach (FuncDoc doc in docs)
                {
                    doc.ArticleStr = System.IO.File.ReadAllText(hostEnvironment.ContentRootPath + string.Format(@"/Repository/Docs/Details/{0}.txt", doc.Name));
                }
                LocalCache.Set("FuncDoc", docs);
            }
            return docs;
        }
    }
}
