using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.Admin.User
{
    public class UserIndexModel
    {
        public List<Blog.User> Users { get; set; }

        public static UserIndexModel UserIndex()
        {
            UserIndexModel model = new UserIndexModel();

            BlogDataDataContext database = new BlogDataDataContext();
            List<Blog.User> users = database.Users.ToList();
            model.Users = users;

            return model;
        }
    }
}
