namespace HospitalAppointmentSystem.Models
{
    // Represents a doctor available at the hospital.
    public class Doctor
    {
        // Stores the unique doctor ID.
        public int DoctorId { get; set; }

        // Stores the doctor's full name.
        public string DoctorName { get; set; } = string.Empty;

        // Stores the doctor's medical specialization.
        public string Specialization { get; set; } = string.Empty;

        // Stores the doctor's years of experience.
        public int Experience { get; set; }

        // Stores the consultation fee in rupees.
        public decimal ConsultationFee { get; set; }
    }
}