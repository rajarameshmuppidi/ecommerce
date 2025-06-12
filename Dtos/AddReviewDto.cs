namespace EcommercePlatform.Dtos
{
    public class AddReviewDto
    {
        public string UserId { get; set; }
        public required string ReviewString { get;set; }
        public int Rating { get;set; }

    }
}
