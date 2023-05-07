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

    [BindProperty] public int ThisUserId { get; set; }
    [BindProperty] public List<Product> Product { get; set; } = default!;
    public string DisplayMessage { get; set; } = string.Empty;

    public IndexModel(DatabaseContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Get the list of products from Admin User to show to the
    // current user. Set "ThisUserId" to id for routing
    public void OnGet(int id)
    {
        SetProperties(id);
    }

    // This method adds the selected product to the user's cart
    public IActionResult OnPostAddToCart(int ProdId, int UserId)
    {
        // Get the current product and the current user
        Product CurrentProduct = _context.Product.Where(i => i.ProductId == ProdId).SingleOrDefault()!;
        User ThisUser = _context.User.Where(i => i.UserId == UserId).SingleOrDefault()!;

        // Create a new Product and attach it to the current user
        Product newProduct = new Product()
        {
            ProductName = CurrentProduct.ProductName,
            ProductPrice = CurrentProduct.ProductPrice,
            User = ThisUser
        };

        // Update the database and the message text
        _context.Add(newProduct);
        DisplayMessage = newProduct.ProductName + " has been added to the cart.";
        _context.SaveChanges();

        // Set the properties again and return back to this page
        SetProperties(UserId);
        return Page();
    }

    // This method removes the selected product from the user's cart
    public IActionResult OnPostRemoveFromCart(int ProdId, int UserId)
    {
        // Get the product the user wants to remove
        Product AdminProduct = _context.Product.Where(i => i.ProductId == ProdId).SingleOrDefault()!;
        Product UserProduct = _context.Product.Where(n => n.ProductName == AdminProduct.ProductName) // Product name
                                              .Where(i => i.UserId == UserId).FirstOrDefault()!; // User associated

        // Remove the product only if the product is in the user's list.
        if (UserProduct != null)
        {
            _context.Remove(UserProduct);
            DisplayMessage = AdminProduct.ProductName + " has been removed from the cart.";
            _context.SaveChanges();
        }
        else
            DisplayMessage = "This product is not in your cart and cannot be removed.";

        // Set the properties again and return back to this page
        SetProperties(UserId);
        return Page();
    }

    // This function sets ThisUser and Product properties to prevent from being null when the page returns
    public void SetProperties(int userId)
    {
        // Get the admin user who holds all the available products
        User AdminUser = _context.User.Where(a => a.UserName == "admin").SingleOrDefault()!;
        Product = _context.Product.Where(i => i.UserId == AdminUser.UserId).ToList(); // Get the Products

        // Set the UserId for this webpage
        ThisUserId = userId;
    }
}