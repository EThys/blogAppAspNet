using System.ComponentModel.DataAnnotations;

namespace blogApp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "La description de la categorie est obligatoire")]
        [StringLength(100, ErrorMessage = "La description de la categorie ne peut contenir que 100 caractères")]
        public string ? CategoryTitle { get; set; }

        [Required(ErrorMessage ="La description de la categorie est obligatoire")]
        [StringLength(500,ErrorMessage = "La description de la categorie ne peut contenir que 500 caractères")]

        public string? CategoryDescription { get; set; }
        public DateOnly CreatedAt { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
