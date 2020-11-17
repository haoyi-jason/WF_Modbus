using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NModbus;
using NModbus.Serial;
using NModbus.Utility;
using System.IO;
using System.IO.Ports;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


using HY_INSTRUMENTS;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Remoting.Channels;

using STLINK_ADAPTER;
using System.Linq.Expressions;

namespace WF_Modbus
{
    public partial class Form1 : Form
    {
        delegate void HandleThreadDone(int state);
        delegate void HandleStateChange(int state);
        private const string benchFile = @"./bench_config.bch";
        private const string recipeFile = @"./recipe_config.rcp";
        HY_TestBench _testBench;
        SerialPort port;
        List<Recipedef> _recipe;
        Recipe _theRecipe;

        Recipes RecipeList;
        Recipe CurrentRecipe;

        ManualResetEvent mres = new ManualResetEvent(false);
        bool ExitThread = false;
        string CurrentBenchFile, CurrentRecipeFile;
        bool ReciipeRunning = false;
        int RecipeStart = -1, RecipeStop = -1;
        Thread ThRecipe = null;
        public Form1()
        {
            InitializeComponent();
            RecipeList = new Recipes();

            //_testBench = new HY_TestBench();
            //_testBench.SetPort(port);
            CurrentBenchFile = benchFile;
            CurrentRecipeFile = recipeFile;
            loadInstrumentDefinition(benchFile);
            dataGridView1.DataSource = _testBench.Instruments;
            
            loadRecipeDefinitions(recipeFile);
            foreach(Recipe r in RecipeList.HYRecipe)
            {
                lbRecipe.Items.Add(r.Name);
            }

            lbRecipe.SelectedIndex = 0;
            CurrentRecipe = RecipeList.HYRecipe[0];
            //dataGridView2.DataSource = _theRecipe.Recipes;
            dataGridView2.DataSource = RecipeList.HYRecipe[0].Recipes;
            //DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            //dataGridView1.Columns.Add(btn);
            //btn.HeaderText = "Click";
            //btn.Name = "btn";
            //btn.Text = "Read";
            //btn.UseColumnTextForButtonValue = true;

            //test();

            //Thread t1 = new Thread(worker);
            //t1.Start();
            ThRecipe = new Thread(recipe_worker);
        }

        private void S_WriteCommand(object sender, ushort[] cmd)
        {
            SRInstrument s = (SRInstrument)sender;
            string[] sl = s.InterfaceName.Split(',');

            using (SerialPort port = new SerialPort(sl[0], int.Parse(sl[1])))
            {
                port.Open();
                if (port.IsOpen)
                {
                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateRtuMaster(port);
                    master.WriteMultipleRegisters(s.ID, s.WriteAddress, cmd);
                }
            }
        }

        private void S_ReadCommand(object sender, ushort[] cmd)
        {
            SRInstrument s = (SRInstrument)sender;
            string[] sl = s.InterfaceName.Split(',');

            using (SerialPort port = new SerialPort(sl[0], int.Parse(sl[1])))
            {
                port.Open();
                if (port.IsOpen)
                {
                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateRtuMaster(port);
                    ushort[] rb = master.ReadHoldingRegisters(s.ID, s.ReadAddress, 2);
                    s.SetValue(rb);
                }
            }

        }

        void test()
        {
            
            //using (SerialPort port = new SerialPort("COM1"))
            //{
            //    port.BaudRate = 9600;
            //    port.DataBits = 8;
            //    port.Parity = Parity.None;
            //    port.StopBits = StopBits.One;
            //    port.Open();

            //    //SRInstrument psu = new SRInstrument(port);
            //    for(int i = 0; i < 10; i++)
            //    {
            //        psu.write();
            //        System.Threading.Thread.Sleep(1000);
            //    }
            //}

        }

        void masterRTUWriteRegister()
        {
            using (SerialPort port = new SerialPort("COM1"))
            {
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateRtuMaster(port);

                byte slaveID = 1;
                ushort startAddress = 100;
                ushort[] registers = new ushort[] { 1, 2, 3 };

                master.WriteMultipleRegisters(slaveID, startAddress, registers);
            }
        }

