using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheJawaShop.Models;

namespace TheJawaShop.Pages.AdminPages;

public class CreateUser : PageModel
{
    private readonly DatabaseContext _context;
    private readonly ILogger<CreateUser> _logger;

    [BindProperty]
    public User TheUser { get; set; } = default!;

    public CreateUser(DatabaseContext context, ILogger<CreateUser> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Empty OnGet method
    public void OnGet() { }

    // This OnPost method will add the user to the database
    // If the user is already in the database, this method
    // will simply return
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Double check that this is a new user to add to the database.
        User validUser = _context.User.FirstOrDefault(n => n.UserName == TheUser.UserName)!;

        // Add the new user to the database
        if (validUser == null)
        {
            _context.User.Add(TheUser);
            _context.SaveChanges();
            return RedirectToPage("./ManageUser");
        }
        else // User exists. Simply return
        {
            return RedirectToPage("./ManageUser");
        }
    }
}