using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models.Post;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Page(int? id)
        {
            PostHomeTable model = new PostHomeTable();
            if (id.HasValue)
            {
                model = PostHomeTable.GetTable(id.Value);
            }
            else
            {
                model = PostHomeTable.GetTable(1);
            }

            return View(model);
        }

        [Route("")]
        public ActionResult Index(int? id)
        {
            PostHomeTable model = new PostHomeTable();
            if (id.HasValue)
            {
                model = PostHomeTable.GetTable(id.Value);
            }
            else
            {
                model = PostHomeTable.GetTable(1);
            }

            return View("Page", model);
        }

        [Route("{slug?}")]
        public ActionResult View(string slug)
        {
            return View("View", PostViewModel.PostViewModelFromSlug(slug));
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}