using Core_Prac_01.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Core_Prac_01.ViewModels
{
    public class ApplicantEditModel
    {
        public int ApplicantId { get; set; }
        [Required, StringLength(40)]
        public string ApplicantName { get; set; } = default!;
        [Required, Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        [Required, StringLength(30)]
        public string AppliedFor { get; set; } = default!;
        public IFormFile? Picture { get; set; } = default!;
        public bool IsReadyToWorkAnyWhere { get; set; }
        public virtual List<QualificationInputModel> Qualifications { get; set; } = new List<QualificationInputModel>();
    }
}
