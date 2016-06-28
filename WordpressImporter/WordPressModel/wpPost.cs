using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WordpressImporter.WordPressModel
{
    class wpPost
    {
        XNamespace excerptNamespace = "http://wordpress.org/export/1.2/excerpt/";
        XNamespace contentNamespace = "http://purl.org/rss/1.0/modules/content/";
        XNamespace wfwNamespace = "http://wellformedweb.org/CommentAPI/";
        XNamespace dcNamespace = "http://purl.org/dc/elements/1.1/";
        XNamespace wpNamespace = "http://wordpress.org/export/1.2/";

        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
        public Guid Creator { get; set; }
        public string Guid { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public int PostId { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime PostDateGmt { get; set; }
        public string CommentStatus { get; set; }
        public string PostName { get; set; }
        public string Status { get; set; }
        public int PostParent { get; set; }
        public int MenuOrder { get; set; }
        public string PostType { get; set; }
        public string PostPassword { get; set; }
        public string IsSticky { get; set; }
        public List<wpCategory> Categories { get; set; }
        public List<wpTag> Tags { get; set; }
        public List<wpComment> Comments { get; set; }

        public wpPost(XElement postElement)
        {
            BlogDataDataContext database = new BlogDataDataContext();

            Title = postElement.Element("title").Value;
            Link = postElement.Element("link").Value;
            PubDate = DateTime.Parse(postElement.Element("pubDate").Value);
            Creator = database.Users.FirstOrDefault(x => x.Username == postElement.Element(dcNamespace + "creator").Value).Id;
            Guid = postElement.Element("guid").Value;
            Description = postElement.Element("description").Value;
            Content = postElement.Element(contentNamespace + "encoded").Value;
            Excerpt = postElement.Element(excerptNamespace + "encoded").Value;
            PostId = int.Parse(postElement.Element(wpNamespace + "post_id").Value);
            PostDate = DateTime.Parse(postElement.Element(wpNamespace + "post_date").Value);
            PostDateGmt = DateTime.Parse(postElement.Element(wpNamespace + "post_date_gmt").Value);
            CommentStatus = postElement.Element(wpNamespace + "comment_status").Value;
            PostName = postElement.Element(wpNamespace + "post_name").Value;
            Status = postElement.Element(wpNamespace + "status").Value;
            PostParent = int.Parse(postElement.Element(wpNamespace + "post_parent").Value);
            MenuOrder = int.Parse(postElement.Element(wpNamespace + "menu_order").Value);
            PostType = postElement.Element(wpNamespace + "post_type").Value;
            PostPassword = postElement.Element(wpNamespace + "post_password").Value;
            IsSticky = postElement.Element(wpNamespace + "is_sticky").Value;

            List<wpCategory> categoriesTemp = new List<wpCategory>();
            IEnumerable<XElement> categories = postElement.Elements("category").Where(x => (string)x.Attribute("domain") == "category");
            foreach (var category in categories)
            {
                wpCategory newCategory = new wpCategory(category.Attribute("nicename").Value, category.Value);
                categoriesTemp.Add(newCategory);
            }
            Categories = categoriesTemp;

            List<wpTag> tagsTemp = new List<wpTag>();
            IEnumerable<XElement> tags = postElement.Elements("category").Where(x => (string)x.Attribute("domain") == "post_tag");
            foreach (var tag in tags)
            {
                wpTag newTag = new wpTag(tag.Attribute("nicename").Value, tag.Value);
                tagsTemp.Add(newTag);
            }
            Tags = tagsTemp;

            List<wpComment> commentsTemp = new List<wpComment>();
            IEnumerable<XElement> comments = postElement.Elements(wpNamespace + "comment");
            foreach (var comment in comments)
            {
                wpComment newComment = new wpComment(comment);
                commentsTemp.Add(newComment);
            }
            Comments = commentsTemp;

        }


        public static bool ImportPost(wpPost post)
        {
            BlogDataDataContext database = new BlogDataDataContext();
            string slug = post.Title.Substring(post.Title.IndexOf("http://theprime.co/") + "http://theprime.co/".Length - 1);
            Post databasePost = database.Posts.FirstOrDefault(x => x.Slug == slug);
            if (databasePost == null)
            {
                Post newPost = new Post();
                newPost.Id = System.Guid.NewGuid();
                newPost.Title = post.Title;
                newPost.Body = post.Content;
                newPost.Author = post.Creator;
                newPost.Timestamp = post.PubDate;
                if (post.CommentStatus.ToLower() == "enabled")
                {
                    newPost.CommentsEnabled = true;
                }
                else
                {
                    newPost.CommentsEnabled = false;
                }

                newPost.Status = post.Status;
                newPost.Visibility = "Public";
                newPost.Slug = slug;

                database.Posts.InsertOnSubmit(newPost);
                database.SubmitChanges();

                foreach (var category in post.Categories)
                {
                    PostCategoryMap map = new PostCategoryMap();

                    PostCategory databaseCategory = database.PostCategories.FirstOrDefault(x => x.Slug == category.Slug);
                    if (databaseCategory == null)
                    {
                        PostCategory newCategory = new PostCategory();
                        newCategory.Id = System.Guid.NewGuid();
                        newCategory.Slug = category.Slug;
                        newCategory.Name = category.Name;
                        database.PostCategories.InsertOnSubmit(newCategory);
                        map.CategoryId = newCategory.Id;

                        database.SubmitChanges();
                    }
                    else
                    {
                        map.CategoryId = databaseCategory.Id;
                    }

                    map.PostId = newPost.Id;
                    database.PostCategoryMaps.InsertOnSubmit(map);
                    database.SubmitChanges();
                }

                foreach (var tag in post.Tags)
                {
                    PostTagMap map = new PostTagMap();

                    PostTag databaseTag = database.PostTags.FirstOrDefault(x => x.Slug == tag.Slug);
                    if (databaseTag == null)
                    {
                        PostTag newTag = new PostTag();
                        newTag.Id = System.Guid.NewGuid();
                        newTag.Slug = tag.Slug;
                        newTag.Name = tag.Name;
                        database.PostTags.InsertOnSubmit(newTag);
                        map.TagId = newTag.Id;

                        database.SubmitChanges();
                    }
                    else
                    {
                        map.TagId = databaseTag.Id;
                    }

                    map.PostId = newPost.Id;
                    database.PostTagMaps.InsertOnSubmit(map);
                    database.SubmitChanges();
                }

                foreach (var comment in post.Comments)
                {
                    
                }

            }

            return true;
        }

    }
}
