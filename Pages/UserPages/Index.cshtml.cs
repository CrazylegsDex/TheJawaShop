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

    [BindProperty (SupportsGet = true)] public int ThisUserId { get; set; }
    [BindProperty (SupportsGet = true)] public int CurrentPageNum { get; set; } = 1;
    [BindProperty (SupportsGet = true)] public int TotalProducts { get; set; } = 0;
    [BindProperty (SupportsGet = true)] public string CurrentSort { get; set; } = string.Empty;
    [BindProperty (SupportsGet = true)] public string SearchString { get; set; } = string.Empty;
    [BindProperty] public List<Product> Product { get; set; } = default!;
    public int PageSize { get; set; } = 10;
    public string DisplayMessage { get; set; } = string.Empty;

    public IndexModel(DatabaseContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Set the SearchString variable from OnGet parameters.
    // Call SetProperties function to get the product list.
    public void OnGet(int id, string SearchItem)
    {
        SearchString = SearchItem;
        SetProperties(id);
    }

    // This method tests if the product the user is searching for is found in the database.
    // If the product is found, this method sets the SearchString variable. Else, this method
    // displays to the user the product was not found.
    public void OnPostFindProduct(int UserId, int PageNum, int Total, string SortValue, string SearchValue)
    {
        if (!String.IsNullOrEmpty(SearchValue))
        {
            // Get a query for all products in the database and see if the search value is in it
            var testQuery = _context.Product.Where(n => n.ProductName.Contains(SearchValue)
                            || n.ProductPrice.ToString().Contains(SearchValue));
            
            if (testQuery.Count() > 0)
                SearchString = SearchValue;
            else
            {
                SearchString = "";
                DisplayMessage = "Product could not be found.";
            }
        }

        // Set the list of products and url items again for return
        CurrentPageNum = PageNum;
        TotalProducts = Total;
        CurrentSort = SortValue;
        SetProperties(UserId);
    }

    // This method adds the selected product to the user's cart
    public void OnPostAddToCart(int ProdId, int UserId, int PageNum, int Total, string SortValue, string SearchValue)
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

        // Set the list of products and url items again for return
        CurrentPageNum = PageNum;
        TotalProducts = Total;
        CurrentSort = SortValue;
        SearchString = SearchValue;
        SetProperties(UserId);
    }

    // This method removes the selected product from the user's cart
    public void OnPostRemoveFromCart(int ProdId, int UserId, int PageNum, int Total, string SortValue, string SearchValue)
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

        // Set the list of products and url items again for return
        CurrentPageNum = PageNum;
        TotalProducts = Total;
        CurrentSort = SortValue;
        SearchString = SearchValue;
        SetProperties(UserId);
    }

    // This function, SetProperties, primarily sets the Product
    // variable with the correct items to show the user when they
    // load/reload the page. This function will also Sort, Search
    // and get proper paging information
    public void SetProperties(int userId)
    {
        // Get the Admin User's id for product selection, then select all products from the Admin
        var AdminId = _context.User.Where(n => n.UserName == "admin").SingleOrDefault()!;
        var query = _context.Product.Select(p => p).Where(i => i.UserId == AdminId.UserId);

        // If the Search String is not Empty/null, search for items
        // containing that specific name. The query will not result
        // in null due to the check done in FindProduct
        if (!String.IsNullOrEmpty(SearchString))
            query = query.Where(n => n.ProductName.Contains(SearchString) || n.ProductPrice.ToString().Contains(SearchString));

        // Update the query based on the user's sort preferences
        // 
        // NOTE: Cannot use sorting on ProductPrice.
        // ERROR: SQLite does not support expressions of type 'decimal' in ORDER BY clauses.
        // REASON: SQLite has no decimal type, only FLOAT.
        //         It's a very limited database, with only four data types.
        switch (CurrentSort)
        {
            case "Name_Ascending":
                query = query.OrderBy(n => n.ProductName);
                break;

            case "Name_Descending":
                query = query.OrderByDescending(n => n.ProductName);
                break;

            default:
                CurrentSort = "";
                break;
        }

        // Set the TotalProducts variable and finalize the query with the page limit
        TotalProducts = query.Count();
        Product = query.Skip((CurrentPageNum - 1) * PageSize).Take(PageSize).ToList();

        // Set the UserId for this webpage
        ThisUserId = userId;
    }
}