using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordpressImporter.WordPressModel
{
    class wpPost
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
        public wpAuthor Creator { get; set; }
        public string Guid { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public int PostId { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime PostDateGmt { get; set; }
        public bool CommentStatus { get; set; }
        public string PostName { get; set; }
        public string Status { get; set; }
        public int PostParent { get; set; }
        public int MenuOrder { get; set; }
        public string PostType { get; set; }
        public string PostPassword { get; set; }
        public bool IsSticky { get; set; }
        public List<wpCategory> Categories { get; set; }
        public List<wpTag> Tags { get; set; }
        public List<wpComment> Comments { get; set; }
    }
}
