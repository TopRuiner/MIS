using Microsoft.AspNetCore.Mvc;
using Polyclinic.Models;
using System.Diagnostics;

namespace Polyclinic.Controllers
{

    public class HomeController : Controller
    {
        //Контекст для получения данных сессии
        private readonly IHttpContextAccessor _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}