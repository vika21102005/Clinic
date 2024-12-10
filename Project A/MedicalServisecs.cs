using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
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
}
