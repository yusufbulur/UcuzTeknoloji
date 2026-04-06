using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UcuzTeknoloji.Pages.Hesap
{
    public class GirisYapModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public GirisYapModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty] public string KullaniciAdi { get; set; }
        [BindProperty] public string Sifre { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
           
            var result = await _signInManager.PasswordSignInAsync(KullaniciAdi, Sifre, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError("", "Kullanýcý adý veya þifre hatalý!");
                return Page();
            }
        }
    }
}