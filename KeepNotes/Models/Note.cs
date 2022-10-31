using System.ComponentModel.DataAnnotations;

namespace KeepNotes.Models
{
    public class Note
    {
        public int NoteId { get; set; }

        public int? UserId { get; set; }
        public Users User { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public bool isPublic { get; set; }
    }
}
