using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51
{
    public class Agent
    {
        public SecurityLevel SecurityLevel { get; }
        private readonly Elevator elevator;
        public string Name { get; }

        private const int MaxFloorsBase = 4;

        public Agent(string name, SecurityLevel securityLevel, Elevator elevator)
        {
            this.Name = name;
            SecurityLevel = securityLevel;
            this.elevator = elevator;
        }

        public void Simulate()
        {
            Random random = new Random();

            while (true)
            {
                int currentFloor = random.Next(MaxFloorsBase);
                int targetFloor = random.Next(MaxFloorsBase);

                if (currentFloor != targetFloor)
                {
                    CallElevator(currentFloor);
                    Thread.Sleep(random.Next(1000, 5000));
                    PressButtonInside(targetFloor);
                    EnterElevator();
                }

                Thread.Sleep(random.Next(5000, 10000));
            }
        }

        public void CallElevator(int floor)
        {
            Console.WriteLine($"{Name} called the elevator from floor {elevator.GetFloorName(floor)}.");
            elevator.CallElevator(floor, SecurityLevel);
        }

        public void PressButtonInside(int floor)
        {
            Console.WriteLine($"{Name} pressed the button inside the elevator for floor {elevator.GetFloorName(floor)}.");
            elevator.PressButtonInside(floor, SecurityLevel);
        }

        public void EnterElevator()
        {
            Console.WriteLine($"{Name} entered the elevator.");
            elevator.EnterElevator(this);
        }
    }
}