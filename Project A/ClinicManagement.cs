using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClinicManagement
{
    // Інтерфейс IEmployee
    public interface IEmployee
    {
        string Name { get; set; }
        string Position { get; set; }

        void PerformDuties();
    }
    // Абстрактний клас Employee
    public abstract class Employee : IEmployee, ICloneable, IComparable<Employee>
    {
        public abstract string Name { get; set; }
        public abstract string Position { get; set; }

        public abstract void PerformDuties();

        // Реалізація ICloneable
        public abstract object Clone();

        // Реалізація IComparable
        public int CompareTo(Employee? other)
        {
            if (other == null) return 1;
            return this.Name.CompareTo(other.Name);
        }

        // Перевизначення ToString для кращого представлення
        public override string ToString()
        {
            return $"{Name} - {Position}";
        }
    }

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

        // Вивід інформації про клініку
        public void DisplayInfo()
        {
            Console.WriteLine($"\nКлініка: {Name}");
            Console.WriteLine("Лікарі:");
            foreach (var doctor in Doctors)
            {
                Console.WriteLine($" - {doctor.Name}, {doctor.Position}, Спеціальність: {doctor.Specialty}");
            }

            Console.WriteLine("Пацієнти:");
            foreach (var patient in Patients)
            {
                Console.WriteLine($" - {patient.FullName}, Дата народження: {patient.DateOfBirth.ToShortDateString()}");
            }

            Console.WriteLine("Кабінети:");
            foreach (var room in Rooms)
            {
                Console.WriteLine($" - Кабінет {room.RoomNumber}, Тип: {room.Type}");
            }

            Console.WriteLine("Медичні послуги:");
            foreach (var service in MedicalServices)
            {
                Console.WriteLine($" - {service.ServiceName}, Вартість: {service.Price} грн.");
            }

            Console.WriteLine("Прийоми:");
            foreach (var appointment in Appointments)
            {
                Console.WriteLine($" - {appointment.Patient.FullName} з {appointment.Doctor.Name} на {appointment.AppointmentDate} в кабінеті {appointment.Room.RoomNumber}");
            }
            Console.WriteLine();
        }

        public static implicit operator Clinic(Project_A.Clinic v)
        {
            throw new NotImplementedException();
        }
    }

    // Клас Doctor
    public class Doctor : IEmployee
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Specialty { get; set; }
        public Clinic? Clinic { get; set; } // Зроблено nullable
        public List<Appointment> Appointments { get; private set; }

        public Doctor(string name, string position, string specialty)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Specialty = specialty ?? throw new ArgumentNullException(nameof(specialty));
            Appointments = new List<Appointment>();
        }

        public void PerformDuties()
        {
            Console.WriteLine($"{Name} виконує свої обов'язки як {Position}.");
        }

        public void PrescribeMedication(Patient patient, string medication)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));
            if (medication == null)
                throw new ArgumentNullException(nameof(medication));

            Console.WriteLine($"{Name} призначив пацієнту {patient.FullName} медикамент {medication}.");
        }
    }

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

    // Клас MedicalService
    public class MedicalService
    {
        public string ServiceName { get; set; }
        public decimal Price { get; set; }

        public MedicalService(string serviceName, decimal price)
        {
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            Price = price;
        }

        public void ProvideService(Patient patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            Console.WriteLine($"Надано медичну послугу '{ServiceName}' пацієнту {patient.FullName}. Вартість: {Price} грн.");
        }
    }

    // Клас Room
    public class Room
    {
        public string RoomNumber { get; set; }
        public string Type { get; set; }

        public Room(string roomNumber, string type)
        {
            RoomNumber = roomNumber ?? throw new ArgumentNullException(nameof(roomNumber));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public void ReserveRoom()
        {
            Console.WriteLine($"Кабінет {RoomNumber} зарезервовано для прийому.");
        }
    }

    // Клас Program з функцією Main
    class Program
    {
        static void Main(string[] args)
        {
            // Створення клініки
            Clinic clinic = new Clinic("Приватна Клініка \"Здоров'я\"");

            // Додавання лікарів
            Doctor doctor1 = new Doctor("Іван Іванович", "Терапевт", "Терапія");
            Doctor doctor2 = new Doctor("Світлана Світланівна", "Кардіолог", "Кардіологія");
            clinic.AddDoctor(doctor1);
            clinic.AddDoctor(doctor2);

            // Додавання пацієнтів
            Patient patient1 = new Patient("Марія Петрівна", new DateTime(1990, 5, 20));
            Patient patient2 = new Patient("Олег Олександрович", new DateTime(1985, 3, 15));
            clinic.AddPatient(patient1);
            clinic.AddPatient(patient2);

            // Додавання кабінетів
            Room room1 = new Room("101", "Консультаційний");
            Room room2 = new Room("102", "Операційний");
            clinic.AddRoom(room1);
            clinic.AddRoom(room2);

            // Додавання медичних послуг
            MedicalService service1 = new MedicalService("Консультація", 500m);
            MedicalService service2 = new MedicalService("Рентген", 300m);
            clinic.AddMedicalService(service1);
            clinic.AddMedicalService(service2);

            // Основне меню
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nВиберіть дію:");
                Console.WriteLine("1. Додати лікаря");
                Console.WriteLine("2. Додати пацієнта");
                Console.WriteLine("3. Додати кабінет");
                Console.WriteLine("4. Додати медичну послугу");
                Console.WriteLine("5. Запланувати прийом");
                Console.WriteLine("6. Показати інформацію про клініку");
                Console.WriteLine("7. Вийти");
                Console.Write("Ваш вибір: ");
                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddDoctor(clinic);
                            break;
                        case "2":
                            AddPatient(clinic);
                            break;
                        case "3":
                            AddRoom(clinic);
                            break;
                        case "4":
                            AddMedicalService(clinic);
                            break;
                        case "5":
                            ScheduleAppointment(clinic);
                            break;
                        case "6":
                            clinic.DisplayInfo();
                            break;
                        case "7":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }

            Console.WriteLine("Дякуємо за використання програми! До побачення.");
        }

        static void AddDoctor(Clinic clinic)
        {
            Console.Write("Введіть ім'я лікаря: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ім'я лікаря не може бути порожнім.");
                return;
            }

            Console.Write("Введіть посаду лікаря: ");
            string? position = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(position))
            {
                Console.WriteLine("Посада лікаря не може бути порожньою.");
                return;
            }

            Console.Write("Введіть спеціальність лікаря: ");
            string? specialty = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(specialty))
            {
                Console.WriteLine("Спеціальність лікаря не може бути порожньою.");
                return;
            }

            Doctor doctor = new Doctor(name, position, specialty);
            clinic.AddDoctor(doctor);
        }

        static void AddPatient(Clinic clinic)
        {
            Console.Write("Введіть повне ім'я пацієнта: ");
            string? fullName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.WriteLine("Повне ім'я пацієнта не може бути порожнім.");
                return;
            }

            Console.Write("Введіть дату народження пацієнта (у форматі РРРР-ММ-ДД): ");
            string? dobInput = Console.ReadLine();
            if (!DateTime.TryParse(dobInput, out DateTime dob))
            {
                Console.WriteLine("Невірний формат дати.");
                return;
            }

            Patient patient = new Patient(fullName, dob);
            clinic.AddPatient(patient);
        }

        static void AddRoom(Clinic clinic)
        {
            Console.Write("Введіть номер кабінету: ");
            string? roomNumber = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(roomNumber))
            {
                Console.WriteLine("Номер кабінету не може бути порожнім.");
                return;
            }

            Console.Write("Введіть тип кабінету: ");
            string? type = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(type))
            {
                Console.WriteLine("Тип кабінету не може бути порожнім.");
                return;
            }

            Room room = new Room(roomNumber, type);
            clinic.AddRoom(room);
        }

        static void AddMedicalService(Clinic clinic)
        {
            Console.Write("Введіть назву медичної послуги: ");
            string? serviceName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                Console.WriteLine("Назва медичної послуги не може бути порожньою.");
                return;
            }

            Console.Write("Введіть вартість медичної послуги: ");
            string? priceInput = Console.ReadLine();
            if (!decimal.TryParse(priceInput, out decimal price))
            {
                Console.WriteLine("Невірний формат ціни.");
                return;
            }

            MedicalService service = new MedicalService(serviceName, price);
            clinic.AddMedicalService(service);
        }

        static void ScheduleAppointment(Clinic clinic)
        {
            if (clinic.Doctors.Count == 0)
            {
                Console.WriteLine("Немає доступних лікарів. Додайте лікаря перед плануванням прийому.");
                return;
            }

            if (clinic.Patients.Count == 0)
            {
                Console.WriteLine("Немає зареєстрованих пацієнтів. Додайте пацієнта перед плануванням прийому.");
                return;
            }

            if (clinic.Rooms.Count == 0)
            {
                Console.WriteLine("Немає доступних кабінетів. Додайте кабінет перед плануванням прийому.");
                return;
            }

            Console.WriteLine("\nСписок лікарів:");
            for (int i = 0; i < clinic.Doctors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {clinic.Doctors[i].Name} ({clinic.Doctors[i].Specialty})");
            }
            Console.Write("Виберіть лікаря (номер): ");
            string? doctorIndexInput = Console.ReadLine();
            if (!int.TryParse(doctorIndexInput, out int doctorIndex) || doctorIndex < 1 || doctorIndex > clinic.Doctors.Count)
            {
                Console.WriteLine("Невірний вибір лікаря.");
                return;
            }
            Doctor selectedDoctor = clinic.Doctors[doctorIndex - 1];

            Console.WriteLine("\nСписок пацієнтів:");
            for (int i = 0; i < clinic.Patients.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {clinic.Patients[i].FullName}");
            }
            Console.Write("Виберіть пацієнта (номер): ");
            string? patientIndexInput = Console.ReadLine();
            if (!int.TryParse(patientIndexInput, out int patientIndex) || patientIndex < 1 || patientIndex > clinic.Patients.Count)
            {
                Console.WriteLine("Невірний вибір пацієнта.");
                return;
            }
            Patient selectedPatient = clinic.Patients[patientIndex - 1];

            Console.WriteLine("\nСписок кабінетів:");
            for (int i = 0; i < clinic.Rooms.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Кабінет {clinic.Rooms[i].RoomNumber} ({clinic.Rooms[i].Type})");
            }
            Console.Write("Виберіть кабінет (номер): ");
            string? roomIndexInput = Console.ReadLine();
            if (!int.TryParse(roomIndexInput, out int roomIndex) || roomIndex < 1 || roomIndex > clinic.Rooms.Count)
            {
                Console.WriteLine("Невірний вибір кабінету.");
                return;
            }
            Room selectedRoom = clinic.Rooms[roomIndex - 1];

            Console.Write("Введіть дату та час прийому (у форматі РРРР-ММ-ДД ГГ:ХХ): ");
            string? appointmentDateInput = Console.ReadLine();
            if (!DateTime.TryParse(appointmentDateInput, out DateTime appointmentDate))
            {
                Console.WriteLine("Невірний формат дати та часу.");
                return;
            }

            selectedPatient.ScheduleAppointment(clinic, selectedDoctor, selectedRoom, appointmentDate);
        }
    }
}