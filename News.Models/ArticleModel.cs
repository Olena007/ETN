using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class ArticleModel
    {
        public string Article {  get; set; }
        public string Heading { get; set; }
        public DateTime Date { get; set; }
        public string NewsType { get; set; }
    }
}
