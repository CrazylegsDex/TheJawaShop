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

        public int AdminUserId { get; set; }

        public async Task OnGetAsync(int id)
        {
            TheUser = await _context.User.Where(u => u.UserName != "admin").Include(o => o.Orders).ToListAsync();

            // Set AdminUserId and return
            AdminUserId = id;
        }
    }
}
