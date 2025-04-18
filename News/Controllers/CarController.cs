using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static News.BusinessLogic.Cars.CreateCar;
using static News.BusinessLogic.Cars.DeleteCar;
using static News.BusinessLogic.Cars.GetCar;
using static News.BusinessLogic.Cars.GetCars;
using static News.BusinessLogic.Cars.UpdateCar;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CarController : BaseController
    {
        private readonly IMapper _mapper;

        public CarController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<CarsVm>> GetAll(GetCarsQuery query)
        {
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarVm>> Get(Guid id)
        {
            var query = new GetCarQuery
            {
                CarId = id
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCarCommandDto commandDto)
        {
            var command = _mapper.Map<CreateCarCommand>(commandDto);
            var vm = await Mediator.Send(command);
            return Ok(vm);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCarCommand
            {
                Id = id,
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCarCommandDto commandDto)
        {
            var command = _mapper.Map<UpdateCarCommand>(commandDto);
            var rm = await Mediator.Send(command);
            return NoContent();
        }
    }
}
