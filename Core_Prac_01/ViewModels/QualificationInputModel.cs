using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core_Prac_01.ViewModels
{
    public class QualificationInputModel
    {
        public int QualificationId { get; set; }
        [Required, StringLength(30)]
        public string Degree { get; set; } = default!;
        [Required, StringLength(40)]
        public string Institute { get; set; } = default!;
        [Required]
        public int PassingYear { get; set; }
        [Required, StringLength(20)]
        public string Result { get; set; } = default!;
        [Required, ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
    }
}
