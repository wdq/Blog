using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers.Post
{
    using Models.Post;
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewId(string id)
        {
            return View("View", PostViewModel.PostViewModelFromId(id));
        }

        public ActionResult View(string slug)
        {
            return View("View", PostViewModel.PostViewModelFromSlug(slug));
        }
    }
}