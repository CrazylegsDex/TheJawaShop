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
    [StringLength (maximumLength: 60, MinimumLength = 3)]
    [DataType (DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    [DataType (DataType.CreditCard)]
    public string CardNumber { get; set; } = string.Empty;

    public User TheUser { get; set; } = default!;

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

    // This method cancel's the user's purchase (returns to index).
    // This method will not clear the user's cart.
    public IActionResult OnPostCancel(int id)
    {
        _logger.LogWarning("YAY!");
        return RedirectToPage("./Index", new { id = TheUser.UserId });
    }

    // This method will "purchase" the user's items. This method will
    // clear the user's cart and add the products to an order.
    public IActionResult OnPostPurchase(int id)
    {
        _logger.LogWarning("Works");
        return RedirectToPage("./Index", new { id = TheUser.UserId });
    }
}