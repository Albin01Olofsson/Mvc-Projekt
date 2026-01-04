using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public MessageController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ======================
        // KRAV 1 – SKICKA
        // ======================
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Send(
            int receiverProfileId,
            string content,
            string senderName)
        {
            var userId = await _userManager.GetUserAsync(User);

            if (string.IsNullOrWhiteSpace(content))
                return RedirectToAction("Details", "CV", new { id = receiverProfileId });

            var receiverProfile = await _context.Profile
                .FirstOrDefaultAsync(p => p.ProfileId == receiverProfileId);

            if (receiverProfile == null)
                return NotFound();

            var message = new Message
            {
                Content = content,
                SentAt = DateTime.Now,
                ReceiverId = receiverProfile.UserId,
                IsRead = false
            };

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                message.SenderId = user.Id;
                message.SenderName = user.UserName;
            }
            else
            {
                message.SenderName = senderName;
            }

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "CV", new { id = receiverProfileId });
        }

        // ======================
        // KRAV 2 – INKORG
        // ======================
        [Authorize]
        public async Task<IActionResult> Inbox()
        {
            var userId = _userManager.GetUserId(User);

            var messages = await _context.Messages
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return View(messages);
        }

        // ======================
        // KRAV 2 – LÄS
        // ======================
        [Authorize]
        public async Task<IActionResult> Read(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (message.ReceiverId != userId)
                return Forbid();

            message.IsRead = true;
            await _context.SaveChangesAsync();

            return View(message);
        }
    }
}
