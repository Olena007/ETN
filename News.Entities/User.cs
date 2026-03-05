
using News.Enums;

namespace News.Entities
{
    public class User : BaseEntity
    {

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;
    }
}
