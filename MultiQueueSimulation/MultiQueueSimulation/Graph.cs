using MultiQueueModels;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MultiQueueSimulation
{
    public partial class Graph : Form
    {
        SimulationSystem system;
        int servNum;
        public Graph(SimulationSystem system, int servNum)
        {
            InitializeComponent();
            this.system = system;
            this.servNum = servNum;
        }


        private void Graph_Load(object sender, EventArgs e)
        {


            if (servNum <= system.NumberOfServers)
            {

                chart1.Titles.Add("Server Graph ");

                chart1.ChartAreas[0].AxisX.Minimum = 0;
                chart1.ChartAreas[0].AxisX.Maximum = system.Servers[servNum - 1].FinishTime;
                chart1.ChartAreas[0].AxisX.Interval = 5;
                chart1.ChartAreas[0].AxisX.Name = "Time";
                chart1.ChartAreas[0].AxisX.IsMarginVisible = true;

                var chartSeries = chart1.Series.First();
                string label = "Server Number " + servNum;
                chartSeries.Name = label;
                int j = 0;
                bool Num1 = true;
                foreach (SimulationCase c in system.SimulationTable)
                {
                    if (c.AssignedServer.ID == servNum)
                    {
                        if (Num1 == true)
                        {
                            Num1 = false;
                            chart1.Series[label].Points.AddXY(c.StartTime, 1);
                            chart1.Series[label].Points.AddXY(c.EndTime, 1);
                            j = c.EndTime;

                        }
                        if (j != c.StartTime)
                        {
                            chart1.Series[label].Points.AddXY(j, 1);
                            for (int i = j; i <= c.StartTime; i++)
                            {
                                chart1.Series[label].Points.AddXY(i, 0);
                            }
                            chart1.Series[label].Points.AddXY(c.StartTime, 1);
                            chart1.Series[label].Points.AddXY(c.EndTime, 1);
                            j = c.EndTime;
                        }
                        if (j == c.StartTime)
                        {
                            chart1.Series[label].Points.AddXY(c.StartTime, 1);
                            chart1.Series[label].Points.AddXY(c.EndTime, 1);
                            j = c.EndTime;
                        }

                    }
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            servNum++;
            this.Hide();
            Graph graph = new Graph(system, servNum);
            graph.Show();

        }
    }
}
