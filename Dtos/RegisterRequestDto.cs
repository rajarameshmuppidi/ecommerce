using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        
        // Additional data that can be used for specific registration flows
        public Dictionary<string, string>? AdditionalData { get; set; }
    }
}
