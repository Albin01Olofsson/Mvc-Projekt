using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DAL;
using Models;

namespace ProjektApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _Context;

        public HomeController(Context context)
        {
            _Context = context;
        }

        public IActionResult Index()
        { 
            List<Project> projects = _Context.Projects.ToList();
            ViewBag.Projects = "Produkter";
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
