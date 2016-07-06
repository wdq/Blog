using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;

namespace Blog.Models.Post
{

    public class PostHomeFeaturedPost
    {
        public string Title { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string Url { get; set; }

        public static PostHomeFeaturedPost PostToFeaturedPost(Blog.Post post, BlogDataDataContext database)
        {
            PostHomeFeaturedPost featuredPost = new PostHomeFeaturedPost();

            string rootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
            string postUrl = rootUrl + post.Slug;

            featuredPost.Title = post.Title;
            featuredPost.Url = postUrl;

            string imageUrl = Regex.Match(post.Body, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

            if (!String.IsNullOrEmpty(imageUrl) && !String.IsNullOrWhiteSpace(imageUrl))
            {
                featuredPost.FeaturedImageUrl = imageUrl;
            }

            return featuredPost;
        }
    }

    public class PostHomeTableRow
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public string Categories { get; set; }
        public string Tags { get; set; }
        public string Comments { get; set; }
        public string Timestamp { get; set; }
        public string FeaturedImage { get; set; }

        public static string ScrubHtml(string value)
        {
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");

            return step2;
        }

        public static PostHomeTableRow PostToRow(Blog.Post post, BlogDataDataContext database)
        { 
            PostHomeTableRow row = new PostHomeTableRow();

            string rootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
            string postUrl = rootUrl + post.Slug;

            var commandButtonLeftHtml = "";
            commandButtonLeftHtml += "<a href='Post/ViewId?id=" + post.Id + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-search'></i></span></a>";
            commandButtonLeftHtml += "<a href='PostAdmin/Edit?id=" + post.Id + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-pencil'></i></span></a>";
            commandButtonLeftHtml += "<div class='btn btn-default hl-view deletePostButton' postId='" + post.Id + "'><i class='fa fa-trash'></i></div>";

            row.Id = commandButtonLeftHtml;
            row.Title = "<a href='" + postUrl  + "'>" + post.Title + "</a>";
            row.Timestamp = post.Timestamp.ToString();


            var databaseAuthor = database.Users.FirstOrDefault(x => x.Id == post.Author);
            if (databaseAuthor != null)
            {
                row.Author = databaseAuthor.PublicName;
            }

            var bodyText = ScrubHtml(post.Body);
            if (bodyText.Length > 300)
            {
                row.Body = bodyText.Substring(0, 300) + " ... <a href='" + postUrl + "'>[Read more...]</a>";
            }
            else
            {
                row.Body = bodyText + " ... <a href='" + postUrl + "'>[Read more...]</a>";
            }

            string tagsTemp = "";
            var databaseTagMaps = database.PostTagMaps.Where(x => x.PostId == post.Id);
            foreach (var tagMap in databaseTagMaps)
            {
                var databaseTag = database.PostTags.FirstOrDefault(x => x.Id == tagMap.TagId);
                tagsTemp += "<a href='" + "#" + "'>" + databaseTag.Name + "</a>, ";
            }
            row.Tags = tagsTemp;

            string categoriesTemp = "";
            var databaseCategoryMaps = database.PostCategoryMaps.Where(x => x.PostId == post.Id);
            foreach (var categoryMap in databaseCategoryMaps)
            {
                var databaseCategory = database.PostCategories.FirstOrDefault(x => x.Id == categoryMap.CategoryId);
                categoriesTemp += "<a href='" + "#" + "'>" + databaseCategory.Name + "</a>, ";
            }
            row.Categories = categoriesTemp;

            row.Comments = database.PostCommentMaps.Count(x => x.PostId == post.Id).ToString();

            string imageUrl = Regex.Match(post.Body, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

            if (!String.IsNullOrEmpty(imageUrl) && !String.IsNullOrWhiteSpace(imageUrl))
            {
                row.FeaturedImage = "<div class='center-cropped-home'><a href='" + postUrl + "'><img src='" + imageUrl + "'/></a></div>";
            }

            return row;
        }
    }

    public class PostHomeTable
    {
        public List<PostHomeFeaturedPost> FeaturedPosts { get; set; }
        public List<PostHomeTableRow> Rows { get; set; }
        public string Pagination { get; set; }

        public static PostHomeTable GetTable(int page)
        {
            PostHomeTable table = new PostHomeTable();
            List<PostHomeFeaturedPost> featuredPosts = new List<PostHomeFeaturedPost>();
            List<PostHomeTableRow> rows = new List<PostHomeTableRow>();

            using (BlogDataDataContext database = new BlogDataDataContext())
            {
                var allPosts = database.Posts.Where(x => x.Status == "publish" && x.Visibility == "Public").ToList().OrderByDescending(x => x.Timestamp);
                var firstThreePosts = allPosts.Take(3);
                foreach (var featuredPost in firstThreePosts)
                {
                    featuredPosts.Add(PostHomeFeaturedPost.PostToFeaturedPost(featuredPost, database));
                }

                var posts = allPosts.Skip((page - 1)*10).Take(10);
                foreach (var post in posts)
                {
                    rows.Add(PostHomeTableRow.PostToRow(post, database));
                }
                table.Rows = rows;

                string rootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                int numberOfPages = (int)Math.Ceiling((double)allPosts.Count()/(double)10);
                string pagination = "<ul class='pagination'>";
                if (page != 1)
                {
                    pagination += "<li><a href='" + rootUrl + "Page/" + (page - 1) + "'>« Previous Page</a></li>";
                    if ((page - 3) > 1)
                    {
                        pagination += "<li><a href='" + rootUrl + "Page/" + 1 + "'>1</a></li>";
                        pagination += "<li class='disabled'><a href='#'>...</a></li>";
                    }
                    else if (page == 4)
                    {
                        pagination += "<li><a href='" + rootUrl + "Page/" + 1 + "'>1</a></li>";
                    }
                }

                if ((page - 2) > 0)
                {
                    pagination += "<li><a href='" + rootUrl + "Page/" + (page - 2) + "'>" + (page - 2) + "</a></li>";
                }
                if ((page - 1) > 0)
                {
                    pagination += "<li><a href='" + rootUrl + "Page/" + (page - 1) + "'>" + (page - 1) + "</a></li>";
                }
                pagination += "<li class='active'><a href='" + rootUrl + "Page/" + (page) + "'>" + (page) + "</a></li>";
                if ((page + 1) < numberOfPages)
                {
                    pagination += "<li><a href='" + rootUrl + "Page/" + (page + 1) + "'>" + (page + 1) + "</a></li>";
                }
                if ((page + 2) < numberOfPages)
                {
                    pagination += "<li><a href='" + rootUrl + "Page/" + (page + 2) + "'>" + (page + 2) + "</a></li>";
                }

                if (page != numberOfPages)
                {
                    if ((page + 3) < numberOfPages)
                    {
                        pagination += "<li class='disabled'><a href='#'>...</a></li>";
                        pagination += "<li><a href='" + rootUrl + "Page/" + numberOfPages + "'>" + numberOfPages + "</a></li>";
                    } else if (page == (numberOfPages - 1) || page == (numberOfPages - 2) || page == (numberOfPages - 3))
                    {
                        pagination += "<li><a href='" + rootUrl + "Page/" + numberOfPages + "'>" + numberOfPages + "</a></li>";
                    }
                    pagination += "<li><a href='" + rootUrl + "Page/" + (page + 1) + "'>Next Page »</a></li>";
                }

                pagination += "</ul>";
                table.Pagination = pagination;
                table.FeaturedPosts = featuredPosts;
            }


            return table;
        }
    }
}