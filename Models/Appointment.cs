using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    // Represents a patient's appointment request.
    public class Appointment
    {
        // Stores the ID of the selected doctor.
        public int DoctorId { get; set; }

        // Requires the patient to enter their name.
        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(80)]
        public string PatientName { get; set; } = string.Empty;

        // Requires the patient to select an appointment date.
        [Required(ErrorMessage = "Please select an appointment date.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);
    }
}