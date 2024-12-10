using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagement;

namespace Project_A
{
    // Клас Appointment
    public class Appointment
    {
        public DateTime AppointmentDate { get; private set; }
        public Doctor Doctor { get; private set; }
        public Patient Patient { get; private set; }
        public Room Room { get; private set; }
        public Clinic? Clinic { get; set; } // Зроблено nullable
        public bool IsConfirmed { get; private set; }

        public Appointment(DateTime appointmentDate, Doctor doctor, Patient patient, Room room)
        {
            AppointmentDate = appointmentDate;
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            IsConfirmed = false;
        }

        public void ConfirmAppointment()
        {
            IsConfirmed = true;
            Console.WriteLine($"Прийом підтверджено: {Patient.FullName} у лікаря {Doctor.Name} на {AppointmentDate} в кабінеті {Room.RoomNumber}.");

        }
    }
}
