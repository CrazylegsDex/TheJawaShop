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
    public class OrderDetails : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public OrderDetails(TheJawaShop.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Order> TheOrder { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check if the user has any orders to view
            User TheUser = _context.User.Where(i => i.UserId == id).SingleOrDefault()!;
            if (TheUser.Orders.Count() == 0)
            {
                // Empty orders, redirect back to home
                return RedirectToPage("./ManageUser");
            }

            if (_context.Order != null && _context.ProductOrder != null)
            {
                TheOrder = await _context.Order.Include(o => o.ProductOrders)
                                .ThenInclude(p => p.Product).Where(i => i.UserId == id).ToListAsync();
            }

            return Page();
        }
    }
}