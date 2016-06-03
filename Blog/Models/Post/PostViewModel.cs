using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models.Post
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public KeyValuePair<Guid, string> Author { get; set; }
        public Dictionary<Guid, string> Categories { get; set; }
        public Dictionary<Guid, string> Tags { get; set; }
        public string Timestamp { get; set; }
        public string Body { get; set; }
        public bool CommentsEnabled { get; set; }
        public string Status { get; set; }
        public string Visibility { get; set; }
        public Guid? FeaturedImage { get; set; }
        public string Slug { get; set; }
        // todo: comments list

        public PostViewModel(Blog.Post post)
        {
            BlogDataDataContext database = new BlogDataDataContext();

            Id = post.Id.ToString();
            Title = post.Title;
            Body = post.Body;

            Blog.User user = database.Users.FirstOrDefault(x => x.Id == post.Author);
            Author = new KeyValuePair<Guid, string>(user.Id, user.FirstName + " " + user.LastName);

            Dictionary<Guid, string> categoriesTemp = new Dictionary<Guid, string>();
            List<Blog.PostCategoryMap> categoryMaps = database.PostCategoryMaps.Where(x => x.PostId == post.Id).ToList();
            foreach (var categoryMap in categoryMaps)
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

            Timestamp = post.Timestamp.ToString();
            CommentsEnabled = post.CommentsEnabled;
            Status = post.Status;
            Visibility = post.Visibility;
            Slug = post.Slug;
        }

        public static PostViewModel PostViewModelFromId(string id)
        {
            BlogDataDataContext database = new BlogDataDataContext();
            var post = database.Posts.FirstOrDefault(x => x.Id == new Guid(id));
            return new PostViewModel(post);
        }

        public static PostViewModel PostViewModelFromSlug(string slug) // todo: make the slug a unique property in the database
        {
            BlogDataDataContext database = new BlogDataDataContext();
            var post = database.Posts.FirstOrDefault(x => x.Slug == slug);
            return new PostViewModel(post);
        }
    }
}