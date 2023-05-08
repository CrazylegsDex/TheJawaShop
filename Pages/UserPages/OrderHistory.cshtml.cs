using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheJawaShop.Models;

namespace TheJawaShop.Pages.UserPages;

public class OrderHistory : PageModel
{
    private readonly DatabaseContext _context;
    private readonly ILogger<OrderHistory> _logger;

    public List<Order> TheOrder { get; set; } = default!;

    public int ThisUserId { get; set; }

    public OrderHistory(DatabaseContext context, ILogger<OrderHistory> logger)
    {
        _context = context;
        _logger = logger;
    }

    // This method gets the user and their orders to display
    // to the webpage
    public IActionResult OnGet(int id)
    {
        // Check if the user has any orders to view
        User TheUser = _context.User.Where(i => i.UserId == id).Include(o => o.Orders).SingleOrDefault()!;
        if (TheUser.Orders.Count() == 0)
        {
            // Empty orders, redirect back to home
            return RedirectToPage("./Index", new { id = TheUser.UserId, SearchItem = string.Empty });
        }

        // Assign the user's order from the database
        TheOrder = _context.Order.Include(o => o.ProductOrders)
                    .ThenInclude(p => p.Product).Where(i => i.UserId == id).ToList();

        // Assign this user's id
        ThisUserId = id;

        return Page();
    }
}