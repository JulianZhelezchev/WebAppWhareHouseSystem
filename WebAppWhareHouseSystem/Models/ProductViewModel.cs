using Microsoft.AspNetCore.Http;

namespace WebAppWhareHouseSystem.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
        public byte[] ImageContent { get; set; }
        public string ImageMimeType { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string ProductCode { get; set; }

    }
}
