using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;

namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        SimulationSystem system;
        SimulationSystem sys;
        operation op;
        public Form1()
        {
            system = new SimulationSystem();
            sys = new SimulationSystem();
            op = new operation();
           
            InitializeComponent();
        }
        // number of servers
        private void NumOfS_ValueChanged(object sender, EventArgs e)
        {
            system.NumberOfServers = (int)NumOfS.Value;
        }
        //stoping critiria
        private void StopC_SelectedIndexChanged(object sender, EventArgs e)
        {
            StoppingNum.Enabled = true;
            system.StoppingCriteria = (Enums.StoppingCriteria)(StopC.SelectedIndex + 1);
        }
        // stopping number
        private void StoppingNum_ValueChanged(object sender, EventArgs e)
        {
            system.StoppingNumber = (int)StoppingNum.Value;
        }
        // selection method 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            system.SelectionMethod = (Enums.SelectionMethod)(comboBox1.SelectedIndex + 1);
        }
        // Submit button
        private void Submit_Click(object sender, EventArgs e)
        {
            for (int rows = 0; rows < InterAT.Rows.Count - 1; rows++)
            {
                system.InterarrivalDistribution.Add(new TimeDistribution()
                {
                    Time = int.Parse((string)InterAT.Rows[rows].Cells[0].Value),
                    Probability = decimal.Parse((string)InterAT.Rows[rows].Cells[1].Value)
                });
            }
            Submit.Text = "Running...";
            Submit.Enabled = false;
            operation.simulator(system);
            foreach (SimulationCase c in system.SimulationTable)
            {
                this.dataGridView2.Rows.Add(c.CustomerNumber.ToString(), c.RandomInterArrival.ToString(), c.InterArrival.ToString(), c.ArrivalTime.ToString(), c.RandomService.ToString(), c.ServiceTime.ToString(), c.AssignedServer.ID.ToString(), c.StartTime.ToString(), c.EndTime.ToString(), c.TimeInQueue.ToString());
            }
            this.panel1.Visible = true;


        }

        // Read  From File button
        private void read_from_file_Click(object sender, EventArgs e)
        {
            //SimulationSystem system = new SimulationSystem();
            string result;
            operation.ReadFromFile(sys,op.FileNumber);
            operation.simulator(sys);
            if (op.FileNumber == 1) result = TestingManager.Test(sys, Constants.FileNames.TestCase1);
            else if (op.FileNumber == 2) result = TestingManager.Test(sys, Constants.FileNames.TestCase2);
            else result = TestingManager.Test(sys, Constants.FileNames.TestCase3);

            foreach (SimulationCase c in sys.SimulationTable)
            {
                this.dataGridView2.Rows.Add(c.CustomerNumber.ToString(), c.RandomInterArrival.ToString(), 
                    c.InterArrival.ToString(), c.ArrivalTime.ToString(), c.RandomService.ToString(),
                    c.ServiceTime.ToString(), c.AssignedServer.ID.ToString(), c.StartTime.ToString(),
                    c.EndTime.ToString(), c.TimeInQueue.ToString());
            }
            this.panel1.Visible = true;
            MessageBox.Show(result);
       
            dataGridView2.DataSource = null;

        }
        int i = 1;
        //next server button
        private void NextServer_Click(object sender, EventArgs e)
        {
            NumOfS.Enabled = false;
            Server server = new Server();
            server.ID = i;

            for (int rows = 0; rows < dataGridView1.Rows.Count - 1; rows++)
            {
                server.TimeDistribution.Add(new TimeDistribution()
                {
                    Time = int.Parse((string)dataGridView1.Rows[rows].Cells[0].Value),
                    Probability = decimal.Parse((string)dataGridView1.Rows[rows].Cells[1].Value)
                });
            }
            dataGridView1.Rows.Clear();
            i++;
            system.Servers.Add(server);
            if (i == system.NumberOfServers)
            {
                NextServer.Text = "Submit Server";
            }
            if (i == system.NumberOfServers + 1)
            {
                NextServer.Enabled = false;
                dataGridView1.Enabled = false;
            }
            else
            {
                ServiceDistribution.Text = "Service Distribution : Server [ " + i + " ] ";
            }
        }

        // test butto
        private void TestButton_Click(object sender, EventArgs e)
        {
            //operation.ReadFromFile(sys);
            for (int rows = 0; rows < InterAT.Rows.Count - 1; rows++)
            {
                system.InterarrivalDistribution.Add(new TimeDistribution()
                {
                    Time = int.Parse((string)InterAT.Rows[rows].Cells[0].Value),
                    Probability = decimal.Parse((string)InterAT.Rows[rows].Cells[1].Value)
                });
            }
            //TestButton.Text = "Testing...";
            //TestButton.Enabled = false;
            operation.Test(system);
            //TestButton.Text = "Automatic Testing";
            //TestButton.Enabled = true;

        }

        private void ServiceDistribution_Click(object sender, EventArgs e)
        {

        }



        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        // clear button
        private void button2_Click(object sender, EventArgs e)
        {
            //Submit.Text = "Submit";
            //Submit.Enabled = true;
            /*foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
            }*/
            this.dataGridView2.Rows.Clear();
           
            dataGridView2.DataSource = null;
          
            
        }

        private void SelectionMethod_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            op.FileNumber = (int)FileNo.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Graph graph = new Graph(sys, 1);
           
            graph.Show();
        }
    }
}
