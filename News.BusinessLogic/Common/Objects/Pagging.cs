using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.BusinessLogic.Common.Objects
{
    public class Pagging
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 12;
    }
}
