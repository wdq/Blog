using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using PressSharp;
using WordpressImporter.WordPressModel;

namespace WordpressImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            BlogDataDataContext database = new BlogDataDataContext();

            XNamespace excerptNamespace = "http://wordpress.org/export/1.2/excerpt/";
            XNamespace contentNamespace = "http://purl.org/rss/1.0/modules/content/";
            XNamespace wfwNamespace = "http://wellformedweb.org/CommentAPI/";
            XNamespace dcNamespace = "http://purl.org/dc/elements/1.1/";
            XNamespace wpNamespace = "http://wordpress.org/export/1.2/";
            
            XElement inputFile = XElement.Load(@"C:\Users\wquade\Downloads\theprime.wordpress.2016-06-28_posts.xml");

            IEnumerable<XElement> authorElements = inputFile.Elements().Elements(wpNamespace + "author");
            foreach (XElement authorElement in authorElements)
            {
                User user = wpAuthor.AuthorToUser(new wpAuthor(authorElement));
                Console.WriteLine("Importing user: " + user.Username);
                var databaseUser = database.Users.FirstOrDefault(x => x.Username == user.Username);
                if (databaseUser == null)
                {
                    database.Users.InsertOnSubmit(user);
                    database.SubmitChanges();
                }
            }


            /*IEnumerable<XElement> elements = inputFile.Elements().Elements("item");
            foreach (var element in elements)
            {
                string title = element.Element("title").Value;
                string link = element.Element("link").Value;
                string pubDate = element.Element("pubDate").Value;
                string creator = element.Element(dcNamespace + "creator").Value;
                string guid = element.Element("guid").Value;
                string description = element.Element("description").Value;
                string content = element.Element(contentNamespace + "encoded").Value;
                string excerpt = element.Element(excerptNamespace + "encoded").Value;
                string comment_status = element.Element(wpNamespace + "comment_status").Value;
                string post_name = element.Element(wpNamespace + "post_name").Value;
                string status = element.Element(wpNamespace + "status").Value;
                string post_type = element.Element(wpNamespace + "post_type").Value;

                IEnumerable<XElement> tags = element.Elements("category").Where(x => (string) x.Attribute("domain") == "post_tag");
                foreach (var tag in tags)
                {
                    string tagValue = tag.Value;
                }

                IEnumerable<XElement> categories = element.Elements("category").Where(x => (string)x.Attribute("domain") == "category");
                foreach (var category in categories)
                {
                    string categoryValue = category.Value;
                }
            }*/


            /*string inputFile = File.ReadAllText(@"C:\Users\wquade\Downloads\theprime.wordpress.2016-06-28_posts.xml");
            var wordpressBlog = new Blog(inputFile);
            Console.WriteLine("Blog title: " + wordpressBlog.Title);
            Console.WriteLine("Blog description: " + wordpressBlog.Description);
            wordpressBlog.Initialize();
            foreach (var author in wordpressBlog.Authors)
            {
                Console.WriteLine("Importing author: " + author.Username);
                var matchingDatabaseUser = database.Users.FirstOrDefault(x => x.Username == author.Username); // search database to see if username already exists
                if (matchingDatabaseUser == null) // user doesn't exist
                {
                    User newUser = new User(); // add user to database
                    newUser.Id = Guid.NewGuid();
                    newUser.Username = author.Username;
                    newUser.Email = author.Email;
                    newUser.Role = "User";
                    newUser.PublicName = newUser.Username;
                    database.Users.InsertOnSubmit(newUser);
                    database.SubmitChanges();
                }
            }

            foreach (var tag in wordpressBlog.Tags)
            {
                Console.WriteLine("Importing tag: " + tag.Slug);
                var matchingDatabaseTag = database.PostTags.FirstOrDefault(x => x.Slug == tag.Slug); // search database to see if tag already exists
                if (matchingDatabaseTag == null) // tag doesn't exist
                {
                    PostTag newTag = new PostTag(); // add tag to the database
                    newTag.Id = Guid.NewGuid();
                    newTag.Name = tag.Slug;
                    newTag.Slug = tag.Slug;
                    database.PostTags.InsertOnSubmit(newTag);
                    database.SubmitChanges();
                }
            }

            foreach (var category in wordpressBlog.Categories)
            {
                Console.WriteLine("Importing category: " + category.Slug);
                var matchingDatabaseCategory = database.PostCategories.FirstOrDefault(x => x.Slug == category.Slug); // search database to see if category already exists
                if (matchingDatabaseCategory == null) // category doesn't exist
                {
                    PostCategory newCategory = new PostCategory(); // add category to the database
                    newCategory.Id = Guid.NewGuid();
                    newCategory.Slug = category.Slug;
                    newCategory.Name = category.Name;
                    database.PostCategories.InsertOnSubmit(newCategory);
                    database.SubmitChanges();
                }
            }

            var posts = wordpressBlog.GetPosts();
            foreach (var post in posts)
            {
                Console.WriteLine("Importing post: " + post.Title);
                var matchingDatabasePost = database.Posts.FirstOrDefault(x => x.Slug == post.Slug); // search database to see if post already exists
                if (matchingDatabasePost == null) // post doesn't eixst
                {
                    Post newPost = new Post(); // add post to the database
                    newPost.Id = Guid.NewGuid();
                    newPost.Slug = post.Slug;
                    newPost.Body = post.Body;
                    newPost.Title = post.Title;
                    newPost.Author = database.Users.FirstOrDefault(x => x.Username == post.Author.Username).Id;
                    newPost.Timestamp = post.PublishedAtUtc.LocalDateTime; // todo: may need an offset
                    foreach (var category in post.Categories)
                    {
                        var databaseCategory = database.PostCategories.FirstOrDefault(x => x.Slug == category.Slug);
                        if (databaseCategory != null)
                        {
                            PostCategoryMap map = new PostCategoryMap();
                            map.PostId = newPost.Id;
                            map.CategoryId = databaseCategory.Id;
                            map.Id = Guid.NewGuid();
                            database.PostCategoryMaps.InsertOnSubmit(map);
                            database.SubmitChanges();
                        }
                    }
                    foreach (var tag in post.Tags)
                    {
                        var databaseTag = database.PostTags.FirstOrDefault(x => x.Slug == tag.Slug);
                        if (databaseTag != null)
                        {
                            PostTagMap map = new PostTagMap();
                            map.PostId = newPost.Id;
                            map.TagId = databaseTag.Id;
                            map.Id = Guid.NewGuid();
                            database.PostTagMaps.InsertOnSubmit(map);
                            database.SubmitChanges();
                        }
                    }
                    newPost.CommentsEnabled = false;
                    newPost.Status = "Published";
                    newPost.Visibility = "Public";
                }
            }
            Console.WriteLine("Done"); */
            Console.WriteLine("Done");
            Console.ReadKey(); 
        }
    }
}
