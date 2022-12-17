using System.ComponentModel.DataAnnotations;

namespace Polyclinic.Models
{
    public class Diagnosis
    {
        [Key]
        public string Id { get; set; }
        public string? Description { get; set; }
    }
}
