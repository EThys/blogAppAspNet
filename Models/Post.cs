using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogApp.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public User ? User { get; set; }

        [Required(ErrorMessage = "CategoryFId est requis")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [StringLength(100, ErrorMessage = "Le titre du post ne peut contenir que 100 caractères, pas plus")]
        public string? PostTitle { get; set; }

        [StringLength(200, ErrorMessage = "Le commentaire ne peut contenir que 200 caractères, pas plus")]

        public string? PostMessage { get; set; }

        public string ? PostFilePath { get; set; }
                
        public int PostStatus { get; set; }
        public DateOnly CreatedAt { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();


    }
}
