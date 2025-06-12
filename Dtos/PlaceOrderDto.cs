namespace EcommercePlatform.Dtos
{
    public class PlaceOrderDto
    {

        public Guid ProductId { get; set; }

        public int Quantity { get;set; }

        public Guid RecentCartId { get; set; }
    }
}
