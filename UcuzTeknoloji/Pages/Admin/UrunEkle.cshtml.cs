using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UcuzTeknoloji.Data;
using UcuzTeknoloji.Models;

namespace UcuzTeknoloji.Pages.Admin
{
    public class UrunEkleModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        [BindProperty]
        public Product YeniUrun { get; set; }

        public UrunEkleModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context; _env = env;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(IFormFile UrunResmi)
        {
           
            if (UrunResmi != null)
            {
                string yuklemeKlasoru = Path.Combine(_env.WebRootPath, "img");
                if (!Directory.Exists(yuklemeKlasoru))
                {
                    Directory.CreateDirectory(yuklemeKlasoru); 
                }

                var fName = Guid.NewGuid().ToString() + Path.GetExtension(UrunResmi.FileName);
                using (var s = new FileStream(Path.Combine(yuklemeKlasoru, fName), FileMode.Create))
                {
                    await UrunResmi.CopyToAsync(s);
                }
                YeniUrun.ImagePath = fName;
            }

           
            _context.Products.Add(YeniUrun);
            await _context.SaveChangesAsync();

           
            _context.ProductFeatures.Add(new ProductFeature { ProductId = YeniUrun.Id, FeatureName = "Marka", FeatureValue = Request.Form["FixMarka"] });
            _context.ProductFeatures.Add(new ProductFeature { ProductId = YeniUrun.Id, FeatureName = "Renk", FeatureValue = Request.Form["FixRenk"] });

          
            var sablonAdi = Request.Form["SecilenSablonAdi"];
            if (!string.IsNullOrEmpty(sablonAdi))
            {
                _context.ProductFeatures.Add(new ProductFeature { ProductId = YeniUrun.Id, FeatureName = "Kategoriﬁablonu", FeatureValue = sablonAdi });
            }

           
            var dNames = Request.Form["DynamicFeatureNames"];
            var dValues = Request.Form["DynamicFeatureValues"];
            for (int i = 0; i < dNames.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(dNames[i]) && !string.IsNullOrWhiteSpace(dValues[i]))
                {
                    _context.ProductFeatures.Add(new ProductFeature { ProductId = YeniUrun.Id, FeatureName = dNames[i], FeatureValue = dValues[i] });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Urunler");
        }
    }
}