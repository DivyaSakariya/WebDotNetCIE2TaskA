using HospitalAppointmentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HospitalAppointmentSystem.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IMemoryCache _cache;

        // Receives the memory cache through dependency injection.
        public DoctorsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Returns the doctor list, cached for three minutes.
        private List<Doctor> GetDoctors()
        {
            return _cache.GetOrCreate("DoctorList", entry =>
            {
                // Sets the cache expiration to three minutes.
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3);

                // Provides sample doctor data for this assignment.
                return new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        DoctorName = "Dr. Aarav Shah",
                        Specialization = "Cardiology",
                        Experience = 12,
                        ConsultationFee = 800
                    },
                    new Doctor
                    {
                        DoctorId = 2,
                        DoctorName = "Dr. Meera Patel",
                        Specialization = "Dermatology",
                        Experience = 8,
                        ConsultationFee = 600
                    },
                    new Doctor
                    {
                        DoctorId = 3,
                        DoctorName = "Dr. Rohan Mehta",
                        Specialization = "Neurology",
                        Experience = 15,
                        ConsultationFee = 1000
                    },
                    new Doctor
                    {
                        DoctorId = 4,
                        DoctorName = "Dr. Nisha Desai",
                        Specialization = "Pediatrics",
                        Experience = 10,
                        ConsultationFee = 500
                    },
                    new Doctor
                    {
                        DoctorId = 5,
                        DoctorName = "Dr. Kavya Joshi",
                        Specialization = "Orthopedics",
                        Experience = 7,
                        ConsultationFee = 700
                    },
                    new Doctor
                    {
                        DoctorId = 6,
                        DoctorName = "Dr. Dev Trivedi",
                        Specialization = "Cardiology",
                        Experience = 9,
                        ConsultationFee = 750
                    }
                };
            })!;
        }

        // Displays all doctors or filters them by specialization.
        public IActionResult Index(string? specialization)
        {
            // Stores the selected specialization in session.
            if (!string.IsNullOrWhiteSpace(specialization))
            {
                HttpContext.Session.SetString(
                    "Specialization", specialization);
            }

            // Reads the specialization saved in session.
            string? selected = HttpContext.Session.GetString(
                "Specialization");

            // Retrieves the doctor list from memory cache.
            var doctors = GetDoctors();

            // Filters the list when a specialization is selected.
            if (!string.IsNullOrWhiteSpace(selected))
            {
                doctors = doctors
                    .Where(d => d.Specialization == selected)
                    .ToList();
            }

            // Sends the selected specialization to the view.
            ViewBag.SelectedSpecialization = selected;

            // Displays the filtered doctor list.
            return View(doctors);
        }

        // Displays doctor details using the DoctorId query string.
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult Details(int DoctorId)
        {
            // Finds the doctor with the requested ID.
            var doctor = GetDoctors()
                .FirstOrDefault(d => d.DoctorId == DoctorId);

            // Displays an error if the doctor does not exist.
            if (doctor == null)
                return NotFound("Doctor not found.");

            // Displays the selected doctor's information.
            return View(doctor);
        }

        // Opens the appointment form for the selected doctor.
        [HttpGet]
        public IActionResult Book(int DoctorId)
        {
            // Checks whether the doctor exists.
            var doctor = GetDoctors()
                .FirstOrDefault(d => d.DoctorId == DoctorId);

            // Returns an error for an invalid doctor ID.
            if (doctor == null)
                return NotFound("Doctor not found.");

            // Prefills the patient name from the cookie, if available.
            var patientName = Request.Cookies["PatientName"] ?? "";

            // Creates the appointment form model.
            var appointment = new Appointment
            {
                DoctorId = DoctorId,
                PatientName = patientName,
                AppointmentDate = DateTime.Today.AddDays(1)
            };

            // Sends doctor details to the form.
            ViewBag.Doctor = doctor;

            // Displays the appointment form.
            return View(appointment);
        }

        // Processes the submitted appointment request.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(Appointment appointment)
        {
            // Checks that the selected doctor exists.
            var doctor = GetDoctors()
                .FirstOrDefault(d => d.DoctorId == appointment.DoctorId);

            // Rejects an invalid doctor ID.
            if (doctor == null)
                return NotFound("Doctor not found.");

            // Prevents appointment dates in the past.
            if (appointment.AppointmentDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(appointment.AppointmentDate),
                    "Appointment date cannot be in the past.");
            }

            // Returns the form when validation fails.
            if (!ModelState.IsValid)
            {
                ViewBag.Doctor = doctor;
                return View(appointment);
            }

            // Saves the patient name in a cookie for one day.
            Response.Cookies.Append(
                "PatientName",
                appointment.PatientName.Trim(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax
                });

            // Sends the successful appointment request to the view.
            ViewBag.Doctor = doctor;
            return View("Success", appointment);
        }
    }
}