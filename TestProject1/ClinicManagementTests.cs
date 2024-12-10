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
        public void Setup()//��� ����� ��������� ������������ ��� ���������� ��'���� ����� ������ ������,
                           //��� ���� ���� ������� ��� ������������ � ������.
        {
            _clinic = new Clinic("�������� ����� \"������'�\"");

            _doctor = new Doctor("���� ��������", "��������", "������");
            _patient = new Patient("���� �������", new DateTime(1990, 5, 20));
            _room = new Room("101", "���������������");
            _service = new MedicalService("������������", 500m);

            _clinic.AddDoctor(_doctor);
            _clinic.AddPatient(_patient);
            _clinic.AddRoom(_room);
            _clinic.AddMedicalService(_service);
        }

        [TestMethod]
        public void AddDoctor_ShouldAddDoctorToClinic()//�������� ����������� ��������� ����� �� �����.
        {
            var newDoctor = new Doctor("����� ��������", "ճ����", "ճ�����");
            _clinic.AddDoctor(newDoctor);
            CollectionAssert.Contains(_clinic.Doctors, newDoctor);
            Assert.AreEqual(_clinic, newDoctor.Clinic);
        }

        [TestMethod]
        public void AddPatient_ShouldAddPatientToClinic()//�������� ����������� ��������� �������� �� �����
        {
            var newPatient = new Patient("����� ���㳿���", new DateTime(1985, 3, 15));
            _clinic.AddPatient(newPatient);
            CollectionAssert.Contains(_clinic.Patients, newPatient);
        }

        [TestMethod]
        public void AddRoom_ShouldAddRoomToClinic()//�������� ����������� ��������� ������ �� �����
        {
            var newRoom = new Room("102", "�����������");
            _clinic.AddRoom(newRoom);
            CollectionAssert.Contains(_clinic.Rooms, newRoom);
        }

        [TestMethod]
        public void AddMedicalService_ShouldAddServiceToClinic()//�������� ����������� ��������� �������
                                                                //������� �� �����
        {
            var newService = new MedicalService("�������", 300m);
            _clinic.AddMedicalService(newService);
            CollectionAssert.Contains(_clinic.MedicalServices, newService);
        }

        [TestMethod]
        public void ScheduleAppointment_ShouldAddAppointmentToClinic()//�������� ����������� ��������� ������
                                                                      //�� ������ �� �����
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
        public void ScheduleAppointment_ShouldThrowException_WhenDoctorNotInClinic()//��������, �� ����������
                         //����������, ���� ���� �� �������� �� �����, �� ������� ���������� ���������� �� ������.
        {
            var otherClinic = new Clinic("���� �����");
            var otherDoctor = new Doctor("������� ���������", "��������", "���������");
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(11);

            _patient.ScheduleAppointment(_clinic, otherDoctor, _room, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//��������, �� ���������� ����������, ���� �������
                                                     //�� � ��������� �����, � ��� ���������� ���������� �� ������
        public void ScheduleAppointment_ShouldThrowException_WhenPatientNotInClinic()
        {
            var otherPatient = new Patient("����� ���������", new DateTime(1980, 7, 25));
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(12);

            otherPatient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//��������, �� ���������� ����������, ���� ������,
                                                //�� ��� ������� ���������� ���������� �� ������, �� �������� �����
        public void ScheduleAppointment_ShouldThrowException_WhenRoomNotInClinic()
        {
            var otherRoom = new Room("103", "��������������");
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(13);

            _patient.ScheduleAppointment(_clinic, _doctor, otherRoom, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//��������, �� ���������� ����������, ���� ������
                                                              //��� ������� �� ������ ���� � ���.
        public void ScheduleAppointment_ShouldThrowException_WhenRoomIsOccupied()
        {
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(14);
            _patient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);

            var anotherPatient = new Patient("���� ��������", new DateTime(1992, 8, 30));
            _clinic.AddPatient(anotherPatient);

            anotherPatient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//��������, �� ���������� ����������, ���� ����
                                                              //��� �� ������ �� ������ ���� � ���
        public void ScheduleAppointment_ShouldThrowException_WhenDoctorIsBusy()
        {
            var appointmentDate = DateTime.Now.AddDays(1).Date.AddHours(15);
            _patient.ScheduleAppointment(_clinic, _doctor, _room, appointmentDate);

            var anotherPatient = new Patient("������� ���������", new DateTime(1988, 12, 5));
            _clinic.AddPatient(anotherPatient);
            var anotherRoom = new Room("104", "ĳ�����������");
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
            Assert.IsTrue(output.Contains($"{_doctor.Name} ������ ��� ����'���� �� {_doctor.Position}."));
            Assert.IsTrue(output.Contains($"{_receptionist.Name} ������ �� ���: {_receptionist.Shift}."));
        }
    }
}
