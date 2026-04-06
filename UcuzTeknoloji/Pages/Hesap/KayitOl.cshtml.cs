using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UcuzTeknoloji.Pages.Hesap
{
    public class KayitOlModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public KayitOlModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty] public string KullaniciAdi { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Sifre { get; set; }
        [BindProperty] public string SifreTekrar { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Sifre != SifreTekrar)
            {
                ModelState.AddModelError("", "Þifreler birbiriyle uyuþmuyor hacý!");
                return Page();
            }

            var kAdiKucuk = KullaniciAdi.ToLower();
            if (kAdiKucuk == "admin" || kAdiKucuk == "sistemadmin")
            {
                ModelState.AddModelError("", "Bu kullanýcý adýný alamazsýn çakal!");
                return Page();
            }

            var user = new IdentityUser { UserName = KullaniciAdi, Email = Email };
            var result = await _userManager.CreateAsync(user, Sifre); 

            if (result.Succeeded)
            {
                
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return Page();
        }


    }
}