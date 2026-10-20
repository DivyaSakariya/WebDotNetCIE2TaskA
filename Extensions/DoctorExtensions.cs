using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.Extensions
{
    // Contains additional functionality for Doctor objects.
    public static class DoctorExtensions
    {
        // Calculates the total charge including the service fee.
        public static decimal CalculateTotalCharge(this Doctor doctor)
        {
            // Adds the fixed ₹100 service charge.
            return doctor.ConsultationFee + 100m;
        }
    }
}