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
        var jwt = token.GenerateUserToken(client.Email);

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, client.PasswordHash))
            return BadRequest(new { message = "Wrong password" });
        
        Response.Cookies.Append("jwt", jwt, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            Expires = DateTime.Now.AddDays(2) 
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
            return Ok(client);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }
    
    [HttpGet]
    public IActionResult CheckAuth()
    {
        var tokenResult = Request.Cookies["jwt"]; 
        var isAuthenticated = !string.IsNullOrEmpty(tokenResult);
        return Ok(new { isAuthenticated });
    }
    
    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt", new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            Path = "/"
        }); 
    
        return Ok(new { message = "logged out" });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommandDto commandDto)
    {
        var command = mapper.Map<UpdateUserCommand>(commandDto);
        await Mediator.Send(command);
        return NoContent();
    }
}