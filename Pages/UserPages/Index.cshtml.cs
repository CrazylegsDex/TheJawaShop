using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheJawaShop.Models;

namespace TheJawaShop.Pages.UserPages;

public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;
    private readonly ILogger<IndexModel> _logger;

    public List<Product> Product { get; set; } = default!;

    public int ThisUserId { get; set; }

    public IndexModel(DatabaseContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Get the list of products from Admin User to show to the
    // current user. Set "ThisUserId" to id for routing
    public void OnGet(int id)
    {
        // Get the admin user who holds all the available products
        User AdminUser = _context.User.Where(a => a.UserName == "admin").SingleOrDefault()!;
        Product = _context.Product.Where(i => i.UserId == AdminUser.UserId).ToList(); // Get the Products

        // Set the UserId for this webpage
        ThisUserId = id;
    }

    public IActionResult OnPostAddToCart(int ProdId, int UserId)
    {
        _logger.LogWarning("Works");

        // Returns back to this page. Ensures the current UserId is still set to the page.
        return RedirectToPage("./Index", new {id = UserId});
    }
}