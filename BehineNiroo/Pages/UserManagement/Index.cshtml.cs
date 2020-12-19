using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using BehineNiroo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BehineNiroo
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly BehineNirooContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public IndexModel(BehineNirooContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public IEnumerable<IdentityUser> users { get; set; }

        public IActionResult OnGet()
        {
            users = userManager.Users.ToList();
            return Page();
        }
    }
}