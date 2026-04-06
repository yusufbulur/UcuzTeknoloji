using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UcuzTeknoloji.Data;
using UcuzTeknoloji.Models;

namespace UcuzTeknoloji.Pages.Admin
{
    
    public class UrunlerModel : PageModel
    {
        private readonly AppDbContext _context;
        public UrunlerModel(AppDbContext context) { _context = context; }

        public List<Product> Products { get; set; } = new List<Product>();

        public void OnGet()
        {
            
            Products = _context.Products.Include(p => p.Features).ToList();
        }

        public IActionResult OnPostStokIslem(int UrunId, int Miktar, string IslemTuru)
        {
            var urun = _context.Products.Find(UrunId);
            if (urun != null)
            {
                if (IslemTuru == "Arttir") urun.Stock += Miktar;
                else if (IslemTuru == "Azalt") urun.Stock -= Miktar;

                if (urun.Stock < 0) urun.Stock = 0; 
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        
        public IActionResult OnPostUrunSil(int id)
        {
            
            var urun = _context.Products.Include(p => p.Features).FirstOrDefault(p => p.Id == id);

            if (urun != null)
            {
               
                _context.ProductFeatures.RemoveRange(urun.Features);
               
                _context.Products.Remove(urun);

                _context.SaveChanges();
            }
            return RedirectToPage();
        }

       
        public IActionResult OnPostUrunGuncelle(int UrunId, string UrunAdi, decimal UrunFiyati, int UrunStogu)
        {
            var urun = _context.Products.Include(p => p.Features).FirstOrDefault(p => p.Id == UrunId);
            if (urun != null)
            {
                urun.Name = UrunAdi;
                urun.Price = UrunFiyati;
                urun.Stock = UrunStogu;

               
                foreach (var feature in urun.Features)
                {
                    var formdanGelenYeniDeger = Request.Form["Feature_" + feature.Id];
                    if (!string.IsNullOrEmpty(formdanGelenYeniDeger))
                    {
                        feature.FeatureValue = formdanGelenYeniDeger;
                    }
                }
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}