using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheJawaShop.Models;

namespace TheJawaShop.Pages.AdminPages
{
    public class EditUser : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public EditUser(TheJawaShop.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User TheUser { get; set; } = default!;

        public int AdminUserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int ThisUserId, int AdminId)
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            var user =  await _context.User.FirstOrDefaultAsync(m => m.UserId == ThisUserId);
            if (user == null)
            {
                return NotFound();
            }

            // Set This user and the AdminUserId
            TheUser = user;
            AdminUserId = AdminId;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(TheUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(TheUser.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            // Get the AdminUserId to send back
            User admin = _context.User.Where(n => n.UserName == "admin").FirstOrDefault()!;
            return RedirectToPage("./ManageUser", new { id = admin.UserId });
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
