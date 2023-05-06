using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheJawaShop.Models;

namespace TheJawaShop.Pages.AdminPages
{
    public class CreateModel : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public CreateModel(TheJawaShop.Models.DatabaseContext context)
        {
            _context = context;
        }

        public int AdminUserId { get; set; }

        // Set the AdminUserId and return
        public IActionResult OnGet(int id) { AdminUserId = id; return Page(); }

        [BindProperty]
        public Product Product { get; set; } = default!;
        
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (_context.Product == null || Product == null)
            {
                return Page();
            }

            // Set the Foreign key to the admin user
            User adminUser = _context.User.Where(n => n.UserName == "admin").SingleOrDefault()!;
            Product.UserId = adminUser.UserId;
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = adminUser.UserId });
        }
    }
}
