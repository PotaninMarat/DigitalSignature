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
        public IActionResult Load(string path, int type)
        {
            var splits = path.Split('\\').Last().Split('.');
            var name = "";
            for (int i = 0; i < splits.Length - 1; i++)
            {
                name += splits[i];
            }

            if (type == 1)
            {
                var paths = System.IO.Directory.GetFiles("Files\\xls");
                foreach (var path_ in paths)
                {
                    var splitsLoc = path_.Split('\\').Last().Split('.');
                    var nameLoc = "";
                    for (int i = 0; i < splitsLoc.Length - 1; i++)
                    {
                        nameLoc += splitsLoc[i];
                    }

                    if (nameLoc == name)
                    {
                        using (var stream = new FileStream(path_, FileMode.Open))
                        {
                            var bytesT = new byte[stream.Length];
                            var l = stream.Read(bytesT, 0, bytesT.Length);
                            var content = new System.IO.MemoryStream(bytesT);
                            //string mimeType = MimeUtility.GetMimeMapping(path);
                            var contentType = "APPLICATION/octet-stream";
                            return File(content, contentType, path_.Split('\\').Last());
                        }
                    }
                }
            }
            else if (type == 2)
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var bytesT = new byte[stream.Length];
                    var l = stream.Read(bytesT, 0, bytesT.Length);
                    var content = new System.IO.MemoryStream(bytesT);
                    //string mimeType = MimeUtility.GetMimeMapping(path);
                    var contentType = "APPLICATION/octet-stream";
                    return File(content, contentType, path.Split('\\').Last());
                }
            }

            return Json("File not exist");
        }
    }
}