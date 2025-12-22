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

        public IActionResult Index()
        {
            var publicProfiles = _context.Profile
                .Where(p => !p.IsPrivate)
                .ToList();

            var cvs = _context.CVs
                .Join(
                    _context.Profile,
                    cv => cv.ProfileId,
                    profile => profile.ProfileId,
                    (cv, profile) => new { cv, profile }
                )
                .Where(x => !x.profile.IsPrivate)
                .Select(x => x.cv)
                .ToList();

            var vm = new HomeViewModel
            {
                PublicProfiles = publicProfiles,
                CVs = cvs,
                LatestProjects = _context.Projects
            .OrderByDescending(p => p.ProjectId) 
            .Take(3)
            .ToList()
            };

            return View(vm);
        }
    }

    
}
