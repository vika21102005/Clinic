using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagement;

namespace Project_A
{
    // Клас Clinic
    public class Clinic
    {
        public string Name { get; set; }
        public List<Doctor> Doctors { get; private set; }
        public List<Patient> Patients { get; private set; }
        public List<Room> Rooms { get; private set; }
        public List<MedicalService> MedicalServices { get; private set; }
        public List<Appointment> Appointments { get; private set; }

        public Clinic(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Doctors = new List<Doctor>();
            Patients = new List<Patient>();
            Rooms = new List<Room>();
            MedicalServices = new List<MedicalService>();
            Appointments = new List<Appointment>();
        }

        // Агрегація: Додавання лікаря до клініки
        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            Doctors.Add(doctor);
            doctor.Clinic = this;
            Console.WriteLine($"Лікар {doctor.Name} доданий до клініки.");
        }

        // Асоціація: Додавання пацієнта до клініки
        public void AddPatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            Patients.Add(patient);
            Console.WriteLine($"Пацієнт {patient.FullName} доданий до клініки.");
        }

        // Агрегація: Додавання кабінету до клініки
        public void AddRoom(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));

            Rooms.Add(room);
            Console.WriteLine($"Кабінет {room.RoomNumber} додано до клініки.");
        }

        // Асоціація: Додавання медичної послуги до клініки
        public void AddMedicalService(MedicalService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            MedicalServices.Add(service);
            Console.WriteLine($"Медичну послугу '{service.ServiceName}' додано до клініки.");
        }

        // Композиція: Планування прийому
        public void ScheduleAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            // Перевірка наявності лікаря та пацієнта в клініці
            if (!Doctors.Contains(appointment.Doctor))
                throw new InvalidOperationException("Лікар не працює в цій клініці.");
            if (!Patients.Contains(appointment.Patient))
                throw new InvalidOperationException("Пацієнта не зареєстровано в клініці.");
            if (!Rooms.Contains(appointment.Room))
                throw new InvalidOperationException("Кабінет не існує в клініці.");

            // Перевірка доступності кабінету
            if (Appointments.Any(a => a.Room == appointment.Room && a.AppointmentDate == appointment.AppointmentDate))
                throw new InvalidOperationException("Кабінет вже зайнятий на цей час.");

            // Перевірка доступності лікаря
            if (Appointments.Any(a => a.Doctor == appointment.Doctor && a.AppointmentDate == appointment.AppointmentDate))
                throw new InvalidOperationException("Лікар вже має прийом на цей час.");

            Appointments.Add(appointment);
            appointment.Clinic = this;
            appointment.ConfirmAppointment();
            appointment.Patient.Appointments.Add(appointment);
            appointment.Doctor.Appointments.Add(appointment);
            Console.WriteLine($"Прийом заплановано для пацієнта {appointment.Patient.FullName} у лікаря {appointment.Doctor.Name} на {appointment.AppointmentDate} в кабінеті {appointment.Room.RoomNumber}.");
        }
    }

}
