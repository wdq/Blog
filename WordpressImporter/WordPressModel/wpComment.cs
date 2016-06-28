using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordpressImporter.WordPressModel
{
    class wpComment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorUrl { get; set; }
        public string AuthorIp { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateGmt { get; set; }
        public string Content { get; set; }
        public bool Approved { get; set; }
        public string Type { get; set; }
        public int Parent { get; set; }
        public int UserId { get; set; }
    }
}
