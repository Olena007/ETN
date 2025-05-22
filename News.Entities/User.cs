using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace News.Entities
{
    public class User
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserSurname { get; set; }
        public string? UserRole { get; set; }
        public string? UserEmail { get; set; } = null!;
        [JsonIgnore]
        public string? Password { get; set; } = null!;
        public int? Level { get; set; }
        public ICollection<View> Views { get; set; }
    }
}
