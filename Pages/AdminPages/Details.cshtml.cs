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
    public class DetailsModel : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public DetailsModel(TheJawaShop.Models.DatabaseContext context)
        {
            _context = context;
        }

      public Product Product { get; set; } = default!;

      public int AdminUserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? ProdId, int AdminId)
        {
            if (ProdId == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductId == ProdId);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }

            // Set the AdminUserId
            AdminUserId = AdminId;
            
            return Page();
        }
    }
}
