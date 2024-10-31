using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }
        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        [StringLength(500, ErrorMessage = "Le commentaire ne peut contenir que 500 caractères, pas plus")]
        public string ? CommentBody { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
