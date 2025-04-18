using News.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Cars
{
    public class DeleteCar
    {
        public class DeleteCarCommand : IRequest
        {
            public Guid Id { get; set; }
        }

        public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand>
        {

            private readonly INewsDbContext _context;

            public DeleteCarCommandHandler(INewsDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Cars
                    .FirstOrDefaultAsync(x => x.CarId == request.Id);

                if (entity == null)
                    throw new NotFoundException(nameof(Car), request.Id);

                _context.Cars.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
