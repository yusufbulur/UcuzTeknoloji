using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UcuzTeknoloji.Pages
{
    [Authorize] 
    public class ProfilModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfilModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IdentityUser AktifKullanici { get; set; }

        [BindProperty]
        public string TelefonNumarasi { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            AktifKullanici = await _userManager.GetUserAsync(User);
            if (AktifKullanici == null) return RedirectToPage("/Hesap/GirisYap");

            TelefonNumarasi = AktifKullanici.PhoneNumber; 
            return Page();
        }

        public async Task<IActionResult> OnPostBilgiGuncelleAsync()
        {
            AktifKullanici = await _userManager.GetUserAsync(User);
            if (AktifKullanici != null)
            {
                AktifKullanici.PhoneNumber = TelefonNumarasi;
                await _userManager.UpdateAsync(AktifKullanici);
            }
            return RedirectToPage();
        }
    }
}