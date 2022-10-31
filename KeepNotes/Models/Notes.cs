using System.ComponentModel.DataAnnotations;

namespace KeepNotes.Models
{
    public class Notes
    {
        public int NotesId { get; set; }

        public Users User { get; set; }
        public Category Category { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public bool isPublic { get; set; }
    }
}
