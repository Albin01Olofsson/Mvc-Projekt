using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ProjektApp.Viewmodels;

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


        // KRAV 1 – SKICKA


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Send(int receiverProfileId)
        {
            var profileExists = await _context.Profile
                .AnyAsync(p => p.ProfileId == receiverProfileId);

            if (!profileExists)
                return NotFound();

            return View(new SendMessageViewModel
            {
                ReceiverProfileId = receiverProfileId
            });
        }





        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Send(SendMessageViewModel model)
        {

            var receiverProfile = await _context.Profile
        .Include(p => p.User)
        .FirstOrDefaultAsync(p => p.ProfileId == model.ReceiverProfileId);

            if (receiverProfile == null)
            {
                return NotFound("Mottagaren kunde inte hittas.");


            }

            if (!User.Identity.IsAuthenticated &&
        string.IsNullOrWhiteSpace(model.SenderName))
            {
                ModelState.AddModelError(
                    nameof(model.SenderName),
                    "Du måste ange ditt namn när du inte är inloggad."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var message = new Message
            {
                Content = model.Content,
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
                message.SenderName = model.SenderName;
            }

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Profile",
                new { id = receiverProfile.UserId }
            );
        }


        // KRAV 2 – INKORG

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


        // KRAV 2 – LÄS

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id);

            if (message == null)
                return NotFound();

            // säkerhetskontroll
            if (message.ReceiverId != userId)
                return Forbid();

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Inbox");
        }
    }
}
