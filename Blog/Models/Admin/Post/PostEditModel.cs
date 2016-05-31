using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Blog.Models.Admin.Post
{
    public class PostEditModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid Author { get; set; }
        public DateTime Timestamp { get; set; }
        public bool CommentsEnabled { get; set; }
        public string Status { get; set; }
        public string Visibility { get; set; }
        public Guid? FeaturedImage { get; set; }
        public string Slug { get; set; }
        public string AddOrEdit { get; set; }

        public PostEditModel()
        {
            
        }

        public PostEditModel(Blog.Post post, string addOrEdit)
        {
            Id = post.Id;
            Title = post.Title;
            Body = post.Body;
            Author = post.Author;
            Timestamp = post.Timestamp;
            CommentsEnabled = post.CommentsEnabled;
            Status = post.Status;
            Visibility = post.Visibility;
            FeaturedImage = post.FeaturedImage;
            Slug = post.Slug;
            AddOrEdit = addOrEdit;
        }

        public static PostEditModel PostEdit(string id)
        {
            BlogDataDataContext database = new BlogDataDataContext();

            if (id.IsNullOrWhiteSpace())
            {
                PostEditModel model = new PostEditModel(new Blog.Post(), "Add");
                return model;
            }
            else
            {
                Blog.Post post = database.Posts.FirstOrDefault(x => x.Id == new Guid(id));
                PostEditModel model = new PostEditModel(post, "Edit");
                return model;
            }
        }

        public static Blog.Post PostEditPost(PostEditModel model)
        {
            BlogDataDataContext database = new BlogDataDataContext();
            Blog.Post databaseModel = database.Posts.FirstOrDefault(x => x.Id == model.Id);

            if (databaseModel != null)
            {
                databaseModel.Title = model.Title;
                databaseModel.Body = model.Body;
                databaseModel.Author = new Guid("cebe8069-bd64-4dc9-8622-d3de189287b1"); // todo: get current user id
                databaseModel.Timestamp = model.Timestamp;
                databaseModel.CommentsEnabled = model.CommentsEnabled;
                databaseModel.Status = model.Status;
                databaseModel.Visibility = model.Visibility;
                databaseModel.FeaturedImage = model.FeaturedImage;
                databaseModel.Slug = model.Slug;
            }
            else
            {
                databaseModel = new Blog.Post();

                databaseModel.Id = Guid.NewGuid();

                databaseModel.Title = model.Title;
                databaseModel.Body = model.Body;
                databaseModel.Author = new Guid("cebe8069-bd64-4dc9-8622-d3de189287b1"); // todo: get current user id
                databaseModel.Timestamp = model.Timestamp;
                databaseModel.CommentsEnabled = model.CommentsEnabled;
                databaseModel.Status = model.Status;
                databaseModel.Visibility = model.Visibility;
                databaseModel.FeaturedImage = model.FeaturedImage;
                databaseModel.Slug = model.Slug;

                database.Posts.InsertOnSubmit(databaseModel);
            }
            database.SubmitChanges();

            return databaseModel;
        }
    }
}