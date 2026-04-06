namespace UcuzTeknoloji.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductFeature> Features { get; set; }
        public List<ProductUpgrade> Upgrades { get; set; }
    }
}