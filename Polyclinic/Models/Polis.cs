using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Polis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Patient")]
        public int Id { get; set; }
        public string Company { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        [NotMapped]
        public string ReturnDateForDisplay
        {
            get
            {
                return this.EndDate.ToShortDateString();
            }
        }
    }
}
