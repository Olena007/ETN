using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ViewController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}