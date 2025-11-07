using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class LoginVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="User Name Or Email")]
        public string UserNameOrEmail { get; set; } = string.Empty;
        [Required , DataType(DataType.Password)]

        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
