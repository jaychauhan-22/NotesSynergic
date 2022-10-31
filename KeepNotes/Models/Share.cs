using System.ComponentModel.DataAnnotations;

namespace KeepNotes.Models
{
    public class Share
    {
        public Users User { get; set; }
        public Category Category { get; set; }
        [Required]
        public bool isWritable { get; set; }
        [Required]
        public int SharedUid { get; set; }
    }
}
