using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UcuzTeknoloji.Data;
using UcuzTeknoloji.Models;

namespace UcuzTeknoloji.Pages.Admin
{
    public class SistemOlusturModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SistemOlusturModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context; _env = env;
        }

        // HER BÝLEÞEN ÝÇÝN AYRI ÖZEL LÝSTELER
        public List<Product> Kasalar { get; set; } = new();
        public List<Product> Islemciler { get; set; } = new();
        public List<Product> Anakartlar { get; set; } = new();
        public List<Product> EkranKartlari { get; set; } = new();
        public List<Product> Ramler { get; set; } = new();
        public List<Product> Ssdler { get; set; } = new();
        public List<Product> GucKaynaklari { get; set; } = new();
        public List<Product> Hddler { get; set; } = new();
        public List<Product> Sogutucular { get; set; } = new();
        public List<Product> Ekipmanlar { get; set; } = new();
        public Dictionary<string, List<Product>> EkstraUrunlerSözlügü { get; set; } = new();

        [BindProperty] public string SistemAdi { get; set; }
        [BindProperty] public decimal Fiyat { get; set; }
        [BindProperty] public int Stok { get; set; }

        public void OnGet()
        {
            var tumUrunler = _context.Products.Include(p => p.Features).ToList();
            Ekipmanlar = tumUrunler.Where(p => p.CategoryId == 3).ToList(); // Hediyeler için ekipmanlar

         
            List<Product> SablonaGoreFiltrele(string sablonAdi)
            {
                return tumUrunler.Where(p => p.Features.Any(f =>
                    f.FeatureName == "KategoriÞablonu" &&
                    f.FeatureValue.ToLower() == sablonAdi.ToLower()
                )).ToList();
            }

           
            Kasalar = SablonaGoreFiltrele("Kasa");
            Islemciler = SablonaGoreFiltrele("Ýþlemci");
            Anakartlar = SablonaGoreFiltrele("Anakart");
            EkranKartlari = SablonaGoreFiltrele("Ekran Kartý");
            Ramler = SablonaGoreFiltrele("RAM");
            Ssdler = SablonaGoreFiltrele("SSD");
            GucKaynaklari = SablonaGoreFiltrele("Güç Kaynaðý");
            Hddler = SablonaGoreFiltrele("HDD");
            Sogutucular = SablonaGoreFiltrele("Soðutucu");

            
            var yasakliSablonlar = new List<string> { "kasa", "iþlemci", "anakart", "ekran kartý", "ram", "ssd", "güç kaynaðý", "hdd", "soðutucu", "özel hazýr sistem" };

           
            var ekstraUrunler = tumUrunler.Where(p =>
            {
                var sName = p.Features.FirstOrDefault(f => f.FeatureName == "KategoriÞablonu")?.FeatureValue?.ToLower();
                return sName != null && !yasakliSablonlar.Contains(sName);
            }).ToList();

            EkstraUrunlerSözlügü = ekstraUrunler
                .GroupBy(p => p.Features.First(f => f.FeatureName == "KategoriÞablonu").FeatureValue)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public async Task<IActionResult> OnPostAsync(IFormFile UrunResmi)
        {
            var yeniSistem = new Product { Name = SistemAdi, Price = Fiyat, Stock = Stok, CategoryId = 1 };

            if (UrunResmi != null)
            {
                string yuklemeKlasoru = Path.Combine(_env.WebRootPath, "img");
                if (!Directory.Exists(yuklemeKlasoru)) Directory.CreateDirectory(yuklemeKlasoru);
                var fName = Guid.NewGuid().ToString() + Path.GetExtension(UrunResmi.FileName);
                using (var s = new FileStream(Path.Combine(yuklemeKlasoru, fName), FileMode.Create)) { await UrunResmi.CopyToAsync(s); }
                yeniSistem.ImagePath = fName;
            }

            _context.Products.Add(yeniSistem);
            await _context.SaveChangesAsync();

            var secilenler = new Dictionary<string, string>
            {
                { "Kasa", Request.Form["Bilesen_Kasa"] }, { "Ýþlemci", Request.Form["Bilesen_Islemci"] },
                { "Anakart", Request.Form["Bilesen_Anakart"] }, { "Ekran Kartý", Request.Form["Bilesen_EkranKarti"] },
                { "RAM", Request.Form["Bilesen_Ram"] }, { "SSD", Request.Form["Bilesen_SSD"] },
                { "Güç Kaynaðý", Request.Form["Bilesen_PSU"] }, { "HDD", Request.Form["Bilesen_HDD"] },
                { "Soðutucu", Request.Form["Bilesen_Sogutucu"] }, { "Sistem Hediyesi", Request.Form["HediyeUrunId"] },
                { "Özel Ýndirim", Request.Form["SistemIndirimi"] }

            };

            // ALTERNATÝF (ÖZELLEÞTÝRME) SEÇENEKLERÝNÝ VERÝTABANINA KAYDET
            string[] alternatifTurleri = { "Alternatif_Islemci", "Alternatif_EkranKarti", "Alternatif_Ram", "Alternatif_Anakart", "Alternatif_Depolama" };
            foreach (var tur in alternatifTurleri)
            {
                var secilenAlternatifler = Request.Form[tur]; 
                foreach (var alt in secilenAlternatifler)
                {
                    _context.ProductFeatures.Add(new ProductFeature { ProductId = yeniSistem.Id, FeatureName = tur, FeatureValue = alt });
                }
            }

            foreach (var bilesen in secilenler)
            {
                if (!string.IsNullOrWhiteSpace(bilesen.Value))
                    _context.ProductFeatures.Add(new ProductFeature { ProductId = yeniSistem.Id, FeatureName = bilesen.Key, FeatureValue = bilesen.Value });
            }

            _context.ProductFeatures.Add(new ProductFeature { ProductId = yeniSistem.Id, FeatureName = "KategoriÞablonu", FeatureValue = "Özel Hazýr Sistem" });
            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Urunler");
        }
    }
}