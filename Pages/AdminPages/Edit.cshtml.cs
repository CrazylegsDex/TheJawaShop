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
    public class EditModel : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public EditModel(TheJawaShop.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public int AdminUserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? ProdId, int AdminId)
        {
            if (ProdId == null || _context.Product == null)
            {
                return NotFound();
            }

            var product =  await _context.Product.FirstOrDefaultAsync(m => m.ProductId == ProdId);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            
            // Set the AdminUserId
            AdminUserId = AdminId;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Set the Foreign key to the admin user
            User adminUser = _context.User.Where(n => n.UserName == "admin").SingleOrDefault()!;
            Product.UserId = adminUser.UserId;
            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { id = adminUser.UserId});
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
