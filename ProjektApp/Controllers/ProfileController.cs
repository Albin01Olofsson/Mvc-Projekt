using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using ProjektApp.Viewmodels;

namespace ProjektApp.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize]
        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
