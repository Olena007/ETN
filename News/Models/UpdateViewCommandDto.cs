using AutoMapper;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.View;

namespace WebApi.Models;

public class UpdateViewCommandDto: IMapWith<UpdateViewCommand>
{
    public Guid? ViewId { get; set; }
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateViewCommandDto, UpdateViewCommand>();
    }
}