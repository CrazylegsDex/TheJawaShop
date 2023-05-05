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
    public class ManageUser : PageModel
    {
        private readonly TheJawaShop.Models.DatabaseContext _context;

        public ManageUser(TheJawaShop.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<User> TheUser { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.User != null)
            {
                TheUser = await _context.User.Include(o => o.Orders).ToListAsync();
            }
        }
    }
}
