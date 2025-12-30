using DAL;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjektApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly Context _context;

        public ProjectController(Context context)
        {
            _context = context;
        }
        

        public IActionResult Index()
        {
            var projects = _context.Projects
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.User)
                .ToList();
            return View(projects);
        }
    }
}
