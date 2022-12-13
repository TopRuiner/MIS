using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Diagnosis
    {
        [Key]
        public string Id { get; set; }
        public string? Description { get; set; }
    }
}
