using News.BusinessLogic.Common.Mappings;
using AutoMapper;
using static News.BusinessLogic.Cars.UpdateCar;

namespace WebApi.Models
{
    public class UpdateCarCommandDto: IMapWith<UpdateCarCommand>
    {
        public Guid? CarId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? LicensePlate { get; set; }
        public int? YearOfIssue { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Image { get; set; }
        public int? Level { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCarCommandDto, UpdateCarCommand>();
        }
    }
}
