using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheJawaShop.Models;

namespace TheJawaShop.Pages.UserPages;

public class Checkout : PageModel
{
    private readonly DatabaseContext _context;
    private readonly ILogger<Checkout> _logger;

    [Required]
    [BindProperty]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    [CreditCard]
    public string CardNumber { get; set; } = string.Empty;

    [BindProperty] public User TheUser { get; set; } = default!;

    public Checkout(DatabaseContext context, ILogger<Checkout> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult OnGet(int id)
    {
        // Assign TheUser the user's id and products
        TheUser = _context.User.Where(i => i.UserId == id).Include(p => p.Products).SingleOrDefault()!;

        // Check for items in the User's cart. Return if empty and send the user's id to index
        if (TheUser.Products.Count() == 0)
        {
            return RedirectToPage("./Index", new { id = TheUser.UserId });
        }

        return Page();
    }

    // This method cancel's the user's checkout action (returns to index).
    // This method will not clear/cancel the user's cart.
    // Removal of cart items is done on the index page only
    public IActionResult OnPostCancel(int id)
    {
        // Re-assign user for ID back to index
        TheUser = _context.User.Where(i => i.UserId == id).SingleOrDefault()!;
        return RedirectToPage("./Index", new { id = TheUser.UserId });
    }

    // This method will "purchase" the user's items. This method will
    // clear the user's cart and add the products to an order, which is
    // also added to a ProductOrder.
    public IActionResult OnPostPurchase(int id)
    {
        // Create a list of ProductIds to hold the ProductIds the User is purchasing
        // Get the Admin's and the current User's Products. Use the lists to assign the
        // Admin User's ProductIds to the ProductIds list.
        List<int> productIds = new List<int>();
        User adminProducts = _context.User.Where(n => n.UserName == "admin").Include(p => p.Products).SingleOrDefault()!;
        TheUser = _context.User.Where(i => i.UserId == id).Include(p => p.Products).SingleOrDefault()!;

        // Get the Total Cost and Date to add to Order
        // Assign the productIds list
        decimal TotalCost = 0;
        DateTime currentDateTime = DateTime.Now;
        for (int i = 0; i < TheUser.Products.Count(); ++i) // Loop through all the User's products
        {
            // Add up the current products price
            TotalCost += TheUser.Products[i].ProductPrice;

            // Get the current products id in the adminProducts id table and assing to productIds
            foreach (Product item in adminProducts.Products)
            {
                if (TheUser.Products[i].ProductName == item.ProductName)
                    productIds.Add(item.ProductId);
            }
        }

        // Create an order containing the order information
        Order newOrder = new Order()
        {
            OrderDate = currentDateTime,
            OrderPrice = TotalCost,
            UserId = id
        };

        // Add the Order to the Database and save the changes
        _context.Add(newOrder);
        _context.SaveChanges();

        // Get the updated Id of newOrder from the database
        newOrder = _context.Order.Where(d => d.OrderDate == currentDateTime).SingleOrDefault()!;

        // Create an empty list of ProductOrder
        List<ProductOrder> newProductOrder = new List<ProductOrder>();
        foreach (int itemId in productIds) // Loop through each item in the productIds list
        {
            // For each product, add a new ProductOrder in the list
            newProductOrder.Add
            (
                new ProductOrder
                {
                    OrderId = newOrder.OrderId, // All orders get the same Order number
                    ProductId = itemId, // Each product gets the ProductId in productIds
                }
            );
        }

        // Clear the products from the user and update the database with ProductOrder
        _context.AddRange(newProductOrder);
        TheUser.Products.Clear();
        _context.SaveChanges();

        return RedirectToPage("./Index", new { id = TheUser.UserId });
    }
}