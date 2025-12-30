using DAL;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace ProjektApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public ProjectController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        

        public IActionResult Index()
        {
            var projects = _context.Projects
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.User)
                .ToList();
            return View(projects);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Join(int projectId)
        {
            var userId = _userManager.GetUserId(User);

            bool alreadyMember = _context.ProjectMembers
            .Any(pm => pm.ProjectId == projectId && pm.UserId == userId);

            if (!alreadyMember)
            {
                var membership = new ProjectMember
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                _context.ProjectMembers.Add(membership);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
