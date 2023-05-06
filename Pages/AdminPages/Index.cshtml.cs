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
    public class IndexModel : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public IndexModel(TheJawaShop.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public int AdminUserId { get; set; }

        public async Task OnGet(int id)
        {
            Product = await _context.Product.Where(i => i.UserId == id).ToListAsync();

            // Set the AdminUserId
            AdminUserId = id;
        }
    }
}
