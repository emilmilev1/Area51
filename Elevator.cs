using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51
{
    public class Elevator
    {
        private readonly object lockObject = new object();
        private int currentFloor = 0;
        private List<Agent> agentsInside;
        private bool[] floorButtons;

        public Elevator(int numberOfFloors)
        {
            floorButtons = new bool[numberOfFloors];
            agentsInside = new List<Agent>();
        }

        public void CallElevator(int floor, SecurityLevel securityLevel)
        {
            lock (lockObject)
            {
                floorButtons[floor] = true;
                Console.WriteLine($"{securityLevel} agent on floor {GetFloorName(floor)} called the elevator.");
            }
        }

        public void PressButtonInside(int floor, SecurityLevel securityLevel)
        {
            lock (lockObject)
            {
                floorButtons[floor] = true;
                Console.WriteLine($"{securityLevel} agent inside the elevator pressed button for floor {GetFloorName(floor)}.");
            }
        }

        public void Move()
        {
            while (true)
            {
                int targetFloor = GetNextTargetFloor();

                if (targetFloor == -1)
                {
                    Console.WriteLine("Elevator is idle.");
                    Thread.Sleep(1000);
                }
                else
                {
                    MoveToFloor(targetFloor);
                    OpenDoor();
                    CloseDoor();
                }
            }
        }

        private int GetNextTargetFloor()
        {
            lock (lockObject)
            {
                for (int i = 0; i < floorButtons.Length; i++)
                {
                    if (floorButtons[i])
                    {
                        floorButtons[i] = false;
                        return i;
                    }
                }
                return -1;
            }
        }

        private void MoveToFloor(int targetFloor)
        {
            int floorsToMove = Math.Abs(currentFloor - targetFloor);
            Thread.Sleep(floorsToMove * 1000);

            currentFloor = targetFloor;
            Console.WriteLine($"Elevator arrived at floor {GetFloorName(currentFloor)}.");
        }

        private void OpenDoor()
        {
            Console.WriteLine("Opening door.");
            Thread.Sleep(1000);

            lock (lockObject)
            {
                if (agentsInside.Any())
                {
                    Agent lowestSecurityAgent = agentsInside.OrderBy(agent => agent.SecurityLevel).First();
                    Console.WriteLine($"Agent with the lowest security level ({lowestSecurityAgent.SecurityLevel}) exits the elevator.");
                    agentsInside.Remove(lowestSecurityAgent);
                }
                else
                {
                    Console.WriteLine("Elevator is empty.");
                }
            }
        }

        private void CloseDoor()
        {
            Console.WriteLine("Closing door.");
            Thread.Sleep(1000);

            lock (lockObject)
            {
                if (floorButtons.Any(button => button) || agentsInside.Any(agent => agent != null))
                {
                    Console.WriteLine("Elevator is still active. Keeping the door open.");
                }
                else
                {
                    Console.WriteLine("No more floor requests or agents. Closing the door.");
                }
            }
        }

        public string GetFloorName(int floor)
        {
            switch (floor)
            {
                case 0:
                    return "G"; // Ground Floor
                case 1:
                    return "S"; // Secret Floor with nuclear weapons
                case 2:
                    return "T1"; // Secret Floor with experimental weapons
                case 3:
                    return "T2"; // Top-secret Floor that stores alien remains
                default:
                    return "Err"; // Unknown floor
            }
        }

        public void EnterElevator(Agent agent)
        {
            lock (lockObject)
            {
                agentsInside.Add(agent);
                Console.WriteLine($"{agent.Name} entered the elevator.");
            }
        }
    }
}