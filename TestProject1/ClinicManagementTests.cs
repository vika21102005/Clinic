using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClinicManagement;
using System;
using System.Numerics;

namespace ClinicManagement.Tests
{
    [TestClass]
    public class ClinicTests
    {
        private Clinic _clinic;
        private Doctor _doctor;
        private Patient _patient;
        private Room _room;
        private MedicalService _service;

        [TestInitialize]
        public void Setup()//Цей метод забезпечує налаштування всіх необхідних об'єктів перед кожним тестом,
                           //щоб вони були доступні для використання в тестах.
        {
            _clinic = new Clinic("Приватна Клініка \"Здоров'я\"");

            _doctor = new Doctor("Іван Іванович", "Терапевт", "Терапія");
            _patient = new Patient("Марія Петрівна", new DateTime(1990, 5, 20));
            _room = new Room("101", "Консультаційний");
            _service = new MedicalService("Консультація", 500m);

            _clinic.AddDoctor(_doctor);
            _clinic.AddPatient(_patient);
            _clinic.AddRoom(_room);
            _clinic.AddMedicalService(_service);
        }

        [TestMethod]
        public void AddDoctor_ShouldAddDoctorToClinic()//перевіряє правильність додавання лікаря до клініки.
        {
            var newDoctor = new Doctor("Петро Петрович", "Хірург", "Хірургія");
            _clinic.AddDoctor(newDoctor);
            CollectionAssert.Contains(_clinic.Doctors, newDoctor);
            Assert.AreEqual(_clinic, newDoctor.Clinic);
        }

        [TestMethod]
        public void AddPatient_ShouldAddPatientToClinic()//перевіряє правильність додавання пацієнта до клініки
        {
            var newPatient = new Patient("Олена Сергіївна", new DateTime(1985, 3, 15));
            _clinic.AddPatient(newPatient);
            CollectionAssert.Contains(_clinic.Patients, newPatient);
        }

        [TestMethod]
        public void AddRoom_ShouldAddRoomToClinic()//перевіряє правильність додавання кімнати до клініки
        {
            var newRoom = new Room("102", "Операційний");
            _clinic.AddRoom(newRoom);
            CollectionAssert.Contains(_clinic.Rooms, newRoom);
        }

        [TestMethod]
        public void AddMedicalService_ShouldAddServiceToClinic()//перевіряє правильність додавання медичної
                                                                //послуги до клініки
        {
            var newService = new MedicalService("Рентген", 300m);
            _clinic.AddMedicalService(newService);
            CollectionAssert.Contains(_clinic.MedicalServices, newService);
        }

        [TestMethod]
        public void ScheduleAppointment_ShouldAddAppointmentToClinic()//перевіряє правильність додавання запису
                                                                      //на прийом до клініки
        {
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(10);
            _patient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);

            Assert.AreEqual(1, _clinic.Appointments.Count);
            var appointment = _clinic.Appointments[0];
            Assert.AreEqual(appointmentDate, appointment.AppointmentDate);
            Assert.AreEqual(_doctor, appointment.Doctor);
            Assert.AreEqual(_patient, appointment.Patient);
            Assert.AreEqual(_room, appointment.Room);
            Assert.IsTrue(appointment.IsConfirmed);
            CollectionAssert.Contains(_patient.Appointments, appointment);
            CollectionAssert.Contains(_doctor.Appointments, appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScheduleAppointment_ShouldThrowException_WhenDoctorNotInClinic()//перевіряє, чи викидається
                         //виключення, коли лікар не належить до клініки, де пацієнт намагається записатися на прийом.
        {
            var otherClinic = new Clinic("Інша Клініка");
            var otherDoctor = new Doctor("Світлана Світланівна", "Кардіолог", "Кардіологія");
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(11);

            _patient.ScheduleAppointment(_clinic, otherDoctor, _room, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//перевіряє, чи викидається виключення, коли пацієнт
                                                     //не є пацієнтом клініки, в яку намагається записатися на прийом
        public void ScheduleAppointment_ShouldThrowException_WhenPatientNotInClinic()
        {
            var otherPatient = new Patient("Андрій Андрійович", new DateTime(1980, 7, 25));
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(12);

            otherPatient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//перевіряє, чи викидається виключення, коли кімната,
                                                //до якої пацієнт намагається записатися на прийом, не належить клініці
        public void ScheduleAppointment_ShouldThrowException_WhenRoomNotInClinic()
        {
            var otherRoom = new Room("103", "Стоматологічний");
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(13);

            _patient.ScheduleAppointment(_clinic, _doctor, otherRoom, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//перевіряє, чи викидається виключення, коли кімната
                                                              //вже зайнята на задану дату і час.
        public void ScheduleAppointment_ShouldThrowException_WhenRoomIsOccupied()
        {
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(14);
            _patient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);

            var anotherPatient = new Patient("Ігор Ігорович", new DateTime(1992, 8, 30));
            _clinic.AddPatient(anotherPatient);

            anotherPatient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//перевіряє, чи викидається виключення, коли лікар
                                                              //вже має прийом на задану дату і час
        public void ScheduleAppointment_ShouldThrowException_WhenDoctorIsBusy()
        {
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(15);
            _patient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);

            var anotherPatient = new Patient("Світлана Світланівна", new DateTime(1988, 12, 5));
            _clinic.AddPatient(anotherPatient);
            var anotherRoom = new Room("104", "Діагностичний");
            _clinic.AddRoom(anotherRoom);

            anotherPatient.ScheduleAppointment(_clinic, _doctor, anotherRoom, appointmentDate);
        }
        [TestMethod]
        public void DisplayEmployeeDuties_ShouldInvokeCorrectMethodForDoctorAndReceptionist()
        {
            // Arrange
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            _clinic.DisplayEmployeeDuties();

            // Assert
            var output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains($"{_doctor.Name} виконує свої обов'язки як {_doctor.Position}."));
            Assert.IsTrue(output.Contains($"{_receptionist.Name} працює на зміні: {_receptionist.Shift}."));
        }
    }
}
