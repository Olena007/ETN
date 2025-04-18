using News.Entities;
using MediatR;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Cars
{
    public class CreateCar
    {
        public class CreateCarCommand : IRequest<Guid>
        {
            public string? Brand { get; set; }
            public string? Model { get; set; }
            public string? LicensePlate { get; set; }
            public int? YearOfIssue { get; set; }
            public bool? IsAvailable { get; set; }
            public string? Image { get; set; }
            public int? Level { get; set; }
        }

        public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, Guid>
        {
            private readonly INewsDbContext _context;
            public CreateCarCommandHandler(INewsDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(CreateCarCommand request, CancellationToken cancellationToken)
            {
                var entity = new Car
                {
                    CarId = Guid.NewGuid(),
                    Brand = request.Brand,
                    Model = request.Model,
                    LicensePlate = request.LicensePlate,
                    YearOfIssue = request.YearOfIssue,
                    IsAvailable = request.IsAvailable,
                    Image = request.Image,
                    Level = request.Level
                };

                await _context.Cars.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return (Guid)entity.CarId;
            }
        }
    }
}
