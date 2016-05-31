using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models.Admin.Post;

namespace Blog.Controllers.Admin.Post
{
    public class PostAdminController : Controller
    {
        // GET: PostAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            return View(PostEditModel.PostEdit(id));
        }

        [HttpPost]
        public ActionResult EditPost(PostEditModel model)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = PostEditModel.PostEditPost(model).Id;

            return jsonResult;
        }
    }
}