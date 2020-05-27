using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.Controllers
{
    [EnableCors("CorsPolicy")]
    public class HomeController : Controller
    {
        //public IActionResult Index(string login, string pass)
        //{
        //    return View();
        //}

        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var i = User.Identities.FirstOrDefault().Claims.ElementAt(1).Value;
                ViewData["Name"] = "" + i;
                ViewData["Role"] = "Роль: " + i + "";
                return View();
            }
            catch
            {
                return this.BadRequest("Non authorized");
            }
        }

        [Authorize]
        public IActionResult FileEdit()
        {
            try
            {
                if (User.Identities.FirstOrDefault().Claims.ElementAt(1).Value != "write") return this.Unauthorized();
                var i = User.Identities.FirstOrDefault().Claims.ElementAt(1).Value;
                ViewData["Name"] = "" + i;
                ViewData["Role"] = "Роль: " + i + "";
                return View();
            }
            catch
            {
                return this.BadRequest("Non authorized");
            }
        }

        [Authorize]
        public IActionResult FileView()
        {
            try
            {
                var i = User.Identities.FirstOrDefault().Claims.ElementAt(1).Value;
                ViewData["Name"] = "" + i;
                ViewData["Role"] = "Роль: " + i + "";
                return View();
            }
            catch
            {
                return this.BadRequest("Non authorized");
            }
        }


        [Authorize]
        [HttpPost]
        public IActionResult UploadFiles()
        {
            var username = User.Identity.Name;
            Random r = new Random();
            var form = Request.Form;

            var fileOrig = form.ElementAt(0);
            var filePreproc = form.ElementAt(1);

            var fileName1 = Preproc(fileOrig.Key);
            var fileName2 = Preproc(filePreproc.Key);

            var value1 = GetData(fileOrig.Value.FirstOrDefault());
            var value2 = GetData(filePreproc.Value.FirstOrDefault());

            var randInt = r.Next(1, 1000);

            string path1 = $"Files\\xls\\{username}___{fileName1[0]}_{randInt}.{fileName1[1]}";
            string path2 = $"Files\\p7s\\{username}___{fileName1[0]}_{randInt}.{fileName2[1]}";


            using (var stream = new FileStream(path1, FileMode.Create))
            {
                stream.Write(value1, 0, value1.Length);
            }
            using (var stream = new FileStream(path2, FileMode.Create))
            {
                stream.Write(value2, 0, value2.Length);
            }


            return Json("Sucess");
        }

        private string[] Preproc(string fileName)
        {
            var key = fileName;
            var splits = key.Split('.');

            var name = "";
            for (int i = 0; i < splits.Length - 1; i++)
                name += splits[i];

            var zp = splits.Last();

            return new string[] { name, zp };
        }

        private byte[] GetData(string data)
        {
            var value = data;
            var spltB = value.Split(',');

            var bytes = new byte[spltB.Length];
            for (int i = 0; i < spltB.Length; i++)
                bytes[i] = byte.Parse(spltB[i]);

            return bytes;
        }
    }
}