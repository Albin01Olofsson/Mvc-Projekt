using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ProjektApp.Viewmodels;


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
            var userId = _userManager.GetUserId(User);

            var projects = _context.Projects
                .Include(p => p.ProjectMembers)
                .Select(p => new ProjectListViewModel
                {
                    ProjectId = p.ProjectId,
                    Title = p.Title,
                    Description = p.Description,
                    MemberCount = p.ProjectMembers.Count,
                    IsCurrentUserMember = userId != null &&
                        p.ProjectMembers.Any(pm => pm.UserId == userId)
                })
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Leave(int projectId)
        {
            var userId = _userManager.GetUserId(User);

            var membership = await _context.ProjectMembers
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == projectId &&
                    pm.UserId == userId);

            if (membership != null)
            {
                _context.ProjectMembers.Remove(membership);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
