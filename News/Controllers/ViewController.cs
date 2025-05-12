using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Cars;
using News.BusinessLogic.View;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class ViewController(IMapper mapper) : BaseController
{
    [HttpPost]
    public async Task<ActionResult<ViewsVm>> GetAll(GetViewsQuery query) 
        =>  Ok(await Mediator.Send(query));
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ViewVm>> Get(Guid id)
    {
        var query = new GetViewQuery
        {
            ViewId = id
        };
        
        return Ok(await Mediator.Send(query));
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateViewCommandDto commandDto)
    {
        var command = mapper.Map<CreateViewCommand>(commandDto);
        return Ok(await Mediator.Send(command));
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteViewCommand
        {
            Id = id,
        };
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateViewCommandDto commandDto)
    {
        var command = mapper.Map<UpdateViewCommand>(commandDto);
        await Mediator.Send(command);
        return NoContent();
    }
}