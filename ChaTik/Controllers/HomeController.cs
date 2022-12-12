using Microsoft.AspNetCore.Mvc;

namespace ChaTik.Controllers;

public class HomeController : Controller
{
    public string Index()
    {
        return "Hello world";
    }
}