        void masterRTUWriteRegister(SRInstrument instrument)
        {
            if (instrument == null) return;
            string[] sl = instrument.InterfaceName.Split(',');
            if (sl.Length < 2) return;
            using (SerialPort port = new SerialPort(sl[0]))
            {
                port.BaudRate = int.Parse(sl[1]);
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                port.ReadTimeout = port.WriteTimeout = 2000;
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateRtuMaster(port);

                byte slaveID = instrument.ID;
                ushort startAddress = instrument.WriteAddress;
                ushort[] registers = instrument.PacketToWrite();

                master.Transport.ReadTimeout = master.Transport.WriteTimeout = 2000;
                //                master.WriteMultipleRegisters(slaveID, startAddress, registers);
                try
                {
                    master.WriteMultipleRegisters(slaveID, startAddress, registers);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        void masterRTUReadRegister(SRInstrument instrument)
        {
            if (instrument == null) return;
            string[] sl = instrument.InterfaceName.Split(',');
            if (sl.Length < 2) return;
            using (SerialPort port = new SerialPort(sl[0]))
            {
                port.BaudRate = int.Parse(sl[1]);
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateRtuMaster(port);
                master.Transport.ReadTimeout = master.Transport.WriteTimeout = 2000;

                byte slaveID = instrument.ID;
                ushort startAddress = instrument.ReadAddress;

                ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, 1);

                instrument.SetValue(registers);

                port.Close();
            }
        }

        private bool ConsoleWriteMessage(SRInstrument instrument)
        {
            bool success = false;
            if (instrument == null) return false;
            string[] sl = instrument.InterfaceName.Split(',');
            if (sl.Length < 2) return false;
            using (SerialPort port = new SerialPort(sl[0]))
            {
                port.BaudRate = int.Parse(sl[1]);
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                try
                {
                    port.Open();
                    string s = instrument.ValueToWrite + "\r\n";
                    port.Write(s);
                    System.Threading.Thread.Sleep(instrument.parameter);
                    instrument.ValueReadBack = port.ReadExisting();
                    port.Close();
                    success = true;
                }
                catch (Exception e)
                {
                    
                }

            }
            return success;
        }

        void SerialSendCommand(ASCInstrument instrument)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            int id = dataGridView1.SelectedCells[0].RowIndex;
            _testBench.InstrumentWrite(id);
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = dataGridView1.SelectedCells[0].RowIndex;
            _testBench.InstrumentRead(id);
        }

        private void worker()
        {
            for(int i=0;i<100;i++)
            {
                Console.WriteLine("Thread");
                System.Threading.Thread.Sleep(1000);
            }
        }


        private void btnExecRecipe_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(recipe_worker);
            t1.Start();

            //int id = dataGridView2.SelectedCells[0].RowIndex;

            //string instName = _theRecipe.Recipes.ElementAt(id).Instrument;
            //int instID;
            //if ((instID = _testBench.FindInstrument(instName)) >= 0)
            //{
            //    if (_recipe.ElementAt(id).RW == "R")
            //    {
            //        _testBench.InstrumentRead(instID);
            //    }
            //    else if (_recipe.ElementAt(id).RW == "W")
            //    {
            //        _testBench.Instruments.ElementAt(instID).ValueToWrite = _recipe.ElementAt(id).Value;
            //        _testBench.InstrumentWrite(instID);
            //    }
            //}
        }

        private void RecipeDone(int state)
        {
            if (InvokeRequired)
            {
                Invoke(new HandleThreadDone(ThreadDone),state);
            }
            else
            {
                ThreadDone(state);
            }
        }
        private void UpdateSelectedItem(int id)
        {
            if (InvokeRequired)
            {
                Invoke(new HandleStateChange(UpdateRecipeItem), id);
            }
            else
            {
                UpdateRecipeItem(id);
            }
        }

        private void ThreadDone(int state)
        {
            MessageBox.Show("Recipe Done!");
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            if(state == CurrentRecipe.Recipes.Count)
            {
                tbRecipeIndex.Text = "0";
            }

        }

        private void UpdateRecipeItem(int state)
        {
            dataGridView2.ClearSelection();
            dataGridView2.Rows[state].Selected = true;

            dataGridView3.ClearSelection();
            dataGridView3.Rows[state].Selected = true;

            if (state > 5) {
                dataGridView2.FirstDisplayedScrollingRowIndex = state-5;
                dataGridView3.FirstDisplayedScrollingRowIndex = state - 5;
            }
            else
            {
                dataGridView2.FirstDisplayedScrollingRowIndex = 0;
                dataGridView3.FirstDisplayedScrollingRowIndex = 0;
            }
            tbRecipeIndex.Text = (state+1).ToString();
        }
        private void recipe_worker()
        {
            int instID;
            int crow = RecipeStart == -1 ? 0 : RecipeStart;
            int end = RecipeStop == -1 ? CurrentRecipe.Recipes.Count : RecipeStop+1;
            int i;
            for(i = crow; i < end; i++)
            {
                UpdateSelectedItem(crow++);
                ExecuteRecipe(i);
            }
       
            RecipeDone(i);
        }
        
