using AutoMapper;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.View;

namespace WebApi.Models;

public class CreateViewCommandDto : IMapWith<CreateViewCommand>
{
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateViewCommandDto, CreateViewCommand>();
    }
}