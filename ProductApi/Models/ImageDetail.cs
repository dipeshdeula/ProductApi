namespace ProductApi.Models
{
    public class ImageDetail
    {
        public int Id { get; set; }
        public string ProductImage { get; set; } = null!;
        public string ImagePath { get; set; } = null!;

        public string Base64Image { get; set; } = null!;
    }
}
