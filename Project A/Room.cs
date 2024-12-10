using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{

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

}
