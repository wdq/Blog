using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.Admin.Post
{
    public class PostIndexModel
    {
        public List<Blog.Post> Posts { get; set; }

        public static PostIndexModel PostIndex()
        {
            PostIndexModel model = new PostIndexModel();;

            BlogDataDataContext database = new BlogDataDataContext();
            List<Blog.Post> posts = database.Posts.ToList();
            model.Posts = posts;

            return model;
        }
    }
}
