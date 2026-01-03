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
                        p.ProjectMembers.Any(pm => pm.UserId == userId),
                    IsOwner = userId != null && p.OwnerId == userId
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

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var project = new Project
            {
                Title = model.Title,
                Description = model.Description,
                OwnerId = user.Id
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var membership = new ProjectMember
            {
                ProjectId = project.ProjectId,
                UserId = user.Id
            };

            _context.ProjectMembers.Add(membership);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = project.ProjectId });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            if (project.OwnerId != userId)
            {
                return Forbid();
            }

            var vm = new EditProjectViewModel
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                Description = project.Description
            };
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = _userManager.GetUserId(User);

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == model.ProjectId);

            if (project == null)
            {
                return NotFound();
            }

            if (project.OwnerId != userId)
            {
                return Forbid();
            }
            project.Title = model.Title;
            project.Description = model.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = project.ProjectId });
        }
    }
}