        private void loadDefaultInstrument()
        {
            _testBench = new HY_TestBench();
            _testBench.AddInstrument(new SRInstrument() { Name = "PSUV1", ID = 1, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "PSUA1", ID = 1, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "PSUEN1", ID = 1, Type = _vType.SHORT, ReadAddress = 8, WriteAddress = 8, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });

            _testBench.AddInstrument(new SRInstrument() { Name = "PSUV2", ID = 2, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "PSUA2", ID = 2, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "PSUEN2", ID = 2, Type = _vType.SHORT, ReadAddress = 8, WriteAddress = 8, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });

            _testBench.AddInstrument(new SRInstrument() { Name = "DCL01_A", ID = 3, Type = _vType.SHORT, ReadAddress = 13, WriteAddress = 13, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "DCL01_UVLO", ID = 3, Type = _vType.SHORT, ReadAddress = 14, WriteAddress = 14, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "DCL01_CTRL", ID = 3, Type = _vType.SHORT, ReadAddress = 17, WriteAddress = 17, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "DCL01_VMEAS", ID = 3, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });

            _testBench.AddInstrument(new SRInstrument() { Name = "DCV_01", ID = 6, Type = _vType.SHORT, ReadAddress = 8, WriteAddress = 8, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX100", ID = 11, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX101", ID = 11, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX102", ID = 11, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 2, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX103", ID = 11, Type = _vType.SHORT, ReadAddress = 3, WriteAddress = 3, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX104", ID = 11, Type = _vType.SHORT, ReadAddress = 4, WriteAddress = 4, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX105", ID = 11, Type = _vType.SHORT, ReadAddress = 5, WriteAddress = 5, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX106", ID = 11, Type = _vType.SHORT, ReadAddress = 6, WriteAddress = 6, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX107", ID = 11, Type = _vType.SHORT, ReadAddress = 7, WriteAddress = 7, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX200", ID = 12, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX201", ID = 12, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX202", ID = 12, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 2, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX203", ID = 12, Type = _vType.SHORT, ReadAddress = 3, WriteAddress = 3, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX204", ID = 12, Type = _vType.SHORT, ReadAddress = 4, WriteAddress = 4, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX205", ID = 12, Type = _vType.SHORT, ReadAddress = 5, WriteAddress = 5, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX206", ID = 12, Type = _vType.SHORT, ReadAddress = 6, WriteAddress = 6, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            _testBench.AddInstrument(new SRInstrument() { Name = "MUX207", ID = 12, Type = _vType.SHORT, ReadAddress = 7, WriteAddress = 7, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            
            _testBench.AddInstrument(new SRInstrument() { Name = "CON_DUMMY", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 1000 });
            _testBench.AddInstrument(new SRInstrument() { Name = "CON_DC", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 1000 });
            _testBench.AddInstrument(new SRInstrument() { Name = "CON_COMM", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 2000 });

            _testBench.AddInstrument(new SRInstrument() { Name = "STLINK", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 2000 });
        }

        private void SaveBench(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HY_TestBench));
            TextWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, _testBench);
            writer.Close();
        }

