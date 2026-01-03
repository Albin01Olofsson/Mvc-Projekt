using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ProjektApp.Viewmodels;

namespace ProjektApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Context _context;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("login", "account");
            }

            var profile = await _context.Profile
        .Include(p => p.CV)
        .Include(p => p.User)
            .ThenInclude(u => u.ProjectMembers)
                .ThenInclude(pm => pm.Project)
        .FirstOrDefaultAsync(p => p.UserId == user.Id);

            ViewBag.UserEmail = user.Email;

            return View(profile);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.Profile
                .Include(p => p.CV)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return View(new EditProfileViewModel());
            }
            var vm = new EditProfileViewModel
            {
                FullName = profile.FullName,
                Bio = profile.Bio,
                IsPrivate = profile.IsPrivate,
            };


            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.Profile.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (profile == null)
            {
                profile = new Profile
                {
                    UserId = user.Id
                };
                _context.Profile.Add(profile);
            }

            profile.FullName = model.FullName;
            profile.Bio = model.Bio;
            profile.IsPrivate = model.IsPrivate;

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads", "profiles");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                profile.PictureUrl = "/uploads/profiles/" + fileName;
            }



            await _context.SaveChangesAsync();

            return RedirectToAction("MyProfile");

        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var profile = await _context.Profile
                .Include(p => p.CV)
             .Include(p => p.User)
            .ThenInclude(u => u.ProjectMembers)
                .ThenInclude(pm => pm.Project)
             .FirstOrDefaultAsync(p => p.UserId == id);

            if (profile == null)
            {
                return NotFound();
            }

            if (profile.IsPrivate && !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserId = _userManager.GetUserId(User);

            if (User.Identity.IsAuthenticated && id == currentUserId)
            {
                return RedirectToAction("MyProfile");
            }
            return View(profile);
        }

        public IActionResult Index(string search)
        {
            var profilesQuery = _context.Profile.AsQueryable();

            //Inte inloggad bara publika profiler
            if (!User.Identity.IsAuthenticated)
            {
                profilesQuery = profilesQuery.Where(p => !p.IsPrivate);
            }

            //Sök på namn
            if (!string.IsNullOrWhiteSpace(search))
            {
                profilesQuery = profilesQuery
                    .Where(p => p.FullName.Contains(search));
            }

            profilesQuery = profilesQuery.OrderBy(p => p.FullName);

            var profiles = profilesQuery.ToList();

            ViewBag.Search = search;
            return View(profiles);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(
             user,
             model.CurrentPassword,
             model.NewPassword
             );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            //  uppdaterar inloggningen
            await _signInManager.RefreshSignInAsync(user);

            TempData["Success"] = "Lösenordet har ändrats.";
            return RedirectToAction("MyProfile");
        }

    }
}
