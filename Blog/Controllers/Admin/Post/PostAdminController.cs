using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers.Admin.Post
{
    public class PostAdminController : Controller
    {
        // GET: PostAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}