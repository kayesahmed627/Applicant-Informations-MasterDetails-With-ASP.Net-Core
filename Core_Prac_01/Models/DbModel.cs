using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core_Prac_01.Models
{
    public enum Gender { Male = 1, Female }
    public class Applicant
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
        [Required, StringLength(30)]
        public string Picture { get; set; } = default!;
        public bool IsReadyToWorkAnyWhere { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();
    }
    public class Qualification
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
        public virtual Applicant? Applicant { get; set; } = default!;
    }
    public class ApplicantDbContext : DbContext
    {
        public ApplicantDbContext(DbContextOptions<ApplicantDbContext> options) : base(options) { }
        public DbSet<Applicant> Applicants { get; set; } = default!;
        public DbSet<Qualification> Qualifications { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applicant>().HasData(
                new Applicant { ApplicantId = 1, ApplicantName = "A Applicant", AppliedFor = "Peon", BirthDate = DateTime.Now.AddYears(-19), Gender = Gender.Female, IsReadyToWorkAnyWhere = false, Picture = "1.jpg" }
                );
            modelBuilder.Entity<Qualification>().HasData(
               new Qualification { QualificationId = 1, ApplicantId = 1, Degree = "SSC", PassingYear = 2019, Institute = "KPS", Result = "4.5" },
               new Qualification { QualificationId = 2, ApplicantId = 1, Degree = "HSC", PassingYear = 2021, Institute = "KPC", Result = "5.0" }
               );
        }
    }
}
