using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ProjektApp.Viewmodels;
using System.Threading.Tasks;
using System.IO;

namespace ProjektApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Context _context;

        public ProfileController(UserManager<User> userManager, Context context)
        {
            _userManager = userManager;
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

            var profile = await _context.Profile.FirstOrDefaultAsync(p => p.UserId == user.Id);
            ViewBag.UserEmail = user.Email;

            return View(profile);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.Profile.FirstOrDefaultAsync(p => p.UserId == user.Id);
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

            if(model.ProfileImage != null && model.ProfileImage.Length > 0)
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
            var profile = await _context.Profile.FirstOrDefaultAsync(p => p.UserId == id);
            if (profile == null)
            {
                return NotFound();
            }

            if(profile.IsPrivate && !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(profile);
        }


    }
}
