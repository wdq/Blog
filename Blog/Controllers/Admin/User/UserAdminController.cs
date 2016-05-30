using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models.Admin.User;

namespace Blog.Controllers.Admin.User
{
    public class UserAdminController : Controller
    {
        // GET: UserAdmin
        public ActionResult Index()
        {
            return View(UserIndexModel.UserIndex());
        }

        public ActionResult Edit(string id)
        {
            return View(UserEditModel.UserEdit(id));
        }

        [HttpPost]
        public ActionResult EditPost(UserEditModel model)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = UserEditModel.UserEditPost(model).Id;

            return jsonResult;
        }
    }
}