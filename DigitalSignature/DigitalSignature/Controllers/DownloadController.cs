using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeMapping;
//using System.Web.MimeMapping.GetMimeMapping;
namespace DigitalSignature.Controllers
{
    public class DownloadController : Controller
    {
        [Authorize]
        [HttpGet("download")]
        public IActionResult Load(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var bytesT = new byte[stream.Length];
                var l = stream.Read(bytesT, 0, bytesT.Length);
                //var bytes = new byte[l];
                //for (int i = 0; i < l; i++)
                //{
                //    bytes[i] = bytesT[i];
                //}
                var content = new System.IO.MemoryStream(bytesT);
                //string mimeType = MimeUtility.GetMimeMapping(path);
                var contentType = "APPLICATION/octet-stream";
                return File(content, contentType, path.Split('\\').Last());
            }
        }
    }
}