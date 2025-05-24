using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Token;
using WebApi.Models;
using static News.BusinessLogic.Users.GetUsers;
using static News.BusinessLogic.Users.GetUser;
using static News.BusinessLogic.Users.UpdateUser;
using static News.BusinessLogic.Users.CreateUser;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class UserController : BaseController
{
    private readonly IMapper _mapper;
    private readonly Token _token;


    public UserController(IMapper mapper, Token token)
    {
        _mapper = mapper;
        _token = token;
    }

    [HttpPost]
    public async Task<ActionResult<UsersVm>> GetAll(GetUsersQuery query)
    {
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Register([FromBody] CreateUserCommandDto commandDto)
    {
        // var user = new User() { UserName = commandDto.UserName, UserSurname = commandDto.UserSurname, UserRole = "Admin", UserEmail = commandDto.UserEmail, Password = BCrypt.Net.BCrypt.HashPassword(commandDto.Password), Level = commandDto.Level };
        var command = _mapper.Map<CreateUserCommand>(commandDto);
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

        if (client == null) return BadRequest(new { message = "Invalid Operation" });

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, client.Password))
            return BadRequest(new { message = "Wrong password" });

        if (client.UserRole == "User")
            jwt = _token.GenerateUserToken(client.UserEmail);
        else
            jwt = _token.GenerateAdminToken(client.UserEmail);

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

            var token = _token.Verify(jwt);

            var email = token.Claims
                .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var query = new GetUserQueryByEmail
            {
                UserEmail = email
            };
            var client = await Mediator.Send(query);
            var queryId = new GetUserQuery
            {
                UserId = (Guid)client.UserId
            };

            return Ok(queryId);
        }
        catch (Exception ex)
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

            var token = _token.Verify(jwt);

            var email = token.Claims
                .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            var query = new GetUserQueryByEmail
            {
                UserEmail = email
            };
            var client = await Mediator.Send(query);
            var queryId = new GetUserQuery
            {
                UserId = (Guid)client.UserId
            };
            var clientRole = await Mediator.Send(queryId);

            var role = clientRole.UserRole;
            return Ok(role);
        }
        catch (Exception ex)
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
        var command = _mapper.Map<UpdateUserCommand>(commandDto);
        var rm = await Mediator.Send(command);
        return NoContent();
    }
}