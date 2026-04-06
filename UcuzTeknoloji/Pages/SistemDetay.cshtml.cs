using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UcuzTeknoloji.Data;
using UcuzTeknoloji.Models;

namespace UcuzTeknoloji.Pages
{
    public class SistemDetayModel : PageModel
    {
        private readonly AppDbContext _context;
        public SistemDetayModel(AppDbContext context) { _context = context; }
        public Product Sistem { get; set; }
        public List<Product> TumParcalar { get; set; } 
        public List<ProductFeature> AlternatifIslemciler { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Sistem = _context.Products.Include(p => p.Features).FirstOrDefault(p => p.Id == id);
            if (Sistem == null) return NotFound();

            TumParcalar = _context.Products.ToList(); 

            AlternatifIslemciler = Sistem.Features.Where(f => f.FeatureName == "Alternatif_Islemci").ToList();

            return Page();
        }

        public IActionResult OnPostSepeteEkle(int SistemId, decimal GuncelFiyat, string SecilenIslemci)
        {
            TempData["Mesaj"] = $"Sistem {GuncelFiyat} ₺ fiyattan sepete eklendi! (Seçilen: {SecilenIslemci})";
            return RedirectToPage("/Index");
        }
    }
}