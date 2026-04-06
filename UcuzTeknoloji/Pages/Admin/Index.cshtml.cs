using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UcuzTeknoloji.Models;
using UcuzTeknoloji.Repositories;

namespace UcuzTeknoloji.Pages.Admin
{
    [Authorize] // Sadece giriþ yapanlar görebilir
    public class IndexModel : PageModel
    {
        private readonly IRepository<Product> _productRepo;
        public List<Product> Products { get; set; }

        public IndexModel(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public void OnGet()
        {
            Products = _productRepo.GetAll();
        }

        public IActionResult OnPostSil(int id)
        {
            _productRepo.Delete(id);
            return RedirectToPage("/Admin/Index");
        }
    }
}