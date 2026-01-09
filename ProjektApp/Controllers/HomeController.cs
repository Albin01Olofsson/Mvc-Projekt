using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektApp.Viewmodels;

namespace ProjektApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;

            var vm = new HomeViewModel
            {
                PublicProfiles = _context.Profile
                    .Where(p => isAuthenticated || !p.IsPrivate)
                    .Take(8)
                    .ToList(),

                CVs = _context.CVs
                    .Include(cv => cv.Profile)
                    .Where(cv => isAuthenticated || !cv.Profile.IsPrivate)
                    .Take(6)
                    .ToList(),

                LatestProjects = _context.Projects
                    .OrderByDescending(p => p.ProjectId)
                    .Take(3)
                    .ToList()
            };

            return View(vm);
        }

    }
}