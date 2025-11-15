using System;

using Tasks.Common;

namespace Tasks.FlatCount
{
    public class FlatCount : IFlatCountSolution
    {
        public void Run()
        {
            string flatNumberInput = Console.ReadLine();
            string floorCountInput = Console.ReadLine();
            string flatsCountInput = Console.ReadLine();
            int flatNumber;
            int floorCount;
            int flatsPerFloor;
            try
            {
                flatNumber = int.Parse(flatNumberInput);
                floorCount = int.Parse(floorCountInput);
                flatsPerFloor = int.Parse(flatsCountInput);
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка: одна из введенных строк не является целым числом.");
                return;
            }
            if (flatNumber < 1 || flatNumber > 999)
            {
                Console.WriteLine($"Ошибка: введенная квартира '{flatNumber}' не попадает в диапазон [1;999]");
                return;
            }
            if (floorCount < 1 || floorCount > 999)
            {
                Console.WriteLine($"Ошибка: число этажей '{floorCount}' не попадает в диапазон [1;999]");
                return;
            }
            if (flatsPerFloor < 1 || flatsPerFloor > 999)
            {
                Console.WriteLine($"Ошибка: число квартир на лестничной площадке '{flatsPerFloor}' не попадает в диапазон [1;999]");
                return;
            }
            var result = Calculate(flatNumber, floorCount, flatsPerFloor);
            Console.WriteLine(result.entrance);
            Console.WriteLine(result.floor);
        }

        public (int entrance, int floor) Calculate(int flatNumber, int floorCount, int flatsPerFloor)
        {
            int entranceFlatsCount = flatsPerFloor * floorCount;
            int entranceNumber = (flatNumber - 1) / entranceFlatsCount + 1;
            int relativeFlatNumber = (flatNumber - 1) % entranceFlatsCount + 1;
            int floorNumber = (relativeFlatNumber - 1) / flatsPerFloor + 1;
            return (entranceNumber, floorNumber);
        }
    }
}