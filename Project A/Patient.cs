using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagement;

namespace Project_A
{
    // Клас Patient
    public class Patient
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Appointment> Appointments { get; private set; }

        public Patient(string fullName, DateTime dateOfBirth)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            DateOfBirth = dateOfBirth;
            Appointments = new List<Appointment>();
        }

        // Асоціація: Планування прийому
        public void ScheduleAppointment(Clinic clinic, Doctor doctor, Room room, DateTime appointmentDate)
        {
            if (clinic == null)
                throw new ArgumentNullException(nameof(clinic));
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));
            if (room == null)
                throw new ArgumentNullException(nameof(room));

            var appointment = new Appointment(appointmentDate, doctor, this, room);
            clinic.ScheduleAppointment(appointment);
        }
    }
}
