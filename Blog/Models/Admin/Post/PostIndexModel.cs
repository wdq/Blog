using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.Admin.Post
{
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
