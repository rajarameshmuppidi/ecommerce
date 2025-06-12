namespace EcommercePlatform.Dtos
{
    public class UpdateOrderDto
    {
        public int? Qauntity { get; set; }
        public DateTime? ExpectedDeliveryBy { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public OrderStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
