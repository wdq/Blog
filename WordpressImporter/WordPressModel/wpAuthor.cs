using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WordpressImporter.WordPressModel
{
    class wpAuthor
    {

        XNamespace excerptNamespace = "http://wordpress.org/export/1.2/excerpt/";
        XNamespace contentNamespace = "http://purl.org/rss/1.0/modules/content/";
        XNamespace wfwNamespace = "http://wellformedweb.org/CommentAPI/";
        XNamespace dcNamespace = "http://purl.org/dc/elements/1.1/";
        XNamespace wpNamespace = "http://wordpress.org/export/1.2/";

        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public wpAuthor(XElement authorElement)
        {
            Id = int.Parse(authorElement.Element(wpNamespace + "author_id").Value);
            Login = authorElement.Element(wpNamespace + "author_login").Value;
            Email = authorElement.Element(wpNamespace + "author_email").Value;
            DisplayName = authorElement.Element(wpNamespace + "author_display_name").Value;
            FirstName = authorElement.Element(wpNamespace + "author_first_name").Value;
            LastName = authorElement.Element(wpNamespace + "author_last_name").Value;
        }


        public static User AuthorToUser(wpAuthor author)
        {
            User user = new User();
            user.Id = Guid.NewGuid();
            user.Username = author.Login;
            user.Email = author.Email;
            user.PublicName = author.DisplayName;
            user.FirstName = author.FirstName;
            user.LastName = author.LastName;
            user.Role = "Author";
            user.Password = "password";

            return user;
        }
    }
}
