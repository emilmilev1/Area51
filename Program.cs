namespace Area51
{
    class Program
    {
        public static void Main()
        {
            Elevator elevator = new Elevator(4);

            Thread elevatorThread = new Thread(() => elevator.Move());
            elevatorThread.IsBackground = true;
            elevatorThread.Start();

            List<Agent> agents = new List<Agent>
            {
                new Agent("Confidential Agent", SecurityLevel.Confidential, elevator),
                new Agent("Secret Agent", SecurityLevel.Secret, elevator),
                new Agent("Top-secret Agent", SecurityLevel.TopSecret, elevator)
            };

            List<Thread> agentThreads = new List<Thread>();
            if (agentThreads == null) throw new ArgumentNullException(nameof(agentThreads));

            foreach (var agent in agents)
            {
                Thread agentThread = new Thread(() => agent.Simulate());

                agentThread.IsBackground = true;
                agentThreads.Add(agentThread);
                agentThread.Start();
            }

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}