using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Token;
using News.Enums;
using WebApi.Models;
using static News.BusinessLogic.Users.GetUsers;
using static News.BusinessLogic.Users.GetUser;
using static News.BusinessLogic.Users.UpdateUser;
using static News.BusinessLogic.Users.CreateUser;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class UserController(IMapper mapper, Token token) : BaseController
{
    [HttpPost]
    public async Task<ActionResult<UsersVm>> GetAll(GetUsersQuery query)
    {
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterDto commandDto)
    {
        var command = mapper.Map<CreateUserCommand>(commandDto);
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult<UserVm>> Login(LoginDto dto)
    {
        var query = new GetUserQueryByEmail
        {
            UserEmail = dto.Email
        };
        var client = await Mediator.Send(query);
        string jwt;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, client.PasswordHash))
            return BadRequest(new { message = "Wrong password" });

        if (client.Role == UserRole.User)
            jwt = token.GenerateUserToken(client.Email);
        else
            jwt = token.GenerateAdminToken(client.Email);

        Response.Cookies.Append("jwt", jwt, new CookieOptions
        {
            HttpOnly = true
        });

        return Ok(jwt);
    }

    [HttpGet]
    public async Task<ActionResult<UserVm>> Client()
    {
        try
        {
            var jwt = Request.Cookies["jwt"];

            var token1 = token.Verify(jwt ?? string.Empty);

            var email = token1.Claims
                .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var query = new GetUserQueryByEmail
            {
                UserEmail = email
            };
            var client = await Mediator.Send(query);
            var queryId = new GetUserQuery
            {
                UserId = client.Id
            };

            return Ok(queryId);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    [HttpGet]
    public async Task<ActionResult<UserVm>> GetRole()
    {
        try
        {
            var jwt = Request.Cookies["jwt"];

            var token1 = token.Verify(jwt!);

            var email = token1.Claims
                .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var query = new GetUserQueryByEmail
            {
                UserEmail = email
            };
            var client = await Mediator.Send(query);
            var queryId = new GetUserQuery
            {
                UserId = client.Id
            };
            var clientRole = await Mediator.Send(queryId);

            var role = clientRole.Role;
            return Ok(role);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");

        return Ok(new
        {
            message = "logouted"
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommandDto commandDto)
    {
        var command = mapper.Map<UpdateUserCommand>(commandDto);
        await Mediator.Send(command);
        return NoContent();
    }
}