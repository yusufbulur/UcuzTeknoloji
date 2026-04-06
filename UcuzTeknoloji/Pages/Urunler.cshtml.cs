using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UcuzTeknoloji.Data;
using UcuzTeknoloji.Models;

namespace UcuzTeknoloji.Pages
{
    public class UrunlerModel : PageModel
    {
        private readonly AppDbContext _context;

        public UrunlerModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new List<Product>();

        public void OnGet()
        {
           
            Products = _context.Products.Include(p => p.Features).ToList();
        }
    }
}