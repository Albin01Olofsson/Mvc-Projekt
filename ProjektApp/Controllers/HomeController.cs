   using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ProjektApp.Viewmodels;
using System.Diagnostics;

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

        public IActionResult Index(string search)
        {
          
            var profilesQuery = _context.Profile.AsQueryable();

            //  Om användaren INTE är inloggad ? visa bara publika profiler
            if (!User.Identity.IsAuthenticated)
            {
                profilesQuery = profilesQuery.Where(p => !p.IsPrivate);
            }

            //  Sök på FullName (om något är inskrivet)
            if (!string.IsNullOrWhiteSpace(search))
            {
                profilesQuery = profilesQuery
                    .Where(p => p.FullName.Contains(search));
            }

            //  Sortera alfabetiskt
            profilesQuery = profilesQuery
                .OrderBy(p => p.FullName);

            // CVs (respekterar privat/offentlig på samma sätt)
            var cvsQuery = _context.CVs
    .Include(cv => cv.Profile)
    .AsQueryable();

            //  Om användaren INTE är inloggad ? filtrera bort privata CV
            if (!User.Identity.IsAuthenticated)
            {
                cvsQuery = cvsQuery.Where(cv => !cv.Profile.IsPrivate);
            }

            var vm = new HomeViewModel
            {
                PublicProfiles = profilesQuery.ToList(),
                CVs = cvsQuery.ToList(),
                LatestProjects = _context.Projects
                    .OrderByDescending(p => p.ProjectId)
                    .Take(3)
                    .ToList()
            };

            ViewBag.Search = search;
            return View(vm);
        }
    }

}