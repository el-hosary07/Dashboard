using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class RegesterVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public string UserName { get; set; } = String.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = String.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = String.Empty;
    }
}
