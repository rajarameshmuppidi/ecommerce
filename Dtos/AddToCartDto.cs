namespace EcommercePlatform.Dtos
{
    public class AddToCartDto
    {
        public string? UserId { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
