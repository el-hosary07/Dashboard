using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Dashboard.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(99)]
        [MinLength(8)]
        public string Name { get; set; } = string.Empty;
        [ValidateNever]
        public List<Movie> Movies { get; set; }

    }
}
