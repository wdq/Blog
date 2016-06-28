using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WordpressImporter.WordPressModel
{
    class wpComment
    {
        XNamespace excerptNamespace = "http://wordpress.org/export/1.2/excerpt/";
        XNamespace contentNamespace = "http://purl.org/rss/1.0/modules/content/";
        XNamespace wfwNamespace = "http://wellformedweb.org/CommentAPI/";
        XNamespace dcNamespace = "http://purl.org/dc/elements/1.1/";
        XNamespace wpNamespace = "http://wordpress.org/export/1.2/";

        public int Id { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorUrl { get; set; }
        public string AuthorIp { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateGmt { get; set; }
        public string Content { get; set; }
        public string Approved { get; set; }
        public string Type { get; set; }
        public int Parent { get; set; }
        public int UserId { get; set; }

        public wpComment(XElement commentElement)
        {
            Id = int.Parse(commentElement.Element(wpNamespace + "comment_id").Value);
            Author = commentElement.Element(wpNamespace + "comment_author").Value;
            AuthorEmail = commentElement.Element(wpNamespace + "comment_author_email").Value;
            AuthorUrl = commentElement.Element(wpNamespace + "comment_author_url").Value;
            AuthorIp = commentElement.Element(wpNamespace + "comment_author_IP").Value;
            Date = DateTime.Parse(commentElement.Element(wpNamespace + "comment_date").Value);
            DateGmt = DateTime.Parse(commentElement.Element(wpNamespace + "comment_date_gmt").Value);
            Content = commentElement.Element(wpNamespace + "comment_content").Value;
            Approved = commentElement.Element(wpNamespace + "comment_approved").Value;
            Type = commentElement.Element(wpNamespace + "comment_type").Value;
            Parent = int.Parse(commentElement.Element(wpNamespace + "comment_parent").Value);
            UserId = int.Parse(commentElement.Element(wpNamespace + "comment_user_id").Value);
        }
    }
}
