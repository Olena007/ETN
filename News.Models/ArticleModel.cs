using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class ArticleModel
    {
        public string Title {  get; set; }
        public string Link { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Creator { get; set; }
        public string VideoUrl  { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime PubDate { get; set; }
        public string FullDescription { get; set; }
        public string ImageUrl { get; set; }
        public string SourceId { get; set; }
    }
}
