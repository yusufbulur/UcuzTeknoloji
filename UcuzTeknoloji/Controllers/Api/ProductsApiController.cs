using Microsoft.AspNetCore.Mvc;
using UcuzTeknoloji.Models;
using UcuzTeknoloji.Repositories;

namespace UcuzTeknoloji.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IRepository<Product> _repo;
        public ProductsApiController(IRepository<Product> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repo.GetAll()); 
        }
    }
}