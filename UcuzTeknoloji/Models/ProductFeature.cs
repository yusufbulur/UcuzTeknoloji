namespace UcuzTeknoloji.Models
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string FeatureName { get; set; }
        public string FeatureValue { get; set; }
    }
}