using System;

namespace EcommercePlatform.Dtos
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
