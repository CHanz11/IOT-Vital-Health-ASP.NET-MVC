using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Models
{
    public class Patients
    {
        [Key] // This is the Primary key or ID
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is Required")]
        [StringLength(50, ErrorMessage = "Last name Cannot Exceed 50 characters.")]
        public string LastName { get; set; }

        public string? ProfileImagePath { get; set; } = null; // Profile image

        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birthdate is Required")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }

        public decimal Weight { get; set; } = 0;

        public decimal Height { get; set; } = 0;

        public decimal Temperature { get; set; } = 0;

        public int Systolic { get; set; } = 0;

        public int Diastolic { get; set; } = 0;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Systolic > Diastolic)
            {
                yield return new ValidationResult(
                    "Systolic cannot be greater than Diastolic.",
                    new[] { nameof(Systolic), nameof(Diastolic) }
                );
            }
        }

        public string BloodPressure { get; set; } = "N/A";

        public decimal BMI { get; set; } = 0;

        public DateTime UpdatedDate { get; set; } =  DateTime.Now;

    }
}
