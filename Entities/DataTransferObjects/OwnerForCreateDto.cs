using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class OwnerForCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(60,ErrorMessage = "Name must be less than 60 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address must be less than 100 characters.")]
        public string? Address { get; set; }
    }
}
