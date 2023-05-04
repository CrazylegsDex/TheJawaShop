using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheJawaShop.Models;

namespace TheJawaShop.Pages;

public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public User TheUser { get; set; } = default!;
    public string OutputMessage { get; set; } = String.Empty;

    public IndexModel(DatabaseContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Empty OnGet method
    public void OnGet() { }

    // This OnPost method will login the user if they are found in
    // the database. If they are not found, the page will reload with
    // an error message.
    public IActionResult OnPostLoginUser()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Check the input username and password with that in the database
        User validUser = _context.User.FirstOrDefault(n => n.UserName == TheUser.UserName && n.UserPassword == TheUser.UserPassword)!;

        // If the user was not found in the database
        if (validUser == null)
        {
            OutputMessage = "The username or password is incorrect. Please try again.";
            return Page();
        }
        
        // The user exists in the database, check if they logged in
        // as admin or normal user
        if (validUser.UserName == "admin") // Admin user login
            return RedirectToPage("./AdminPages/Index");
        else // Any other user
            return NotFound();
    }

    // This OnPost method will add the user to the database
    // If the user is already in the database, this method
    // will return and ask the user to click Login instead
    public IActionResult OnPostCreateUser()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Double check that this is a new user to add to the database.
        User validUser = _context.User.FirstOrDefault(n => n.UserName == TheUser.UserName)!;

        // Add the new user to the database and login
        if (validUser == null)
        {
            _context.User.Add(TheUser);
            _context.SaveChanges();
            OutputMessage = "You have been added to the database. Please login to shop.";
            return Page();
        }
        else // User exists, return them to the login page
        {
            OutputMessage = "You already have an account at The Jawa Shop. Please choose login instead.";
            return Page();
        }
    }
}