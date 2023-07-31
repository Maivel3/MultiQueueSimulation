using MultiQueueModels;
using MultiQueueTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MultiQueueSimulation
{
    public class operation
    {
        public int FileNumber { get; set; }

        static int SimulationTime = 0;
        static Random randomNum = new Random();
        static private void DisTable(List<TimeDistribution> table)
        {
            if (table.Count != 0)
            {
                for (int i = 0; i < table.Count; i++)
                {

                    if (i == 0)
                    {
                        table[i].CummProbability = table[i].Probability;
                        table[i].MinRange = 1;
                    }
                    else
                    {
                        table[i].CummProbability = table[i].Probability + table[i - 1].CummProbability;
                        table[i].MinRange = table[i - 1].MaxRange + 1;
                    }
                    table[i].MaxRange = (int)(table[i].CummProbability * 100);


                }
            }
            else
            {
                throw new Exception("You Must Enter Values");
            }
        }


        static private int GetRange(List<TimeDistribution> TableDis, int RandomValue)
        {
            int x = -1;
            for (int i = 0; i < TableDis.Count; i++)
            {

                if (RandomValue <= TableDis[i].MaxRange && RandomValue >= TableDis[i].MinRange)
                {
                    x = TableDis[i].Time;
                }
            }
            return x;
        }

        static private void SelectMethod(SimulationCase SimCase, List<Server> Servers, Enums.SelectionMethod SelectionMethod)
        {
            List<Server> emptyServers = new List<Server>();

            int server_num = 0;
            int serverID;
            int minServiceTime = 100000;
            bool first_found = false;
            if (SelectionMethod == Enums.SelectionMethod.HighestPriority)
            {
                if (SimCase.CustomerNumber == 0)
                {
                    SimCase.AssignedServer = Servers[server_num];
                    SimCase.RandomService = randomNum.Next(1, 100);
                    SimCase.ServiceTime = GetRange(Servers[server_num].TimeDistribution, SimCase.RandomService);
                    SimCase.StartTime = 0;
                    SimCase.EndTime = SimCase.ServiceTime;
                    SimCase.TimeInQueue = 0;
                    Servers[server_num].FinishTime = SimCase.ServiceTime;
                    //minServiceTime = Servers[server_num].FinishTime;
                    Servers[server_num].TotalWorkingTime = SimCase.ServiceTime;
                    Servers[server_num].AverageServiceTime = SimCase.ServiceTime;
                    Servers[server_num].ServedCount++;
                }
                else
                {
                    for (int i = 0; i < Servers.Count; i++)
                    {
                        if (Servers[i].FinishTime <= SimCase.ArrivalTime)
                        {
                            server_num = i;
                            first_found = true;
                            break;
                        }

                    }
                    if (first_found == false)
                    {
                        for (int i = 0; i < Servers.Count; i++)
                        {
                            if (Servers[i].FinishTime < minServiceTime)
                            {
                                server_num = i;
                                minServiceTime = Servers[i].FinishTime;
                            }
                        }

                    }
                    SimCase.AssignedServer = Servers[server_num];
                    SimCase.RandomService = randomNum.Next(1, 100);
                    SimCase.ServiceTime = GetRange(Servers[server_num].TimeDistribution, SimCase.RandomService);
                    SimCase.StartTime = Math.Max(Servers[server_num].FinishTime, SimCase.ArrivalTime);
                    SimCase.EndTime = SimCase.StartTime + SimCase.ServiceTime;
                    SimCase.TimeInQueue = SimCase.StartTime - SimCase.ArrivalTime;
                    Servers[server_num].FinishTime = SimCase.StartTime + SimCase.ServiceTime;
                    Servers[server_num].TotalWorkingTime += SimCase.ServiceTime;
                    Servers[server_num].AverageServiceTime += SimCase.ServiceTime;
                    Servers[server_num].ServedCount++;
                }

            }

            else if (SelectionMethod == Enums.SelectionMethod.Random)
            {
                for (int i = 0; i < Servers.Count; i++)
                {
                    if (Servers[i].FinishTime <= SimCase.ArrivalTime)
                    {
                        emptyServers.Add(Servers[i]);
                    }
                }

                int serversCount = emptyServers.Count;
                int random;
                if (serversCount > 0)
                {
                    if (SimCase.CustomerNumber == 0)
                    {
                        random = randomNum.Next(0, serversCount - 1);

                        serverID = emptyServers[random].ID - 1;
                        SimCase.AssignedServer = Servers[serverID];
                        SimCase.RandomService = randomNum.Next(1, 100);
                        SimCase.ServiceTime = GetRange(Servers[serverID].TimeDistribution, SimCase.RandomService);
                        SimCase.StartTime = 0;
                        SimCase.EndTime = SimCase.ServiceTime;
                        SimCase.TimeInQueue = 0;
                        Servers[serverID].FinishTime = SimCase.ServiceTime;
                        minServiceTime = Servers[serverID].FinishTime;
                        Servers[serverID].TotalWorkingTime = SimCase.ServiceTime;
                        Servers[serverID].AverageServiceTime = SimCase.ServiceTime;
                        Servers[serverID].ServedCount++;
                    }
                    else
                    {

                        serversCount = emptyServers.Count;
                        random = randomNum.Next(0, serversCount - 1);
                        for (int i = 0; i < Servers.Count; i++)
                        {
                            if (emptyServers[random].ID == Servers[i].ID)
                            {
                                server_num = i;
                                break;
                            }

                        }


                        SimCase.AssignedServer = Servers[server_num];
                        SimCase.RandomService = randomNum.Next(1, 100);
                        SimCase.ServiceTime = GetRange(Servers[server_num].TimeDistribution, SimCase.RandomService);
                        SimCase.StartTime = Math.Max(Servers[server_num].FinishTime, SimCase.ArrivalTime);
                        SimCase.EndTime = SimCase.StartTime + SimCase.ServiceTime;
                        SimCase.TimeInQueue = SimCase.StartTime - SimCase.ArrivalTime;
                        Servers[server_num].FinishTime = SimCase.StartTime + SimCase.ServiceTime;
                        Servers[server_num].TotalWorkingTime += SimCase.ServiceTime;
                        Servers[server_num].AverageServiceTime += SimCase.ServiceTime;
                        Servers[server_num].ServedCount++;
                    }
                }
                else
                {
                    for (int i = 0; i < Servers.Count; i++)
                    {
                        if (Servers[i].FinishTime < minServiceTime)
                        {
                            server_num = i;
                            minServiceTime = Servers[i].FinishTime;
                        }
                    }
                    SimCase.AssignedServer = Servers[server_num];
                    SimCase.RandomService = randomNum.Next(1, 100);
                    SimCase.ServiceTime = GetRange(Servers[server_num].TimeDistribution, SimCase.RandomService);
                    SimCase.StartTime = Math.Max(Servers[server_num].FinishTime, SimCase.ArrivalTime);
                    SimCase.EndTime = SimCase.StartTime + SimCase.ServiceTime;
                    SimCase.TimeInQueue = SimCase.StartTime - SimCase.ArrivalTime;
                    Servers[server_num].FinishTime = SimCase.StartTime + SimCase.ServiceTime;
                    Servers[server_num].TotalWorkingTime += SimCase.ServiceTime;
                    Servers[server_num].AverageServiceTime += SimCase.ServiceTime;
                    Servers[server_num].ServedCount++;
                }
            }


            else if (SelectionMethod == Enums.SelectionMethod.LeastUtilization)
            {
                for (int i = 0; i < Servers.Count; i++)
                {
                    if (Servers[i].FinishTime <= SimCase.ArrivalTime)
                    {
                        emptyServers.Add(Servers[i]);
                    }
                }
                if (SimCase.CustomerNumber == 0)
                {

                    SimCase.AssignedServer = Servers[server_num];
                    SimCase.RandomService = randomNum.Next(1, 100);
                    SimCase.ServiceTime = GetRange(Servers[server_num].TimeDistribution, SimCase.RandomService);
                    SimCase.StartTime = 0;
                    SimCase.EndTime = SimCase.ServiceTime;
                    SimCase.TimeInQueue = 0;
                    Servers[server_num].FinishTime = SimCase.ServiceTime;
                    minServiceTime = Servers[server_num].FinishTime;
                    Servers[server_num].TotalWorkingTime = SimCase.ServiceTime;
                    Servers[server_num].AverageServiceTime = SimCase.ServiceTime;
                    Servers[server_num].ServedCount++;
                }

                else
                {
                    int index = 0;
                    if (emptyServers.Count != 0)
                    {
                        decimal leastUT = 1000000;

                        for (int i = 0; i < emptyServers.Count; i++)
                        {
                            if (emptyServers[i].TotalWorkingTime < leastUT)
                            {
                                leastUT = emptyServers[i].TotalWorkingTime;
                                index = i;

                            }
                        }

                        for (int i = 0; i < Servers.Count; i++)
                        {
                            if (Servers[i].ID == emptyServers[index].ID)
                            {
                                server_num = i;
                                break;
                            }
                        }
                    }

                    else
                    {
                        for (int i = 0; i < Servers.Count; i++)
                        {
                            if (Servers[i].FinishTime < minServiceTime)
                            {
                                server_num = i;
                                minServiceTime = Servers[i].FinishTime;
                            }
                        }
                    }

                    SimCase.AssignedServer = Servers[server_num];
                    SimCase.RandomService = randomNum.Next(1, 100);
                    SimCase.ServiceTime = GetRange(Servers[server_num].TimeDistribution, SimCase.RandomService);
                    SimCase.StartTime = Math.Max(Servers[server_num].FinishTime, SimCase.ArrivalTime);
                    SimCase.EndTime = SimCase.StartTime + SimCase.ServiceTime;
                    SimCase.TimeInQueue = SimCase.StartTime - SimCase.ArrivalTime;
                    Servers[server_num].FinishTime = SimCase.StartTime + SimCase.ServiceTime;
                    Servers[server_num].TotalWorkingTime += SimCase.ServiceTime;
                    Servers[server_num].AverageServiceTime += SimCase.ServiceTime;
                    Servers[server_num].ServedCount++;
                }
            }
        }

        static public void simulator(SimulationSystem SimSystem)
        {
            DisTable(SimSystem.InterarrivalDistribution);
            for (int j = 0; j < SimSystem.Servers.Count; j++)
            {
                DisTable(SimSystem.Servers[j].TimeDistribution);
            }

            Queue<SimulationCase> waited_list = new Queue<SimulationCase>();

            int Total_TimeinQueue = 0;
            int Number_Of_Customers_Who_Waited = 0;
            int Max_QueueLength = 0;
            List<SimulationCase> All_customers = new List<SimulationCase>();
            if (SimSystem.StoppingCriteria == Enums.StoppingCriteria.NumberOfCustomers)
            {
                for (int i = 0; i < SimSystem.StoppingNumber; i++)
                {
                    SimulationCase simcase = new SimulationCase();
                    simcase.CustomerNumber = i;
                    simcase.RandomInterArrival = randomNum.Next(1, 100);

                    if (i == 0)
                    {
                        simcase.InterArrival = 0;
                        simcase.ArrivalTime = 0;
                    }
                    else
                    {
                        simcase.InterArrival = GetRange(SimSystem.InterarrivalDistribution, simcase.RandomInterArrival);
                        simcase.ArrivalTime = All_customers[i - 1].ArrivalTime + simcase.InterArrival;
                    }
                    SelectMethod(simcase, SimSystem.Servers, SimSystem.SelectionMethod);

                    if (waited_list.Count > 0)
                    {
                        if (waited_list.Peek().StartTime <= simcase.ArrivalTime)
                        {
                            waited_list.Dequeue();
                        }
                    }
                    if (simcase.TimeInQueue > 0)
                    {
                        waited_list.Enqueue(simcase);
                        Total_TimeinQueue += simcase.TimeInQueue;
                        Number_Of_Customers_Who_Waited++;
                    }
                    Max_QueueLength = Math.Max(Max_QueueLength, waited_list.Count);
                    SimulationTime = Math.Max(SimulationTime, simcase.EndTime);
                    All_customers.Add(simcase);
                }
                SimSystem.SimulationTable = All_customers;
                SimSystem.PerformanceMeasures.AverageWaitingTime = ((decimal)Total_TimeinQueue / All_customers.Count);
                SimSystem.PerformanceMeasures.WaitingProbability = ((decimal)Number_Of_Customers_Who_Waited / All_customers.Count);
                SimSystem.PerformanceMeasures.MaxQueueLength = Max_QueueLength;
                SimSystem.SimulationTime = SimulationTime;
                for (int i = 0; i < SimSystem.Servers.Count; i++)
                {
                    if (SimSystem.Servers[i].ServedCount != 0)
                        SimSystem.Servers[i].AverageServiceTime /= SimSystem.Servers[i].ServedCount;
                    SimSystem.Servers[i].Utilization = (decimal)SimSystem.Servers[i].TotalWorkingTime / SimulationTime;
                }
            }

            else
            {
                int customer_num = 0;
                while (true)
                {
                    SimulationCase customer = new SimulationCase();
                    customer.CustomerNumber = customer_num;
                    customer.RandomInterArrival = randomNum.Next(1, 100);

                    if (All_customers.Count == 0)
                    {
                        customer.InterArrival = 0;
                        customer.ArrivalTime = 0;
                    }
                    else
                    {
                        customer.InterArrival = GetRange(SimSystem.InterarrivalDistribution, customer.RandomInterArrival);
                        customer.ArrivalTime = All_customers[All_customers.Count - 1].ArrivalTime + customer.InterArrival;
                    }
                    if (customer.ArrivalTime > SimSystem.StoppingNumber)
                    {
                        break;
                    }
                    SelectMethod(customer, SimSystem.Servers, SimSystem.SelectionMethod);
                    if (waited_list.Count > 0)
                    {
                        if (waited_list.Peek().StartTime <= customer.ArrivalTime)
                        {
                            waited_list.Dequeue();
                        }
                    }
                    if (customer.TimeInQueue > 0)
                    {
                        waited_list.Enqueue(customer);
                        Total_TimeinQueue += customer.TimeInQueue;
                        Number_Of_Customers_Who_Waited++;
                    }
                    SimulationTime = Math.Max(SimulationTime, customer.EndTime);
                    Max_QueueLength = Math.Max(Max_QueueLength, waited_list.Count);
                    customer_num++;
                    All_customers.Add(customer);
                }
                SimSystem.SimulationTable = All_customers;
                SimSystem.PerformanceMeasures.AverageWaitingTime = ((decimal)Total_TimeinQueue / All_customers.Count);
                SimSystem.PerformanceMeasures.WaitingProbability = ((decimal)Number_Of_Customers_Who_Waited / All_customers.Count);
                SimSystem.PerformanceMeasures.MaxQueueLength = Max_QueueLength;
                SimSystem.SimulationTime = SimulationTime;
                for (int i = 0; i < SimSystem.Servers.Count; i++)
                {
                    if (SimSystem.Servers[i].ServedCount != 0)
                        SimSystem.Servers[i].AverageServiceTime /= SimSystem.Servers[i].ServedCount;
                    SimSystem.Servers[i].Utilization = (decimal)SimSystem.Servers[i].TotalWorkingTime / SimulationTime;
                }
            }
        }

        static public void ReadFromFile(SimulationSystem simsys, int FileN0)
        {


            List<string> lines = new List<string>();

            FileStream file = new FileStream("TestCase" + FileN0 + ".txt", FileMode.Open);
            StreamReader read = new StreamReader(file);
            try
            {
                while (read.Peek() != -1)
                {
                    lines.Add(read.ReadLine());
                }
                simsys.NumberOfServers = int.Parse(lines[1]);
                simsys.StoppingNumber = int.Parse(lines[4]);
                simsys.StoppingCriteria = (Enums.StoppingCriteria)int.Parse(lines[7]);
                simsys.SelectionMethod = (Enums.SelectionMethod)int.Parse(lines[10]);
                int count = int.Parse(lines[13]);
                string[] temp;
                int i = 14;
                for (int j = 0; j < count; j++)
                {

                    temp = lines[i].Split(',');
                    simsys.InterarrivalDistribution.Add(new TimeDistribution
                    {
                        Time = int.Parse(temp[0]),
                        Probability = Convert.ToDecimal(temp[1])

                    });
                    i++;
                }
                i += 2;
                for (int z = 0; z < simsys.NumberOfServers; z++)
                {
                    count = int.Parse(lines[i]);
                    Server server = new Server();
                    server.ID = z + 1;
                    i++;
                    for (int j = 0; j < count; j++)
                    {

                        temp = lines[i].Split(',');
                        server.TimeDistribution.Add(new TimeDistribution
                        {
                            Time = int.Parse(temp[0]),
                            Probability = Convert.ToDecimal(temp[1])

                        });
                        i++;
                    }
                    simsys.Servers.Add(server);
                    i += 2;
                }
                file.Close();
            }
            catch
            {
                MessageBox.Show("Error In Reading From File");
            }

        }

        static public void Test(SimulationSystem system)
        {
            simulator(system);
            string testingMessage = TestingManager.Test(system, Constants.FileNames.TestCase1);
            MessageBox.Show(testingMessage);
        }
    }


}
