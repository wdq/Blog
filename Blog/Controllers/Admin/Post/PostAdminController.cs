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
        public ActionResult Posts(int? id)
        {
            PostIndexTable model = new PostIndexTable();
            if (id.HasValue)
            {
                model = Models.Admin.Post.PostIndexTable.GetTable(id.Value);
            }
            else
            {
                model = Models.Admin.Post.PostIndexTable.GetTable(1);
            }
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            return View(PostEditModel.PostEdit(id));
        }

        [HttpPost]
        public JsonResult PostIndexTable()
        {
            var postTable = PostIndexTableModel.PostTable(Request);

            return Json(new
            {
                draw = postTable.Draw,
                recordsTotal = postTable.RecordsTotal,
                recordsFiltered = postTable.RecordsFiltered,
                data = postTable.Data
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditPost(PostEditModel model)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = PostEditModel.PostEditPost(model).Id;

            return jsonResult;
        }

        [HttpPost]
        public ActionResult DeletePostPost(string id)
        {
            JsonResult result = new JsonResult();
            result.Data = PostEditModel.DeletePost(id);
            return result;
        }
    }
}