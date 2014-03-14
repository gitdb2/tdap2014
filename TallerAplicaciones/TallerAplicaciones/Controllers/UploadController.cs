using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Filters;

namespace TallerAplicaciones.Controllers
{
    public class UploadShowModel
    {
        public bool Foto { get; set; }
    }
    [CustomAuthorize]
    public class UploadController : Controller
    {
        // GET: /Upload
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Upload/Foto

        public ActionResult Foto()
        {
            return View("Upload", new UploadShowModel() { Foto = true });
        }

        public ActionResult Video()
        {
            return View("Upload", new UploadShowModel() { Foto = false });
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string puto)
        {

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var basePath = Server.MapPath("~/Uploads");

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                var path = Path.Combine(basePath, fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {

            var basePath = Server.MapPath("~/Uploads");

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            foreach (var file in files)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(basePath, fileName);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("Index");
        }



    }
}
