using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class LoginRequestDto
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
