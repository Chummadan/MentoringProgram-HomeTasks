using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new List<ImageModel>();

            using (var context = new ProfileSampleEntities())
            {
                var sources = await context.ImgSources
                    .Take(20)
                    .Select(x => new { x.Name, x.Data })
                    .ToListAsync();

                foreach (var item in sources)
                {
                    var obj = new ImageModel()
                    {
                        Name = item.Name,
                        Data = item.Data
                    };

                    model.Add(obj);
                }
            };

            return View(model);
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");
            var images = new List<ImgSource>();

            foreach (var file in files)
            {
                byte[] buff;

                using (var stream = new FileStream(file, FileMode.Open))
                {
                    buff = new byte[stream.Length];
                    stream.Read(buff, 0, (int)stream.Length);
                }

                var entity = new ImgSource()
                {
                    Name = Path.GetFileName(file),
                    Data = buff,
                };

                images.Add(entity);
            }

            using (var context = new ProfileSampleEntities())
            {
                context.ImgSources.AddRange(images);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
