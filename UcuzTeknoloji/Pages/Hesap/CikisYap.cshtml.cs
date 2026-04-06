using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace UcuzTeknoloji.Pages.Hesap
{
    public class CikisYapModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public CikisYapModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        
        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync(); 
            return RedirectToPage("/Index"); 
        }
    }
}