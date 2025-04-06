using System.ComponentModel.DataAnnotations;

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Models
{
    public class Records
    {
        [Key] // This is the Primary key or ID
        public int RecordId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is Required")]
        [StringLength(50, ErrorMessage = "Last name Cannot Exceed 50 characters.")]
        public string LastName { get; set; }

        public decimal Weight { get; set; } = 0;

        public decimal Height { get; set; } = 0;

        public decimal Temperature { get; set; } = 0;

        public int Systolic { get; set; } = 0;

        public int Diastolic { get; set; } = 0;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Systolic <= Diastolic)
            {
                yield return new ValidationResult(
                    "Diastolic cannot be greater than or equal to Systolic.",
                    new[] { nameof(Systolic), nameof(Diastolic) }
                );
            }
        }

        public string BloodPressure { get; set; } = "N/A";

        public decimal BMI { get; set; } = 0;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