        private void LoadBench(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(HY_TestBench));
                FileStream fs = new FileStream(fileName, FileMode.Open);
                _testBench = (HY_TestBench)serializer.Deserialize(fs);
                fs.Close();
            }
        }
        private void loadInstrumentDefinition(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                loadDefaultInstrument();

                XmlSerializer serializer = new XmlSerializer(typeof(HY_TestBench));
                TextWriter writer = new StreamWriter(fileName);
                serializer.Serialize(writer, _testBench);
                writer.Close();
            }
            else
            {
                if (!File.Exists(fileName))
                {
                    loadDefaultInstrument();
                    SaveBench(fileName);
                    //XmlSerializer serializer = new XmlSerializer(typeof(HY_TestBench));
                    //TextWriter writer = new StreamWriter(benchFile);
                    //serializer.Serialize(writer, _testBench);
                    //writer.Close();
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(HY_TestBench));
                    FileStream fs = new FileStream(fileName, FileMode.Open);
                    _testBench = (HY_TestBench)serializer.Deserialize(fs);
                    fs.Close();
                }
            }
        }

        private void loadDefaultRecipes()
        {
            Recipe r1 = new Recipe("Image Flash");

            r1.Recipes.Add(new Recipedef() { Process = "SET Target Power", Instrument = "PSUV1", RW = "W", Value = "24", Delay = 200 });
            r1.Recipes.Add(new Recipedef() { Process = "SET Target Current",Instrument = "PSUA1", RW = "W", Value = "0.2", Delay = 200 });
            r1.Recipes.Add(new Recipedef() { Process = "Power On Target",Instrument = "PSUEN1", RW = "W", Value = "1", Delay = 200 });
            r1.Recipes.Add(new Recipedef() { Process = "FLASH", Instrument = "STLINK", RW = "W", Value = "d:\\temp\\17010_vmu_201113_ATE.hex", Delay = 200});
            r1.Recipes.Add(new Recipedef() { Process = "Power Off Target", Instrument = "PSUEN1", RW = "W", Value = "0", Delay = 200 });

            Recipe r2 = new Recipe("Power Test");
            r2.Recipes.Add(new Recipedef() { Process = "SET Target Power", Instrument = "PSUV1", RW = "W", Value = "24", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "SET Target Current", Instrument = "PSUA1", RW = "W", Value = "0.2", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Power On Target", Instrument = "PSUEN1", RW = "W", Value = "1", Delay = 200 });
            /********************/
            r2.Recipes.Add(new Recipedef() { Process = "Measure DC 12V", Instrument = "MUX101", RW = "W", Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "DC Output Wiring", Instrument = "MUX106", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "DC Load Wiring", Instrument = "MUX107", RW = "W", Value = "1", Delay = 200 });

            // DC 12V output
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Load 500mA", Instrument = "DCL01_A", RW = "W", Value = "0.5", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Load UVLO 10V", Instrument = "DCL01_UVLO", RW = "W", Value = "10", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable AUX Power", Instrument = "MUX100", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Send Dummy Command", Instrument = "CON_DUMMY", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable 12V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 1 1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read 12V DC Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable 12V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 1 0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable AUX Power", Instrument = "MUX100", RW = "W", Value = "0", Delay = 200 });
            // DC 5V output
            r2.Recipes.Add(new Recipedef() { Process = "Measure DC 5V", Instrument = "MUX101", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Load 500mA", Instrument = "DCL01_A", RW = "W", Value = "0.5", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Load UVLO 4V", Instrument = "DCL01_UVLO", RW = "W", Value = "4", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable AUX Power", Instrument = "MUX100", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Send Dummy Command", Instrument = "CON_DUMMY", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable 5V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 0 1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read 5V DC Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable 5V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 0 0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable AUX Power", Instrument = "MUX100", RW = "W", Value = "0", Delay = 200 });

            // Digital Input Test
            r2.Recipes.Add(new Recipedef() { Process = "Set Digital input to High State", Instrument = "MUX207", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read State", Instrument = "CON_COMM", RW = "W", Value = "di_read", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set Digital input to Low State", Instrument = "MUX207", RW = "W", Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read State", Instrument = "CON_COMM", RW = "W", Value = "di_read", Delay = 200 });

            // Digital Output Test
            /********************/
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Load 1 A", Instrument = "DCL01_A", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Load UVLO 10V", Instrument = "DCL01_UVLO", RW = "W", Value = "10", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "SET VEXT Voltage to 24V", Instrument = "PSUV2", RW = "W", Value = "24", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "SET VEXT Current to 2A", Instrument = "PSUA2", RW = "W", Value = "2", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Load Wiring", Instrument = "MUX106", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Load Wiring", Instrument = "MUX107", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Switch PSU2 to  VEXT", Instrument = "MUX103", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Power On VEXT", Instrument = "PSUEN2", RW = "W", Value = "1", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.0 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x01", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.0 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.1 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x02", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.1 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.2 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x04", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.2 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.3 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x08", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.3 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.4 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x10", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.4 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.5 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x20", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.5 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.6 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x40", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.6 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Enable CH.7 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x80", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read CH.7 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });


            // analog input
            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.0", Instrument = "MUX200", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.2", Instrument = "MUX201", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.4", Instrument = "MUX202", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.6", Instrument = "MUX203", RW = "W", Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 2V", Instrument = "DCV_01", RW = "W", Value = "2", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 4V", Instrument = "DCV_01", RW = "W", Value = "4", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.1", Instrument = "MUX200", RW = "W", Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.3", Instrument = "MUX201", RW = "W", Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.5", Instrument = "MUX202", RW = "W", Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.7", Instrument = "MUX203", RW = "W", Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 2V", Instrument = "DCV_01", RW = "W", Value = "2", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 4V", Instrument = "DCV_01", RW = "W", Value = "4", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });

            // RS-485 loop test
            r2.Recipes.Add(new Recipedef() { Process = "RS485 Loop test", Instrument = "CON_COMM", RW = "W", Value = "loop_rs485", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "CANBUS Loop test", Instrument = "CON_COMM", RW = "W", Value = "loop_can", Delay = 200 });

            r2.Recipes.Add(new Recipedef() { Process = "Power Off Target", Instrument = "PSUEN1", RW = "W", Value = "0", Delay = 200 });

            RecipeList.Name = "Recipe List";
            RecipeList.HYRecipe = new Recipe[] { r1 ,r2};
        }
        private void SaveRecipe(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Recipes));
            TextWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, RecipeList);
            writer.Close();
        }

        private void LoadRecipe(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                //XmlSerializer serializer = new XmlSerializer(typeof(Recipe));
                //FileStream fs = new FileStream(fileName, FileMode.Open);
                //_theRecipe = (Recipe)serializer.Deserialize(fs);
                //fs.Close();
                XmlSerializer serializer = new XmlSerializer(typeof(Recipes));
                FileStream fs = new FileStream(fileName, FileMode.Open);
                RecipeList = (Recipes)serializer.Deserialize(fs);
                fs.Close();
            }
        }

        private void loadRecipeDefinitions(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                loadDefaultRecipes();
                SaveRecipe(recipeFile);
            }
            else
            {
                if (!File.Exists(fileName))
                {
                    loadDefaultRecipes();
                    SaveRecipe(fileName);
                }
                else
                {
                    LoadRecipe(fileName);
                }
            }
        }

        private void btnInstRead_Click(object sender, EventArgs e)
        {
            int instID = dataGridView1.CurrentCell.RowIndex;
            SRInstrument s = _testBench.Instruments.ElementAt(instID);
            AccessInstrument(s,true);
//            masterRTUReadRegister(_testBench.Instruments.ElementAt(instID));
            dataGridView1.Refresh();
        }
        private void btnInstWrite_Click(object sender, EventArgs e)
        {
            int instID = dataGridView1.CurrentCell.RowIndex;
            SRInstrument s = _testBench.Instruments.ElementAt(instID);
            AccessInstrument(_testBench.Instruments.ElementAt(instID), false);
            //if (s.Name.Contains("CON_"))
            //{
            //    ConsoleWriteMessage(s);
            //}
            //else
            //{
            //    masterRTUWriteRegister(s);
            //}
        }

        private void btnInstSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentBenchFile))
            {
                SaveBench(CurrentBenchFile);
            }

        }
        private void btnSaveBenchAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = "Select File Name",
                Filter = "Bench file(*.bch)|*.bch",
                Title = "Save Bench file"

            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dialog.FileName))
                {
                    if (MessageBox.Show("File exist, override?", "Overwrite", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        SaveBench(dialog.FileName);
                    }
                }
                else
                {
                    SaveBench(dialog.FileName);
                }
            }
        }

        private void btnInstLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FileName = "Select a Bench file",
                Filter = "Bench file(*.bch)|*.bch",
                Title = "Open Bench file"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentBenchFile = dialog.FileName;
                LoadBench(CurrentBenchFile);
            }
        }

        private void btnAddInst_Click(object sender, EventArgs e)
        {
            _testBench.Instruments.Add(new SRInstrument() {ID=1 });
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _testBench.Instruments;
        }

        private void btnRemoveInst_Click(object sender, EventArgs e)
        {
            int instID = dataGridView1.CurrentCell.RowIndex;
            if (instID < _testBench.Instruments.Count)
            {
                dataGridView1.DataSource = null;
                _testBench.Instruments.RemoveAt(instID);
                dataGridView1.DataSource = _testBench.Instruments;
            }
        }
        private void btnInsertInstrument_Click(object sender, EventArgs e)
        {
            int instID = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.DataSource = null;
            _testBench.Instruments.Insert(instID, new SRInstrument());
            dataGridView1.DataSource = _testBench.Instruments;
        }

        private bool AccessInstrument(SRInstrument s, bool RW)
        {
            bool ret = false;
            if (s.Name.Contains("CON_"))
            {
//                s.ValueToWrite = r.Value;
                ConsoleWriteMessage(s);
                // valid?
            }
            else if (s.Name.Contains("STLINK"))
            {

            }
            else
            {
                if (RW)
                {
                    //_testBench.InstrumentRead(instID);
                    masterRTUReadRegister(s);
                }
                else // write
                {
                    masterRTUWriteRegister(s);
                    Thread.Sleep(200);
                    masterRTUReadRegister(s);

                }
            }
            return ret;
        }
        private bool ExecuteRecipe(int rcpid)
        {
            bool result = false;
            int instID;
            Recipedef r = CurrentRecipe.Recipes.ElementAt(rcpid);
            if (r != null)
            {
                if ((instID = _testBench.FindInstrument(r.Instrument)) >= 0)
                {
                    SRInstrument s = _testBench.Instruments.ElementAt(instID);
                    if (s.Name.Contains("CON_"))
                    {
                        s.ValueToWrite = r.Value;
                        ConsoleWriteMessage(s);
                        r.Response = s.ValueReadBack;
                        // valid?
                        if (s.ValueToWrite.Contains("di_read"))
                        {
                            string str = s.ValueReadBack.TrimEnd(new char[] { '\r', '\n',' '});
                            if (str == r.ExpectedResult)
                            {
                                r.Result = "PASS";
                            }
                            else
                            {
                                r.Result = "FAIL";
                            }
                        }
                        else if (s.ValueToWrite.Contains("do_write"))
                        {
                            r.Result = s.ValueReadBack;
                        }
                        else if (s.ValueToWrite.Contains("ai_read"))
                        {
                            string[] low = r.LowThreshold.Split(' ');
                            string[] high = r.HighThreshold.Split(' ');
                            string[] vals = r.Response.TrimEnd(new char[] { '\r', '\n' }).Split(' ');
                            string msg = "";
                            int l, h, v;
                            if((low.Length == 8) && (high.Length == 8) && (vals.Length >= 8))
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    bool fail = false;
                                    fail = !int.TryParse(low[i], out l);
                                    fail = !int.TryParse(high[i], out h);
                                    fail = !int.TryParse(vals[i], out v);
                                    if (fail)
                                    {
                                        r.Result = "Wrong Parameter format";
                                    }
                                    else
                                    {
                                        if ((v >= l) && (v <= h))
                                        {
                                            msg += "PASS";
                                        }
                                        else
                                        {
                                            msg += "FAIL";
                                        }
                                    }

                                }
                                r.Result = msg;
                            }
                            else
                            {
                                r.Result = "FAIL, 請檢查通訊埠";
                            }

                        }
                        else if (s.ValueToWrite.Contains("loop_rs485"))
                        {
                            if (s.ValueReadBack.Contains("OK"))
                            {
                                r.Result = "PASS";
                            }
                            else if (string.IsNullOrEmpty(s.ValueReadBack))
                            {
                                r.Result = "FAIL, 請檢查通訊埠";
                            }
                            else
                            {
                                r.Result = "FAIL";
                            }
                        }
                        else if (s.ValueToWrite.Contains("loop_can"))
                        {
                            if (s.ValueReadBack.Contains("OK"))
                            {
                                r.Result = "PASS";
                            }
                            else if (string.IsNullOrEmpty(s.ValueReadBack))
                            {
                                r.Result = "FAIL, 請檢查通訊埠";
                            }
                            else
                            {
                                r.Result = "FAIL";
                            }
                        }
                        else
                        {
                            r.Result = s.ValueReadBack;
                        }
                    }
                    else if (s.Name.Contains("STLINK"))
                    {
                        string msg;
                        if (ImageFlash(r.Value, out msg))
                        {
                            r.Result = "PASS";
                        }
                        else
                        {
                            r.Result = "FAIL";
                        }
                        r.Response = msg;
                    }
                    else
                    {
                        if (r.RW == "R")
                        {
                            masterRTUReadRegister(s);
                            r.Response =s.ValueReadBack;

                            if (r.Valid)
                            {
                                double low, high, v;
                                bool fail = false;
                                fail = !double.TryParse(r.LowThreshold, out low);
                                fail = !double.TryParse(r.HighThreshold, out high);
                                fail = !double.TryParse(r.Response, out v);

                                if (!fail)
                                {
                                    if ((v >= low) && (v <= high))
                                    {
                                        r.Result = "PASS";
                                    }
                                    else
                                    {
                                        r.Result = "FAIL";
                                    }

                                }
                                else
                                {
                                    r.Result = "Wrong Condition";
                                }
                            }
                            else
                            {
                                r.Result = "No Valid";
                            }
                        }
                        else if (r.RW == "W")
                        {
                            s.ValueToWrite = r.Value;
                            masterRTUWriteRegister(s);
                            Thread.Sleep(r.Delay);
                            masterRTUReadRegister(s);
                            r.Response = s.ValueReadBack;
                            // try to compare
                            double low, high, v;
                            bool fail = false;
                            fail = !double.TryParse(r.LowThreshold, out low);
                            fail = !double.TryParse(r.HighThreshold, out high);
                            fail = !double.TryParse(r.Response, out v);

                            if (!fail && r.Valid)
                            {
                                if ((v >= low) && (v <= high))
                                {
                                    r.Result = "PASS";
                                }
                                else
                                {
                                    r.Result = "Fail";
                                }

                            }
                            else
                            {
                                r.Result = "Not Valid";
                            }


                        }
                        else if (r.RW == "C") // check
                        {
                            //if(_testBench.Instruments.ElementAt(instID).ValueReadBack != _testBench.Instruments.ElementAt(instID).ValueToWrite)
                            //{
                            //    const string Value = "Read check Fail";
                            //    Console.WriteLine(Value);
                            //}
                            //else
                            //{
                            //    const string Value = "Read check OK";
                            //    Console.WriteLine(Value);
                            //}
                        }
                    }
                    Thread.Sleep(r.Delay);
                }
                else
                {
                   // throw error for no device found
                }
            }
            if (!InvokeRequired)
            {
                dataGridView2.Refresh();
            }
            return result;
        }

        private void btnExeRecipe_Click(object sender, EventArgs e)
        {
            int rcpId = dataGridView2.CurrentCell.RowIndex;
            ExecuteRecipe(rcpId);
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            CurrentRecipe.Recipes.Add(new Recipedef());
            dataGridView2.DataSource = CurrentRecipe.Recipes;
        }

        private void btnRemoveRecipe_Click(object sender, EventArgs e)
        {
            int rcpId = dataGridView2.CurrentCell.RowIndex;
            if(rcpId < CurrentRecipe.Recipes.Count)
            {
                dataGridView2.DataSource = null;
                CurrentRecipe.Recipes.RemoveAt(rcpId);
                dataGridView2.DataSource = CurrentRecipe.Recipes;
            }
        }

        private void btnInsertRecipe_Click(object sender, EventArgs e)
        {
            int rcpId = dataGridView2.CurrentCell.RowIndex;
            dataGridView2.DataSource = null;
            CurrentRecipe.Recipes.Insert(rcpId, new Recipedef());
            dataGridView2.DataSource = CurrentRecipe.Recipes;

        }

        private void btnLoadRecipe_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FileName = "Select a Recipe file",
                Filter = "Recipe file(*.rcp)|*.rcp",
                Title = "Open Recipe file"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentRecipeFile = dialog.FileName;
                LoadRecipe(CurrentRecipeFile);
            }
        }

        private void btnSaveRecipe_Click(object sender, EventArgs e)
        {
            SaveRecipe(CurrentRecipeFile);
        }

        private void lbRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = RecipeList.HYRecipe[lbRecipe.SelectedIndex].Recipes;
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = RecipeList.HYRecipe[lbRecipe.SelectedIndex].Recipes;
            CurrentRecipe = RecipeList.HYRecipe[lbRecipe.SelectedIndex];

            for(int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void lbRecipe_DoubleClick(object sender, EventArgs e)
        {
            //Thread t1 = new Thread(recipe_worker);

            //t1.Start();
            RunRecipe();
        }

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridView dg = (DataGridView)sender;
            ExecuteRecipe(dg.CurrentCell.RowIndex);
        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTestStep1_Click(object sender, EventArgs e)
        {
            lbRecipe.SelectedIndex = 0;
            RunRecipe();
        }

        private void btnTestStep2_Click(object sender, EventArgs e)
        {
            lbRecipe.SelectedIndex = 1;
            RunRecipe();
        }

        private void btnTestStep3_Click(object sender, EventArgs e)
        {
            lbRecipe.SelectedIndex = 2;
//            CurrentRecipe = RecipeList.HYRecipe[2];
            Thread t1 = new Thread(recipe_worker);

            t1.Start();

        }

        private void btnTestStep4_Click(object sender, EventArgs e)
        {
            lbRecipe.SelectedIndex = 3;
//            CurrentRecipe = RecipeList.HYRecipe[3];
            Thread t1 = new Thread(recipe_worker);

            t1.Start();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbWorkingDir.Text))
            {
                string fileName = tbWorkingDir.Text + "\\" + tbDUTSerial.Text;
                fileName += DateTime.Now.ToString("_yyyy_MM_dd-HH_mm");
                fileName += ".rcp";
                // todo: check if file presend
                RecipeList.DeviceID = tbDUTSerial.Text;
                RecipeList.TimeStamp = DateTime.Now.ToString("_yyyy_MM_dd-HH_mm_ss");
                SaveRecipe(fileName);
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FileName = "Select a hex file",
                Filter = "HEX file(*.hex)|*.hex",
                Title = "Open HEX file"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tbImagePath.Text = dialog.FileName;
            }
        }

        private bool ImageFlash(string fileName, out string msg)
        {
            bool success = true;
            STLinkAdapter stlink = new STLinkAdapter("");
            STLinkReturnCodes ret;
            string retstr;
            ret = stlink.FindSTLink(out retstr);
            //rtFlashMessage.Text = retstr;
            if (ret == STLinkReturnCodes.Failure)
            {
                MessageBox.Show("NO STLINK Found!");
                msg = retstr;
                return false;
            }
            string target = "F407xx";
            //var res = await Task.Run(() => stlink.ConnectToTarget(target, out retstr));
            ret = stlink.ConnectToTarget(target, out retstr);
            //rtFlashMessage.Text = retstr;
            if (ret == STLinkReturnCodes.Failure)
            {
                MessageBox.Show("STLINK Error!");
                msg = retstr;
                return false;
            }



            ret = stlink.ProgramTarget(fileName, 0x8000000, out retstr);
            //rtFlashMessage.Text = retstr;
            if (ret == STLinkReturnCodes.Failure)
            {
                MessageBox.Show("STLINK Flash Error!");
                msg = retstr;
                return false;
            }
            else
            {
                //rtFlashMessage.Text = "燒錄完成,請留意板上的綠色LED灯應該在閃爍!";
            }
            msg = retstr;
            return success;
        }
        private void btnImageFlash_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbImagePath.Text))
            {
                return;
            }
            STLinkAdapter stlink = new STLinkAdapter("");
            STLinkReturnCodes ret;
            string retstr;
            ret = stlink.FindSTLink(out retstr);
            rtFlashMessage.Text = retstr;
            if (ret == STLinkReturnCodes.Failure)
            {
                MessageBox.Show("NO STLINK Found!");
                return;
            }
            string target = "STM32F10xx";
            //var res = await Task.Run(() => stlink.ConnectToTarget(target, out retstr));
            ret = stlink.ConnectToTarget(target, out retstr);
            rtFlashMessage.Text = retstr;
            if (ret == STLinkReturnCodes.Failure)
            {
                MessageBox.Show("STLINK Error!");
                return;
            }



            ret = stlink.ProgramTarget(tbImagePath.Text, 0x8000000, out retstr);
            rtFlashMessage.Text = retstr;
            if (ret == STLinkReturnCodes.Failure)
            {
                MessageBox.Show("STLINK Flash Error!");
                return;
            }
            else
            {
                rtFlashMessage.Text = "燒錄完成,請留意板上的綠色LED灯應該在閃爍!";
            }
        }

        private void btnWorkingDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbWorkingDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach(DataGridViewRow r in dataGridView2.Rows)
            {
                r.HeaderCell.Value = r.Index + 1;
            }
        }

        private bool RunRecipe()
        {
            if (ThRecipe.IsAlive)
            {
                if (ThRecipe.ThreadState == ThreadState.Suspended)
                {
                    ThRecipe.Resume();
                }
                else
                {
                    ThRecipe.Suspend();
                }
//                    mres.Reset();
            }
            else
            {
                foreach(Recipedef r in CurrentRecipe.Recipes)
                {
                    r.Result = "";
                    r.Response = "";
                }
                ThRecipe = new Thread(recipe_worker);
                ThRecipe.Start();

            }

            return true;
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            //ExitThread = false;
            //mres.Set();
            RecipeStart = dataGridView2.CurrentCell.RowIndex;
            RecipeStop = CurrentRecipe.Recipes.Count;
        
            RunRecipe();

        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            RecipeStart = int.Parse(tbRecipeIndex.Text);
            RecipeStop = dataGridView2.CurrentCell.RowIndex;
            RunRecipe();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = "Select File Name",
                Filter = "Recipe file(*.rcp)|*.rcp",
                Title = "Save Recipe file"

            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dialog.FileName))
                {
                    if (MessageBox.Show("File exist, override?", "Overwrite", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        SaveRecipe(dialog.FileName);
                        CurrentRecipeFile = dialog.FileName;
                    }
                }
                else
                {
                    SaveRecipe(dialog.FileName);
                    CurrentRecipeFile = dialog.FileName;
                }

            }

        }

        private void btnAddRecipe_Click_1(object sender, EventArgs e)
        {
            int id = lbRecipe.SelectedIndex;
            List<Recipe> r = RecipeList.HYRecipe.ToList();
            r.Insert(id, new Recipe() { Name = "New Recipe" });
            dataGridView1.DataSource = null;
            RecipeList.HYRecipe = r.ToArray();
            dataGridView1.DataSource = RecipeList.HYRecipe;
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            if(e.ColumnIndex == 10)
            {
                if(e.Value.ToString().Contains("FAIL"))
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }
        }

        private void btnStopExec_Click(object sender, EventArgs e)
        {
            //ExitThread = true;
            //ThRecipe.Abort();
            //ThRecipe = null;
            if (ThRecipe.IsAlive)
            {
                ThRecipe.Abort();
            }
        }

    }
}
