using System.ComponentModel.DataAnnotations;

namespace KeepNotes.Models
{
    public class Share
    {
        public int ShareId { get; set; }
        public int? UserId { get; set; }
        public Users User { get; set; }

        public int? NoteId { get; set; }
        public Note Note { get; set; }
        [Required]

        public bool isWritable { get; set; }
        [Required]
        public int? ToShareUserId { get; set; }
        public Users ToShareUser { get; set; }
    }
}
