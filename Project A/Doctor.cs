using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagement;

namespace Project_A
{
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
    
}
