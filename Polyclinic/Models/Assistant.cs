using System.ComponentModel.DataAnnotations;

namespace Polyclinic.Models
{
    public class Assistant
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
    }
}
