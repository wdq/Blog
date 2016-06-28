using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordpressImporter.WordPressModel
{
    class wpCategory
    {
        public string Slug { get; set; } // In the nicename attribute of the XML
        public string Name { get; set; } // In the CDATA part of the XML
    }
}
