using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        [DataType (DataType.Password)]
        public required string Password { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}
