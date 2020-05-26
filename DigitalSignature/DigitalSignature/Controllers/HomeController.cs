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
            var form = Request.Form;
            var first = form.FirstOrDefault();
            var key = first.Key;
            var splits = key.Split('.');
            var name = "";
            for (int i = 0; i < splits.Length-1; i++)
            {
                name += splits[i];
            }

            var zp = "." + splits.Last();
            var value = first.Value.FirstOrDefault();
            var spltB = value.Split(',');
            var bytes = new byte[spltB.Length];
            for (int i = 0; i < spltB.Length; i++)
            {
                bytes[i] = byte.Parse(spltB[i]);
            }
            //value.
            Random r = new Random();
            using(var stream = new FileStream($"Files\\{name}_{r.Next(1, 10000)}{zp}", FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            return Json("Sucess");
        }
    }
}