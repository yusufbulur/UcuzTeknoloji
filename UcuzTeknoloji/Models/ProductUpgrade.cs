namespace UcuzTeknoloji.Models
{
    public class ProductUpgrade
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } 
        public string UpgradeName { get; set; } = string.Empty; 
        public decimal PriceDifference { get; set; } 
    }
}