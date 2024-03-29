using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheJawaShop.Models;

namespace TheJawaShop.Pages.AdminPages
{
    public class DeleteUser : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public DeleteUser(TheJawaShop.Models.DatabaseContext context)
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

            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == ThisUserId);

            if (user == null)
            {
                return NotFound();
            }
            else 
            {
                TheUser = user;
                AdminUserId = AdminId;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);

            if (user != null)
            {
                TheUser = user;
                _context.User.Remove(TheUser);
                await _context.SaveChangesAsync();
            }

            User adminUser = _context.User.Where(i => i.UserName == "admin").SingleOrDefault()!;
            return RedirectToPage("./ManageUser", new { id = adminUser.UserId });
        }
    }
}
