using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace KeepNotes.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }

        [Required]
        public string Name { get; set; }

    }
}