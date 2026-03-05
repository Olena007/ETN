using News.BusinessLogic.Common.Mappings;
using AutoMapper;
using News.Enums;
using static News.BusinessLogic.Users.CreateUser;

namespace WebApi.Models
{
    public class RegisterDto: IMapWith<CreateUserCommand>
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterDto, CreateUserCommand>();
        }
    }
}
