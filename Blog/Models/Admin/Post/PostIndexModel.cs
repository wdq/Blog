using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Models.Admin.Post
{
    public class PostIndexTableResultModel
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<PostIndexTableModel> Data { get; set; }

        public PostIndexTableResultModel(int draw, int recordsTotal, int recordsFiltered, List<PostIndexTableModel> data)
        {
            Draw = draw;
            RecordsTotal = recordsTotal;
            RecordsFiltered = recordsFiltered;
            Data = data;
        }
    }

    public class PostIndexTableModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Categories { get; set; }
        public string Tags { get; set; }
        public string Comments { get; set; }
        public string Timestamp { get; set; }

        public static PostIndexTableResultModel PostTable(HttpRequestBase Request)
        {
            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

            List<PostIndexTableModel> postsTable = new List<PostIndexTableModel>();

            using (BlogDataDataContext database = new BlogDataDataContext())
            {
                var posts = database.Posts.ToList();

                foreach (var post in posts)
                {
                    postsTable.Add(FromPost(post, database));
                }
            }

            int totalRecords = postsTable.Count;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                postsTable = postsTable.Where(x =>
                (x.Title != null ? x.Title.ToLower() : "").Contains(search.ToLower()) ||
                (x.Author != null ? x.Author.ToLower() : "").Contains(search.ToLower()) ||
                (x.Categories != null ? x.Categories.ToLower() : "").Contains(search.ToLower()) ||
                (x.Tags != null ? x.Tags.ToLower() : "").Contains(search.ToLower()) ||
                (x.Comments != null ? x.Comments.ToLower() : "").Contains(search.ToLower()) ||
                (x.Timestamp != null ? x.Timestamp.ToLower() : "").Contains(search.ToLower())
                ).ToList();
            }

            postsTable = SortByColumnWithOrder(order, orderDir, postsTable);

            int recFilter = postsTable.Count;
            postsTable = postsTable.Skip(startRec).Take(pageSize).ToList();

            return new PostIndexTableResultModel(Convert.ToInt32(draw), totalRecords, recFilter, postsTable);
        }

        public static PostIndexTableModel FromPost(Blog.Post post, BlogDataDataContext database)
        {
            var model = new PostIndexTableModel();

            var commandButtonLeftHtml = "";
            commandButtonLeftHtml += "<a href='Post/ViewId?id=" + post.Id.ToString() + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-search'></i></span></a>";
            commandButtonLeftHtml += "<a href='PostAdmin/Edit?id=" + post.Id.ToString() + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-pencil'></i></span></a>";
            commandButtonLeftHtml += "<div class='btn btn-default hl-view deletePostButton' postId='" + post.Id.ToString() + "'><i class='fa fa-trash'></i></div>";

            model.Id = commandButtonLeftHtml;
            model.Title = post.Title;
            model.Timestamp = post.Timestamp.ToString();

            /*
            var databaseAuthor = database.Users.FirstOrDefault(x => x.Id == post.Author);
            if (databaseAuthor != null)
            {
                model.Author = databaseAuthor.PublicName;
            }*/

            /*string tagsTemp = "";
            var databaseTagMaps = database.PostTagMaps.Where(x => x.PostId == post.Id);
            foreach (var tagMap in databaseTagMaps)
            {
                var databaseTag = database.PostTags.FirstOrDefault(x => x.Id == tagMap.TagId);
                tagsTemp += "<a href='" + "#" + "'>" + databaseTag.Name + "</a>, ";
            }
            model.Tags = tagsTemp; */
            /*            
            string categoriesTemp = "";
            var databaseCategoryMaps = database.PostCategoryMaps.Where(x => x.PostId == post.Id);
            foreach (var categoryMap in databaseCategoryMaps)
            {
                var databaseCategory = database.PostCategories.FirstOrDefault(x => x.Id == categoryMap.CategoryId);
                categoriesTemp += "<a href='" + "#" + "'>" + databaseCategory.Name + "</a>, ";
            }
            model.Categories = categoriesTemp;
            */

            //model.Comments = database.PostCommentMaps.Count(x => x.PostId == post.Id).ToString();

            return model;
        }

        public static List<PostIndexTableModel> SortByColumnWithOrder(string order, string orderDir,
            List<PostIndexTableModel> data)
        {
            List<PostIndexTableModel> list = new List<PostIndexTableModel>();

            try
            {
                switch (order)
                {
                    case "0":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Id).ToList() : data.OrderBy(x => x.Id).ToList();
                        break;
                    case "1":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Title).ToList() : data.OrderBy(x => x.Title).ToList();
                        break;
                    case "2":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Author).ToList() : data.OrderBy(x => x.Author).ToList();
                        break;
                    case "3":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Categories).ToList() : data.OrderBy(x => x.Categories).ToList();
                        break;
                    case "4":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Tags).ToList() : data.OrderBy(x => x.Tags).ToList();
                        break;
                    case "5":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Comments).ToList() : data.OrderBy(x => x.Comments).ToList();
                        break;
                    case "6":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => Convert.ToDateTime(x.Timestamp).Ticks).ToList() : data.OrderBy(x => Convert.ToDateTime(x.Timestamp).Ticks).ToList();
                        break;
                    default:
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => Convert.ToDateTime(x.Timestamp).Ticks).ToList() : data.OrderBy(x => Convert.ToDateTime(x.Timestamp).Ticks).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return list;
        }
    }

    public class PostIndexModelPost
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public KeyValuePair<Guid, string> Author { get; set; }
        public Dictionary<Guid, string> Categories { get; set; }
        public Dictionary<Guid, string> Tags { get; set; }
        public string Comments { get; set; }
        public string Timestamp { get; set; }

        public PostIndexModelPost(Blog.Post post)
        {
            BlogDataDataContext database = new BlogDataDataContext();

            Id = post.Id.ToString();
            Title = post.Title.ToString();

            Blog.User user = database.Users.FirstOrDefault(x => x.Id == post.Author);
            Author = new KeyValuePair<Guid, string>(user.Id, user.FirstName + " " + user.LastName);

            Dictionary<Guid, string> categoriesTemp = new Dictionary<Guid, string>();
            List<Blog.PostCategoryMap> categoryMaps = database.PostCategoryMaps.Where(x => x.PostId == post.Id).ToList();
            foreach(var categoryMap in categoryMaps)
            {
                Blog.PostCategory category = database.PostCategories.FirstOrDefault(x => x.Id == categoryMap.CategoryId);
                categoriesTemp.Add(category.Id, category.Name);
            }
            Categories = categoriesTemp;

            Dictionary<Guid, string> tagsTemp = new Dictionary<Guid, string>();
            List<Blog.PostTagMap> tagMaps = database.PostTagMaps.Where(x => x.PostId == post.Id).ToList();
            foreach (var tagMap in tagMaps)
            {
                Blog.PostTag tag = database.PostTags.FirstOrDefault(x => x.Id == tagMap.TagId);
                tagsTemp.Add(tag.Id, tag.Name);
            }
            Tags = tagsTemp;

            Comments = "0"; // todo: add comments table, and do a count of them here
            Timestamp = post.Timestamp.ToString();

        }
    }

    public class PostIndexModel
    {
        public List<PostIndexModelPost> Posts { get; set; }

        public static PostIndexModel PostIndex()
        {
            PostIndexModel model = new PostIndexModel();;

            BlogDataDataContext database = new BlogDataDataContext();
            List<Blog.Post> databasePosts = database.Posts.ToList();
            List<PostIndexModelPost> posts = new List<PostIndexModelPost>();
            foreach(var post in databasePosts)
            {
                posts.Add(new PostIndexModelPost(post));
            }
            model.Posts = posts;

            return model;
        }
    }
}
