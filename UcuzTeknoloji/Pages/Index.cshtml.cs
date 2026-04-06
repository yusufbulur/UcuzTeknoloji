using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UcuzTeknoloji.Data;
using UcuzTeknoloji.Models;

namespace UcuzTeknoloji.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager; 

      
        public IndexModel(AppDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public List<Product> Products { get; set; } = new List<Product>();

        public void OnGet()
        {
           
            Products = _context.Products.ToList();
        }

       
        public async Task<IActionResult> OnGetCikisYapAsync()
        {
            await _signInManager.SignOutAsync(); 
            return RedirectToPage("/Index"); 
        }
    }
}