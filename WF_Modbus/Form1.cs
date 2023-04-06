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


using System.Xml.Serialization;
using System.Runtime.Remoting.Channels;

//using STLINK_ADAPTER;
using System.Linq.Expressions;

//using MotionControlVD;

namespace WF_Modbus
{
    public partial class Form1 : Form
    {
        string AppName = "ATE";
        string Version = "1.0.5";
        delegate void HandleThreadDone(int state);
        delegate void HandleStateChange(int state);

        //private const string benchFile = @"./bench_config.bch";
        //private const string recipeFile = @"./recipe_config.rcp";
        //private const string cfgFilePath = @"./system_config.cfg";
        //private const string benchFile = @"./bms_bench_config.bch";
        //private const string recipeFile = @"./bms_recipe_config.rcp";
        private const string benchFile = @"./bms_bench_config3.bch";
        private const string recipeFile = @"./bms_recipe_config3.rcp";
        private const string cfgFilePath = @"./bms_system_config.cfg";
        private const string interfaceFile = @"./interface_config.cfg";

        HY_TestBench _testBench;
        HY_TB_Interfface _instruMentInterface;
        SerialPort port;
        List<Recipedef> _recipe;
        Recipe _theRecipe;

        Recipes RecipeList;
        List<int> RecipeToRun = new List<int>();
        Recipe CurrentRecipe;

        ManualResetEvent mres = new ManualResetEvent(false);
        bool ExitThread = false;
        string CurrentBenchFile, CurrentRecipeFile;
        bool ReciipeRunning = false;
        int RecipeStart = -1, RecipeStop = -1;
        Thread ThRecipe = null;

        CAN_Protocol canProto;
        CAN7565_HAL canBusDev = null;

        WF_MODBUS_System_Config SystemConfig;

        private RecipeExecute recipeExecutor;
        public Form1()
        {
            InitializeComponent();
            statusStrip1.Items.Add("Label1");
            statusStrip1.Items.Add("Label2");

            RecipeList = new Recipes();
            
            LoadConfig(cfgFilePath);
            tbWorkingDir.Text = SystemConfig.DefaultWorkDir;
            //tbUtilPath.Text = SystemConfig.UtilityPath;

            //_testBench = new HY_TestBench();
            //_testBench.SetPort(port);
            CurrentBenchFile = benchFile;
            CurrentRecipeFile = recipeFile;
            LoadInterfaceDefinition(interfaceFile);
            loadInstrumentDefinition(benchFile);
            dataGridView1.DataSource = _testBench.Instruments;
            
            loadRecipeDefinitions(recipeFile);

            //DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            //dataGridView1.Columns.Add(btn);ad
            //btn.HeaderText = "Click";
            //btn.Name = "btn";
            //btn.Text = "Read";
            //btn.UseColumnTextForButtonValue = true;

            //test();

            //Thread t1 = new Thread(worker);
            //t1.Start();
            ThRecipe = new Thread(recipe_worker);
            this.Text = AppName + "-" + Version;

            //canBusDev = new CAN7565_HAL();
            recipeExecutor = new RecipeExecute();
            recipeExecutor.OnIndexUpdate += OnRecipeDone;
            //re.OnRecipeDone += OnRecipeDone;
            recipeExecutor.SetInstrument(_testBench.Instruments);
        }

        private void S_WriteCommand(object sender, ushort[] cmd)
        {
            //SRInstrument s = (SRInstrument)sender;
            //string[] sl = s.InterfaceName.Split(',');

            //using (SerialPort port = new SerialPort(sl[0], int.Parse(sl[1])))
            //{
            //    port.Open();
            //    if (port.IsOpen)
            //    {
            //        var factory = new ModbusFactory();
            //        IModbusMaster master = factory.CreateRtuMaster(port);
            //        master.WriteMultipleRegisters(s.ID, s.WriteAddress, cmd);
            //    }
            //}
        }

        private void S_ReadCommand(object sender, ushort[] cmd)
        {
            //SRInstrument s = (SRInstrument)sender;
            //string[] sl = s.InterfaceName.Split(',');

            //using (SerialPort port = new SerialPort(sl[0], int.Parse(sl[1])))
            //{
            //    port.Open();
            //    if (port.IsOpen)
            //    {
            //        var factory = new ModbusFactory();
            //        IModbusMaster master = factory.CreateRtuMaster(port);
            //        ushort[] rb = master.ReadHoldingRegisters(s.ID, s.ReadAddress, 2);
            //        s.SetValue(rb);
            //    }
            //}

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

//        void masterRTUWriteRegister(SRInstrument instrument)
//        {
//            if (instrument == null) return;
//            string[] sl = instrument.InterfaceName.Split(',');
//            if (sl.Length < 2) return;
//            using (SerialPort port = new SerialPort(sl[0]))
//            {
//                port.BaudRate = int.Parse(sl[1]);
//                port.DataBits = 8;
//                port.Parity = Parity.None;
//                port.StopBits = StopBits.One;
//                port.Open();

//                port.ReadTimeout = port.WriteTimeout = 2000;
//                var factory = new ModbusFactory();
//                IModbusMaster master = factory.CreateRtuMaster(port);

//                byte slaveID = instrument.ID;
//                ushort startAddress = instrument.WriteAddress;
//                ushort[] registers = instrument.PacketToWrite();

//                master.Transport.ReadTimeout = master.Transport.WriteTimeout = 2000;
//                //                master.WriteMultipleRegisters(slaveID, startAddress, registers);
//                try
//                {
//                    master.WriteSingleRegister(slaveID, startAddress, registers[0]);
////                    master.WriteMultipleRegisters(slaveID, startAddress, registers);
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e.Message);
//                }
//            }
//        }

        //void masterRTUReadRegister(SRInstrument instrument)
        //{
        //    if (instrument == null) return;
        //    string[] sl = instrument.InterfaceName.Split(',');
        //    if (sl.Length < 2) return;
        //    using (SerialPort port = new SerialPort(sl[0]))
        //    {
        //        port.BaudRate = int.Parse(sl[1]);
        //        port.DataBits = 8;
        //        port.Parity = Parity.None;
        //        port.StopBits = StopBits.One;
        //        port.Open();

        //        var factory = new ModbusFactory();
        //        IModbusMaster master = factory.CreateRtuMaster(port);
        //        master.Transport.ReadTimeout = master.Transport.WriteTimeout = 2000;

        //        byte slaveID = instrument.ID;
        //        ushort startAddress = instrument.ReadAddress;

        //        ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, 1);

        //        instrument.SetValue(registers);

        //        port.Close();
        //    }
        //}

        //private bool ConsoleWriteMessage(SRInstrument instrument)
        //{
        //    bool success = false;
        //    if (instrument == null) return false;
        //    string[] sl = instrument.InterfaceName.Split(',');
        //    if (sl.Length < 2) return false;
        //    using (SerialPort port = new SerialPort(sl[0]))
        //    {
        //        port.BaudRate = int.Parse(sl[1]);
        //        port.DataBits = 8;
        //        port.Parity = Parity.None;
        //        port.StopBits = StopBits.One;
        //        try
        //        {
        //            port.Open();
        //            string s = instrument.ValueToWrite + "\r\n";
        //            port.Write(s);
        //            System.Threading.Thread.Sleep(instrument.parameter);
        //            instrument.ValueReadBack = port.ReadExisting();
        //            port.Close();
        //            success = true;
        //        }
        //        catch (Exception e)
        //        {
                    
        //        }

        //    }
        //    return success;
        //}

        //private  bool CANReadMessage(SRInstrument instrument)
        //{
        //    bool success = false;
        //    if (instrument == null) return false;
        //    string[] sl = instrument.InterfaceName.Split(',');
        //    if (sl.Length < 2) return false;

        //    int portNumber = int.Parse(sl[0]);

        //    if (instrument == null)
        //    {
        //        return false;
        //    }

        //    if(canBusDev == null)
        //    {
        //        canBusDev = new CAN7565_HAL(portNumber);
        //        if (!canBusDev.OpenDevice()) return false;
        //        canBusDev.startHeartBeat(1000, true);
        //    }
        //    //CAN7565_HAL hal = new CAN7565_HAL(portNumber);
        //    //if (!hal.OpenDevice()) return false;
        //    CANBUS_Packet packet;// = new CANBUS_Packet();
        //    //Thread.Sleep(1200);
        //    //success = canBusDev.RecvPacket(instrument.ReadAddress, out packet);
        //    List<CANBUS_Packet> packets = new List<CANBUS_Packet>();
        //    //canBusDev.RecvAllPacket(out packets);
        //    //canBusDev.CloseDevice();
        //    ushort[] d = null;
        //    success = canBusDev.readPacket(instrument.ReadAddress, out packet);
        //        if (success)
        //        {
        //            switch (instrument.ReadAddress)
        //            {
        //                case 0x110:
        //                case 0x111:
        //                case 0x112:
        //                    instrument.ValueReadBack = "";
        //                    d = instrument.Byte2UShorts(packet.Data);

        //                    for (int i = 0; i < d.Length; i++)
        //                    {
        //                        instrument.ValueReadBack += d[i].ToString();
        //                        if (i != (d.Length - 1))
        //                        {
        //                            instrument.ValueReadBack += ",";
        //                        }
        //                    }
        //                    break;
        //                case 0x113:
        //                    instrument.ValueReadBack = "";
        //                    d = instrument.Byte2UShorts(packet.Data);
        //                    instrument.ValueReadBack = "";
        //                    for (int i = 0; i < d.Length; i++)
        //                    {
        //                        instrument.ValueReadBack += d[i].ToString();
        //                        if (i != (d.Length - 1))
        //                        {
        //                            instrument.ValueReadBack += ",";
        //                        }
        //                    }
        //                    break;
        //                case 0x114:
        //                    instrument.ValueReadBack = "";
        //                    d = instrument.Byte2UShorts(packet.Data);
        //                    instrument.ValueReadBack = "";
        //                    for (int i = 0; i < 1; i++)
        //                    {
        //                        instrument.ValueReadBack += d[i].ToString();
        //                        if (i != (d.Length - 1))
        //                        {
        //                            instrument.ValueReadBack += ",";
        //                        }
        //                    }
        //                    break;
        //                case 0x115:
        //                    break;
        //                case 0x120: // svc
        //                instrument.ValueReadBack = "";
        //                d = instrument.Byte2UShorts(packet.Data);
        //                for (int i = 0; i < 4; i++)
        //                {
        //                    if(i == 2)
        //                    {
        //                        instrument.ValueReadBack += ((short)d[i]).ToString();
        //                    }
        //                    else
        //                    {
        //                        instrument.ValueReadBack += d[i].ToString();
        //                    }
        //                    if (i != (d.Length - 1))
        //                    {
        //                        instrument.ValueReadBack += ",";
        //                    }
        //                }
        //                break;
        //                case 0x121: // source current from bcu
        //                    instrument.ValueReadBack = "";
        //                    d = instrument.Byte2UShorts(packet.Data);
        //                    instrument.ValueReadBack = "";
        //                    for (int i = 0; i < 4; i++)
        //                    {
        //                        instrument.ValueReadBack += d[i].ToString();
        //                        if (i != (d.Length - 1))
        //                        {
        //                            instrument.ValueReadBack += ",";
        //                        }
        //                    }
        //                    break;
        //                default: break;
        //            }
        //        }
            
        //    return success;
        //}

        //private bool CANWriteMessage(SRInstrument instrument)
        //{
        //    bool success = false;
        //    if (instrument == null) return false;
        //    string[] sl = instrument.InterfaceName.Split(',');
        //    if (sl.Length < 3) return false;

        //    int portNumber = int.Parse(sl[0]);
        //    int ch = int.Parse(sl[2]);

        //    if(instrument == null)
        //    {
        //        return false;
        //    }
        //    if ((ch != 1) && (ch !=2)) return false;

        //    if(canBusDev == null)
        //    {
        //        canBusDev = new CAN7565_HAL(portNumber);
        //        if (!canBusDev.OpenDevice()) return false;
        //        canBusDev.startHeartBeat(1000, true);
        //    }

        //    //CAN7565_HAL hal = new CAN7565_HAL(portNumber);
        //    //if(!hal.OpenDevice()) return false;

        //    CANBUS_Packet packet = new CANBUS_Packet();

        //    packet.DeviceID = 1;
        //    packet.portID = 1;
        //    packet.extID = (uint)(instrument.WriteAddress | (instrument.ID << 12));
        //    packet.RTR = 0;

        //    if(instrument.ValueToWrite.Length%2 != 0)
        //    {
        //        return false;
        //    }
        //    //packet.data
        //    int i;
        //    for(i = 0; i < instrument.ValueToWrite.Length / 2; i++)
        //    {
        //        string bv = instrument.ValueToWrite.Substring(i * 2, 2);
        //        packet.Data[i] = Convert.ToByte(bv, 16);
        //    }
        //    packet.DLC = (byte)i;

        //    success = canBusDev.SendPacket(packet);
        //    Thread.Sleep(20);
        //    //canBusDev.CloseDevice();
        //    return success;
        //}

        //void SerialSendCommand(ASCInstrument instrument)
        //{

        //}
        private void button1_Click(object sender, EventArgs e)
        {
            //int id = dataGridView1.SelectedCells[0].RowIndex;
            //_testBench.InstrumentWrite(id);
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //int id = dataGridView1.SelectedCells[0].RowIndex;
            //_testBench.InstrumentRead(id);
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
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            if(state == CurrentRecipe.Recipes.Count)
            {
                tbRecipeIndex.Text = "0";

                if (RecipeToRun.Count > 0)
                {
                    lbRecipe.SelectedIndex = RecipeToRun.First();
                    RecipeToRun.RemoveAt(0);
                    ThRecipe.Abort();
                    System.Threading.Thread.Sleep(100);
                    RunRecipe();
                }
                else
                {
                    MessageBox.Show("Recipe Done!","Message");
                    lvResult.Clear();
                    //foreach (Recipe r in RecipeList.HYRecipe)
                    //{
                    //    for (int i = 0; i < r.Recipes.Count; i++)
                    //    {
                    //        if (r.Recipes[i].Result.Contains("FAIL"))
                    //        {
                    //            string s = "Recipe:" + r.Name + " ID=" + (i + 1).ToString();
                    //            lvResult.Items.Add(s);
                    //        }
                    //    }
                    //}
                    for (int i = 0; i < CurrentRecipe.Recipes.Count; i++)
                    {
                        if (CurrentRecipe.Recipes[i].Result.Contains("FAIL"))
                        {
                            string s = "Recipe:" + CurrentRecipe.Name + " ID=" + (i + 1).ToString();
                            lvResult.Items.Add(s);
                        }
                    }
                    if (lvResult.Items.Count == 0)
                    {
                        lvResult.Items.Add("測試通過");
                    }
                }
            }
            else if(state < 0)
            {
                MessageBox.Show("Recipe Error!", "Error");
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
            bool error = false;
            for(i = crow; i < end; i++)
            {
                UpdateSelectedItem(i);
                if (!ExecuteRecipe(i)){
                    error = true;
                    break;
                }
            }
            if (error)
            {
                RecipeDone(-1);
            }
            else
            {
                RecipeDone(i);
            }
        }
        
        private void loadDefaultInstrument()
        {
            _testBench = new HY_TestBench();
            MODBUS_Device mdev = new MODBUS_Device("MODBUS_DEV1", "PSU1", 1);
            mdev.AddRegisterDefines("VOUT", 0, MODBUS_REG_Struct._REG_TYPE.REG_S16, 100);
            mdev.AddRegisterDefines("AOUT", 0, MODBUS_REG_Struct._REG_TYPE.REG_S16, 100);
            _testBench.AddInstrument(mdev);
            _testBench.AddInstrument(new MODBUS_Device("MODBUS_DEV2", "DC_SOURCE", 1));
            _testBench.AddInstrument(new MODBUS_Device("MODBUS_DEV3", "DC_SOURCE", 1));

            //_testBench.AddInstrument(new SRInstrument() { Name = "PSUV1", ID = 1, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "PSUA1", ID = 1, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "PSUEN1", ID = 1, Type = _vType.SHORT, ReadAddress = 8, WriteAddress = 8, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "PSUV2", ID = 2, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "PSUA2", ID = 2, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "PSUEN2", ID = 2, Type = _vType.SHORT, ReadAddress = 8, WriteAddress = 8, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "DCL01_A", ID = 3, Type = _vType.SHORT, ReadAddress = 13, WriteAddress = 13, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "DCL01_UVLO", ID = 3, Type = _vType.SHORT, ReadAddress = 14, WriteAddress = 14, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "DCL01_CTRL", ID = 3, Type = _vType.SHORT, ReadAddress = 17, WriteAddress = 17, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "DCL01_VMEAS", ID = 3, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "DCV_01", ID = 6, Type = _vType.SHORT, ReadAddress = 8, WriteAddress = 8, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX100", ID = 11, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX101", ID = 11, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX102", ID = 11, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 2, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX103", ID = 11, Type = _vType.SHORT, ReadAddress = 3, WriteAddress = 3, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX104", ID = 11, Type = _vType.SHORT, ReadAddress = 4, WriteAddress = 4, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX105", ID = 11, Type = _vType.SHORT, ReadAddress = 5, WriteAddress = 5, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX106", ID = 11, Type = _vType.SHORT, ReadAddress = 6, WriteAddress = 6, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX107", ID = 11, Type = _vType.SHORT, ReadAddress = 7, WriteAddress = 7, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX200", ID = 12, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX201", ID = 12, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 1, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX202", ID = 12, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 2, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX203", ID = 12, Type = _vType.SHORT, ReadAddress = 3, WriteAddress = 3, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX204", ID = 12, Type = _vType.SHORT, ReadAddress = 4, WriteAddress = 4, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX205", ID = 12, Type = _vType.SHORT, ReadAddress = 5, WriteAddress = 5, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX206", ID = 12, Type = _vType.SHORT, ReadAddress = 6, WriteAddress = 6, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX207", ID = 12, Type = _vType.SHORT, ReadAddress = 7, WriteAddress = 7, InterfaceName = "COM1,9600,N,8,1,N", parameter = 0 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "CON_DUMMY", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 1000 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CON_DC", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 1000 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CON_COMM", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 2000 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "STLINK", ID = 4, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM1,9600,N,8,1,N", parameter = 2000 });

            // below for BMU test

            //_testBench.AddInstrument(new SRInstrument() { Name = "TIMER", ID = 1, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX_DC", ID = 1, Type = _vType.SHORT, ReadAddress = 0, WriteAddress = 0, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX_BAT", ID = 1, Type = _vType.SHORT, ReadAddress = 1, WriteAddress = 1, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX_FLASH", ID = 1, Type = _vType.SHORT, ReadAddress = 2, WriteAddress = 7, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX_DO1", ID = 1, Type = _vType.SHORT, ReadAddress = 4, WriteAddress = 4, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX_DO0", ID = 1, Type = _vType.SHORT, ReadAddress = 5, WriteAddress = 5, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX_DI1", ID = 1, Type = _vType.SHORT, ReadAddress = 6, WriteAddress = 6, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "MUX_DI0", ID = 1, Type = _vType.SHORT, ReadAddress = 7, WriteAddress = 7, InterfaceName = "COM5,9600,N,8,1,N", parameter = 0 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_STAT0", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x110, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_STAT1", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x111, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_STAT2", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x112, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_STAT3", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x113, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_STAT4", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x114, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_STAT5", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x115, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_ID", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x99, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_BC", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x132, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "CAN_NTCCFG", ID = 0x21, Type = _vType.SHORT, ReadAddress = 0x132, WriteAddress = 0x130, InterfaceName = "10,icpdas_i7565,2", parameter = 0 });

            //_testBench.AddInstrument(new SRInstrument() { Name = "BCU_RELAY", ID = 0x1, Type = _vType.SHORT, ReadAddress = 0x140, WriteAddress = 0x140, InterfaceName = "10,icpdas_i7565,1", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "BCU_VSOURCE", ID = 0x1, Type = _vType.SHORT, ReadAddress = 0x180, WriteAddress = 0x140, InterfaceName = "10,icpdas_i7565,1", parameter = 0 });
            //_testBench.AddInstrument(new SRInstrument() { Name = "BCU_SENSE", ID = 0x1, Type = _vType.SHORT, ReadAddress = 0x121, WriteAddress = 0x140, InterfaceName = "10,icpdas_i7565,1", parameter = 0 });
        }

        private void LoadConfig(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
               
                XmlSerializer serializer = new XmlSerializer(typeof(WF_MODBUS_System_Config));
                FileStream fs = new FileStream(fileName, FileMode.Open);
                SystemConfig = (WF_MODBUS_System_Config)serializer.Deserialize(fs);
                fs.Close();
            }
            else
            {
                SystemConfig = new WF_MODBUS_System_Config();
                SaveConfig(fileName);
            }
            statusStrip1.Items[1].Text = "Config File:"+fileName;
        }

        private void SaveConfig(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WF_MODBUS_System_Config));
                TextWriter writer = new StreamWriter(fileName);
                serializer.Serialize(writer, SystemConfig);
                writer.Close();
            }
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
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _testBench.Instruments;
                fs.Close();
            }
        }

        private void loadDefaultInterfaceDefinition()
        {
            _instruMentInterface = new HY_TB_Interfface();

            ModbusInterface m = new ModbusInterface("COM1,9600,N,8,1,N", ProtocolInterface.Protocol.MODBUS);
            _instruMentInterface.Add_Interface(m);
        }

        private void LoadInterfaceDefinition(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(HY_TB_Interfface));

                FileStream fs = new FileStream(fileName, FileMode.Open);
                _instruMentInterface = (HY_TB_Interfface)serializer.Deserialize(fs);
                dgv_interface.DataSource = null;
                dgv_interface.DataSource = _instruMentInterface.Interfaces;
                fs.Close();
            }
            else
            {
                loadDefaultInterfaceDefinition();
                SaveInterfaceDefinition(fileName);
            }

            dgv_interface.DataSource = _instruMentInterface.Interfaces;

            _instruMentInterface.InitialConfig();
        }

        private void SaveInterfaceDefinition(string fileName)
        {
            XmlAttributeOverrides attrOverWrite = new XmlAttributeOverrides();

            XmlAttributes attrs = new XmlAttributes();
            XmlRootAttribute attr = new XmlRootAttribute();

            attr.ElementName = "MODBUS_Device";
            attrs.XmlRoot = attr;
            attrOverWrite.Add(typeof(HY_TB_Interfface), attrs);
            XmlSerializer serializer = new XmlSerializer(typeof(HY_TB_Interfface));
            TextWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, _instruMentInterface);
            writer.Close();
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
            statusStrip1.Items[0].Text = "Bench File:" + fileName;

            _testBench.InitialConfig();
            foreach(Abstract_Instrument ins in _testBench.Instruments)
            {
                ins.SetInterface(_instruMentInterface.FindInterfaceByName(ins.InterfaceName));
                ins.StartProc();
            }
        }

        private void loadDefaultRecipes()
        {
            Recipe r1 = new Recipe("Image Flash");

            r1.Recipes.Add(new Recipedef() { Process = "SET Target Power", Instrument = "PSU1", RW = "W", Function = "AOUT", Value = "0.5", Delay = 200 });
            r1.Recipes.Add(new Recipedef() { Process = "SET Target Power", Instrument = "PSU1", RW = "W",Function="VOUT", Value = "24", Delay = 200 });
            r1.Recipes.Add(new Recipedef() { Process = "Power On Target", Instrument = "PSU1", RW = "W",Function="CTRL", Value = "1", Delay = 200 });
            r1.Recipes.Add(new Recipedef() { Process = "FLASH", Instrument = "STLINK", RW = "W", Value = "d:\\temp\\17010_vmu_201113_ATE.hex", Delay = 200 });
            r1.Recipes.Add(new Recipedef() { Process = "Power Off Target", Instrument = "PSU1", RW = "W",Function="CTRL", Value = "0", Delay = 200 });

            Recipe r2 = new Recipe("Power Test");
            r2.Recipes.Add(new Recipedef() { Process = "SET Target Power", Instrument = "PSU1", RW = "W", Function = "AOUT", Value = "0.5", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "SET Target Power", Instrument = "PSU1", RW = "W", Function = "VOUT", Value = "24", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "Power On Target", Instrument = "PSU1", RW = "W", Function = "CTRL", Value = "1", Delay = 200 });
            /********************/
            r2.Recipes.Add(new Recipedef() { Process = "Measure DC 12V", Instrument = "MUX1", RW = "W", Function="CH1",Value = "0", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "DC Output Wiring", Instrument = "MUX1", RW = "W",Function = "CH6" ,Value = "1", Delay = 200 });
            r2.Recipes.Add(new Recipedef() { Process = "DC Load Wiring", Instrument = "MUX1", RW = "W", Function = "CH7",Value = "1", Delay = 200 });

            //// DC 12V output
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Load 500mA", Instrument = "DCL01_A", RW = "W", Value = "0.5", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Load UVLO 10V", Instrument = "DCL01_UVLO", RW = "W", Value = "10", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable AUX Power", Instrument = "MUX100", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Send Dummy Command", Instrument = "CON_DUMMY", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable 12V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 1 1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read 12V DC Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable 12V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 1 0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable AUX Power", Instrument = "MUX100", RW = "W", Value = "0", Delay = 200 });
            //// DC 5V output
            //r2.Recipes.Add(new Recipedef() { Process = "Measure DC 5V", Instrument = "MUX101", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Load 500mA", Instrument = "DCL01_A", RW = "W", Value = "0.5", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Load UVLO 4V", Instrument = "DCL01_UVLO", RW = "W", Value = "4", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable AUX Power", Instrument = "MUX100", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Send Dummy Command", Instrument = "CON_DUMMY", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable 5V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 0 1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read 5V DC Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable 5V DC Output", Instrument = "CON_DC", RW = "W", Value = "dc 0 0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable AUX Power", Instrument = "MUX100", RW = "W", Value = "0", Delay = 200 });

            //// Digital Input Test
            //r2.Recipes.Add(new Recipedef() { Process = "Set Digital input to High State", Instrument = "MUX207", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read State", Instrument = "CON_COMM", RW = "W", Value = "di_read", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set Digital input to Low State", Instrument = "MUX207", RW = "W", Value = "0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read State", Instrument = "CON_COMM", RW = "W", Value = "di_read", Delay = 200 });

            //// Digital Output Test
            ///********************/
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Load 1 A", Instrument = "DCL01_A", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Load UVLO 10V", Instrument = "DCL01_UVLO", RW = "W", Value = "10", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "SET VEXT Voltage to 24V", Instrument = "PSUV2", RW = "W", Value = "24", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "SET VEXT Current to 2A", Instrument = "PSUA2", RW = "W", Value = "2", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Load Wiring", Instrument = "MUX106", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Load Wiring", Instrument = "MUX107", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Switch PSU2 to  VEXT", Instrument = "MUX103", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Power On VEXT", Instrument = "PSUEN2", RW = "W", Value = "1", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.0 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x01", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.0 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.1 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x02", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.1 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.2 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x04", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.2 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.3 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x08", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.3 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.4 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x10", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.4 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.5 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x20", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.5 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.6 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x40", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.6 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Enable CH.7 Output", Instrument = "CON_COMM", RW = "W", Value = "do_write 0x80", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Enable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read CH.7 Output", Instrument = "V_MEAS", RW = "R", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Disable DC Load", Instrument = "DCL01_CTRL", RW = "W", Value = "0", Delay = 200 });


            //// analog input
            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.0", Instrument = "MUX200", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.2", Instrument = "MUX201", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.4", Instrument = "MUX202", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.6", Instrument = "MUX203", RW = "W", Value = "1", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 2V", Instrument = "DCV_01", RW = "W", Value = "2", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 4V", Instrument = "DCV_01", RW = "W", Value = "4", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.1", Instrument = "MUX200", RW = "W", Value = "0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.3", Instrument = "MUX201", RW = "W", Value = "0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.5", Instrument = "MUX202", RW = "W", Value = "0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set Input Ch.7", Instrument = "MUX203", RW = "W", Value = "0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 2V", Instrument = "DCV_01", RW = "W", Value = "2", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Set DC Source 4V", Instrument = "DCV_01", RW = "W", Value = "4", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Read Analog Input", Instrument = "CON_COMM", RW = "W", Value = "ai_read", Delay = 200 });

            //// RS-485 loop test
            //r2.Recipes.Add(new Recipedef() { Process = "RS485 Loop test", Instrument = "CON_COMM", RW = "W", Value = "loop_rs485", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "CANBUS Loop test", Instrument = "CON_COMM", RW = "W", Value = "loop_can", Delay = 200 });

            //r2.Recipes.Add(new Recipedef() { Process = "Power Off Target", Instrument = "PSUEN1", RW = "W", Value = "0", Delay = 200 });

            //Recipe r1 = new Recipe("BMU Flash");
            //r1.Recipes.Add(new Recipedef() { Process = "SET Target Power ON", Instrument = "BCU_VSOURCE", RW = "W", Value = "0,1,200", Delay = 200 }); // ch-en-ma
            //r1.Recipes.Add(new Recipedef() { Process = "Read current consumption", Instrument = "BCU_SENSE", RW = "R", Value = "", Delay = 200 });
            //r1.Recipes.Add(new Recipedef() { Process = "Toggle Flash Low", Instrument = "BCU_RELAY", RW = "W", Value = "1", Delay = 200 });
            //r1.Recipes.Add(new Recipedef() { Process = "Toggle Flash High", Instrument = "BCU_RELAY", RW = "W", Value = "0", Delay = 200 });
            //r1.Recipes.Add(new Recipedef() { Process = "Wait Flash Finish", Instrument = "TIMER", RW = "W", Value = "5000", Delay = 200 });
            //r1.Recipes.Add(new Recipedef() { Process = "SET Target Power OFF", Instrument = "BCU_VSOURCE", RW = "W", Value = "0,0,200", Delay = 200 });

            //Recipe r2 = new Recipe("BMU Config");
            //r2.Recipes.Add(new Recipedef() { Process = "SET Target Power ON", Instrument = "BCU_VSOURCE", RW = "W", Value = "0,1,200", Delay = 200 });
            ////r2.Recipes.Add(new Recipedef() { Process = "Set Cell Queue to 8-cells", Instrument = "BC", RW = "W", Value = "", Delay = 200 });
            ////r2.Recipes.Add(new Recipedef() { Process = "Toggle Flash High", Instrument = "MUX_FLASH", RW = "W", Value = "0", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "Wait Flash Finish", Instrument = "TIMER", RW = "W", Value = "5000", Delay = 200 });
            //r2.Recipes.Add(new Recipedef() { Process = "SET Target Power OFF", Instrument = "BCU_VSOURCE", RW = "W", Value = "0,1,200", Delay = 200 });

            //Recipe r3 = new Recipe("BMU Data Receive");
            //r3.Recipes.Add(new Recipedef() { Process = "SET Target Power ON", Instrument = "BCU_VSOURCE", RW = "W", Value = "0,1,200", Delay = 200 });
            //r3.Recipes.Add(new Recipedef() { Process = "Wait BMU Ready", Instrument = "TIMER", RW = "W", Value = "2000", Delay = 200 });
            //r3.Recipes.Add(new Recipedef() { Process = "READ CELL VOLT", Instrument = "CAN_STAT0", RW = "R", Value = "1", Delay = 20 });
            //r3.Recipes.Add(new Recipedef() { Process = "READ CELL VOLT", Instrument = "CAN_STAT1", RW = "R", Value = "1", Delay = 20 });
            //r3.Recipes.Add(new Recipedef() { Process = "READ CELL VOLT", Instrument = "CAN_STAT2", RW = "R", Value = "1", Delay = 20 });
            //r3.Recipes.Add(new Recipedef() { Process = "READ CELL VOLT", Instrument = "CAN_STAT3", RW = "R", Value = "1", Delay = 20 });
            //r3.Recipes.Add(new Recipedef() { Process = "READ CELL VOLT", Instrument = "CAN_STAT4", RW = "R", Value = "1", Delay = 20 });
            //r3.Recipes.Add(new Recipedef() { Process = "READ CELL VOLT", Instrument = "CAN_STAT5", RW = "R", Value = "1", Delay = 20 });

            //r3.Recipes.Add(new Recipedef() { Process = "SET Target Power OFF", Instrument = "BCU_VSOURCE", RW = "W", Value = "0,0,200", Delay = 200 });
            RecipeList.Name = "Recipe List";
            //RecipeList.HYRecipe = new Recipe[] { r1 ,r2};
            RecipeList.HYRecipe.Add(r1);
            RecipeList.HYRecipe.Add(r2);
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

                foreach(Recipe r in RecipeList.HYRecipe)
                {
                    foreach(Recipedef rd in r.Recipes)
                    {
                        rd.Response = "";
                    }
                }
            }
        }

        private void UpdateRecipeContents()
        {
            lbRecipe.Items.Clear();
            Button b;
            int top = 10;
            int left = 10;
            int h = (int)(groupBox7.Height * 0.8);

            int id = 0;
            foreach (Recipe r in RecipeList.HYRecipe)
            {
                lbRecipe.Items.Add(r.Name);
                b = new Button();
                b.Text = r.Name;
                b.Click += ExecRecipeClick;
                b.MouseUp += SelectRecipe;
                b.Name = r.Name;
                b.Width = b.Height = h;
                b.Top = 20;
                b.Left = 10 + id * (h + 10);
                id++;
                groupBox2.Controls.Add(b);
            }
            b = new Button();
            b.Text = "ALL";
            b.Click += ExecRecipeClick;
            b.Name = "ALL";
            b.Width = b.Height = (int)(groupBox7.Height * 0.8);
            b.Top = 20;
            b.Left = id * (h + 10) + 10;
            groupBox2.Controls.Add(b);

            CurrentRecipe = RecipeList.HYRecipe[0];
            dataGridView2.DataSource = CurrentRecipe;
            dataGridView3.DataSource = CurrentRecipe;
            lbRecipe.SelectedIndex = 0;
        }

        private void SelectRecipe(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                Button b = (Button)sender;
                for (int i = 0; i < RecipeList.HYRecipe.Count; i++)
                {
                    if (RecipeList.HYRecipe[i].Name == b.Name)
                    {
                        dataGridView2.DataSource = null;
                        dataGridView2.DataSource = RecipeList.HYRecipe[i].Recipes;
                        dataGridView3.DataSource = null;
                        dataGridView3.DataSource = RecipeList.HYRecipe[i].Recipes;
                        CurrentRecipe = RecipeList.HYRecipe[i];

                        UpdateGridItem();
                    }
                }
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

            UpdateRecipeContents();

            UpdateGridItem();
        }

        private void ExecRecipeClick(object sender, EventArgs e)
        {
            RecipeToRun.Clear();

            Button b = (Button)sender;
            if(b.Name == "ALL")
            {
                for (int i = 0; i < RecipeList.HYRecipe.Count; i++)
                {
                    foreach(Recipe r in RecipeList.HYRecipe)
                    {
                        foreach(Recipedef rd in r.Recipes)
                        {
                            rd.Result = "";
                            rd.Response = "";
                        }
                    }
                    if(RecipeList.HYRecipe[i].Name != "Initial")
                        RecipeToRun.Add(i);
                }
            }
            else
            {
                for (int i = 0; i < RecipeList.HYRecipe.Count; i++)
                {
                    if (RecipeList.HYRecipe[i].Name == b.Name)
                    {
                        foreach (Recipedef rd in RecipeList.HYRecipe[i].Recipes)
                        {
                            rd.Result = "";
                            rd.Response = "";
                        }
                        RecipeToRun.Add(i);
                    }
                }
            }

            if(RecipeToRun.Count > 0)
            {
                lbRecipe.SelectedIndex = RecipeToRun.First();
//                CurrentRecipe = RecipeList.HYRecipe[RecipeToRun.First()];
                RecipeToRun.RemoveAt(0);
                RunRecipe();
            }
        }

        private void btnInstRead_Click(object sender, EventArgs e)
        {
            //int instID = dataGridView1.CurrentCell.RowIndex;
            //SRInstrument s = _testBench.Instruments.ElementAt(instID);
//            AccessInstrument(s,true);
//            masterRTUReadRegister(_testBench.Instruments.ElementAt(instID));
            //dataGridView1.Refresh();
        }
        private void btnInstWrite_Click(object sender, EventArgs e)
        {
            //int instID = dataGridView1.CurrentCell.RowIndex;
            //SRInstrument s = _testBench.Instruments.ElementAt(instID);
            //AccessInstrument(_testBench.Instruments.ElementAt(instID), false);
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
                if(MessageBox.Show("警告!", "檔案已存在,覆寫檔案?",MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    SaveBench(CurrentBenchFile);
                }
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
            //_testBench.Instruments.Add(new SRInstrument() {ID=1 });
            //dataGridView1.DataSource = null;
            //dataGridView1.DataSource = _testBench.Instruments;
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
            //int instID = dataGridView1.CurrentCell.RowIndex;
            //dataGridView1.DataSource = null;
            //_testBench.Instruments.Insert(instID, new MODBUS_Device());
            //dataGridView1.DataSource = _testBench.Instruments;
        }

//        private bool AccessInstrument(SRInstrument s, bool RW)
//        {
//            bool ret = false;
//            if (s.Name.Contains("CON_"))
//            {
////                s.ValueToWrite = r.Value;
//                ConsoleWriteMessage(s);
//                // valid?
//            }
//            else if (s.Name.Contains("STLINK"))
//            {

//            }
//            else if (s.Name.Contains("CAN"))
//            {
//                if (RW)
//                {
//                    CANReadMessage(s);
                   
//                }
//                else
//                {
//                    CANWriteMessage(s);
//                }
//            }
//            else
//            {
//                if (RW)
//                {
//                    //_testBench.InstrumentRead(instID);
//                    masterRTUReadRegister(s);
//                }
//                else // write
//                {
//                    masterRTUWriteRegister(s);
//                    Thread.Sleep(200);
//                    masterRTUReadRegister(s);

//                }
//            }
//            return ret;
//        }
        private bool ExecuteRecipe(int rcpid)
        {
            bool result = true;

            Recipedef r = CurrentRecipe.Recipes.ElementAt(rcpid);
            if (r != null)
            {
                RecipeExecute re = new RecipeExecute();
                re.SetInstrument(_testBench.Instruments);
                re.ExecuteRecipe(r);
            }

            //int instID;
            //Recipedef r = CurrentRecipe.Recipes.ElementAt(rcpid);
            //if (r != null)
            //{
            //    if ((instID = _testBench.FindInstrument(r.Instrument)) >= 0)
            //    {
            //        SRInstrument s = _testBench.Instruments.ElementAt(instID);
            //        if (s.Name.Contains("CON_"))
            //        {
            //            s.ValueToWrite = r.Value;
            //            ConsoleWriteMessage(s);
            //            r.Response = s.ValueReadBack;
            //            // valid?
            //            if (s.ValueToWrite.Contains("di_read"))
            //            {
            //                string str = s.ValueReadBack.TrimEnd(new char[] { '\r', '\n',' '});
            //                if (str == r.ExpectedResult)
            //                {
            //                    r.Result = "PASS";
            //                }
            //                else
            //                {
            //                    r.Result = "FAIL";
            //                }
            //                r.Response = str;
            //            }
            //            else if (s.ValueToWrite.Contains("do_write"))
            //            {
            //                r.Result = s.ValueReadBack;
            //                r.Response = s.ValueReadBack;
            //            }
            //            else if (s.ValueToWrite.Contains("ai_read"))
            //            {
            //                string[] low = r.LowThreshold.Split(' ');
            //                string[] high = r.HighThreshold.Split(' ');
            //                string[] vals = r.Response.TrimEnd(new char[] { '\r', '\n' }).Split(' ');
            //                string msg = "";
            //                int l, h, v;
            //                if((low.Length == 8) && (high.Length == 8) && (vals.Length >= 8))
            //                {
            //                    for (int i = 0; i < 8; i++)
            //                    {
            //                        bool fail = false;
            //                        fail = !int.TryParse(low[i], out l);
            //                        fail = !int.TryParse(high[i], out h);
            //                        fail = !int.TryParse(vals[i], out v);
            //                        if (fail)
            //                        {
            //                            r.Result = "Wrong Parameter format";
            //                        }
            //                        else
            //                        {
            //                            if ((v >= l) && (v <= h))
            //                            {
            //                                msg += "PASS";
            //                            }
            //                            else
            //                            {
            //                                msg += "FAIL";
            //                            }
            //                        }

            //                    }
            //                    r.Result = msg;
            //                }
            //                else
            //                {
            //                    r.Result = "FAIL, 請檢查通訊埠";
            //                    result = false;
            //                }

            //            }
            //            else if (s.ValueToWrite.Contains("loop_rs485"))
            //            {
            //                if (s.ValueReadBack.Contains("OK"))
            //                {
            //                    r.Result = "PASS";
            //                }
            //                else if (string.IsNullOrEmpty(s.ValueReadBack))
            //                {
            //                    r.Result = "FAIL, 請檢查通訊埠";
            //                }
            //                else
            //                {
            //                    r.Result = "FAIL";
            //                }
            //            }
            //            else if (s.ValueToWrite.Contains("loop_can"))
            //            {
            //                if (s.ValueReadBack.Contains("OK"))
            //                {
            //                    r.Result = "PASS";
            //                }
            //                else if (string.IsNullOrEmpty(s.ValueReadBack))
            //                {
            //                    r.Result = "FAIL, 請檢查通訊埠";
            //                }
            //                else
            //                {
            //                    r.Result = "FAIL";
            //                }
            //            }
            //            else
            //            {
            //                r.Result = s.ValueReadBack;
            //            }
            //        }
            //        else if (s.Name.Contains("STLINK"))
            //        {
            //            string msg;
            //            if (ImageFlash(r.Value, out msg))
            //            {
            //                r.Result = "PASS";
            //            }
            //            else
            //            {
            //                r.Result = "FAIL";
            //                result = false;
            //            }
            //            r.Response = msg;
            //        }
            //        else if (s.Name.Contains("CAN_"))
            //        {
            //            if(r.RW == "R")
            //            {
            //                CANReadMessage(s);
            //                r.Response = s.ValueReadBack;
            //                //if (!r.Valid) return true;
            //                r.Result = "UNKNOW";
            //                string[] low = r.LowThreshold.Split(',');
            //                string[] high = r.HighThreshold.Split(',');
            //                string[] vals = r.Response.TrimEnd(new char[] { '\r', '\n' }).Split(',');
            //                string msg = "";
            //                int l, h, v;
            //                int nofItem;
            //                nofItem = vals.Length;
            //                if ((low.Length == nofItem) && (high.Length == nofItem) && (vals.Length > 0))
            //                {
            //                    for (int i = 0; i < nofItem; i++)
            //                    {
            //                        bool fail = false;
            //                        fail = !int.TryParse(low[i], out l);
            //                        fail = !int.TryParse(high[i], out h);
            //                        fail = !int.TryParse(vals[i], out v);
            //                        if (fail)
            //                        {
            //                            r.Result = "Wrong Parameter format";
            //                        }
            //                        else
            //                        {
            //                            if ((v >= l) && (v <= h))
            //                            {
            //                                msg += "PASS";
            //                            }
            //                            else
            //                            {
            //                                msg += "FAIL";
            //                            }
            //                            if(i < nofItem - 1)
            //                            {
            //                                msg += ",";
            //                            }
            //                        }

            //                    }
            //                    r.Result = msg;
            //                }

            //            }
            //            else // Write
            //            {
            //                s.ValueToWrite = r.Value;
            //                CANWriteMessage(s);
            //            }
            //        }
            //        else if (s.Name.Contains("TIMER"))
            //        {
            //            int t;
            //            if(int.TryParse(r.Value, out t))
            //            {
            //                Thread.Sleep(t);
            //            }
            //        }
            //        else
            //        {
            //            if (r.RW == "R")
            //            {
            //                masterRTUReadRegister(s);
            //                r.Response =s.ValueReadBack;

            //                if (r.Valid)
            //                {
            //                    double low, high, v;
            //                    bool fail = false;
            //                    fail = !double.TryParse(r.LowThreshold, out low);
            //                    fail = !double.TryParse(r.HighThreshold, out high);
            //                    fail = !double.TryParse(r.Response, out v);

            //                    if (!fail)
            //                    {
            //                        if ((v >= low) && (v <= high))
            //                        {
            //                            r.Result = "PASS";
            //                        }
            //                        else
            //                        {
            //                            r.Result = "FAIL";
            //                        }

            //                    }
            //                    else
            //                    {
            //                        r.Result = "Wrong Condition";
            //                    }
            //                }
            //                else
            //                {
            //                    r.Result = "No Valid";
            //                }
            //            }
            //            else if (r.RW == "W")
            //            {
            //                s.ValueToWrite = r.Value;
            //                masterRTUWriteRegister(s);
            //                Thread.Sleep(r.Delay);
            //                masterRTUReadRegister(s);
            //                r.Response = s.ValueReadBack;
            //                // try to compare
            //                double low, high, v;
            //                bool fail = false;
            //                fail = !double.TryParse(r.LowThreshold, out low);
            //                fail = !double.TryParse(r.HighThreshold, out high);
            //                fail = !double.TryParse(r.Response, out v);

            //                if (!fail && r.Valid)
            //                {
            //                    if ((v >= low) && (v <= high))
            //                    {
            //                        r.Result = "PASS";
            //                    }
            //                    else
            //                    {
            //                        r.Result = "Fail";
            //                    }

            //                }
            //                else
            //                {
            //                    r.Result = "Not Valid";
            //                }


            //            }
            //            else if (r.RW == "C") // check
            //            {
            //                //if(_testBench.Instruments.ElementAt(instID).ValueReadBack != _testBench.Instruments.ElementAt(instID).ValueToWrite)
            //                //{
            //                //    const string Value = "Read check Fail";
            //                //    Console.WriteLine(Value);
            //                //}
            //                //else
            //                //{
            //                //    const string Value = "Read check OK";
            //                //    Console.WriteLine(Value);
            //                //}
            //            }
            //        }
            //        Thread.Sleep(r.Delay);
            //    }
            //    else
            //    {
            //       // throw error for no device found
            //    }
            //}
            //if (!InvokeRequired)
            //{
            //    dataGridView2.Refresh();
            //}
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
            dataGridView2.Refresh();
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
                loadRecipeDefinitions(CurrentRecipeFile);
                statusStrip1.Items[1].Text = "Config File:" + CurrentRecipeFile;
            }
        }

        private void btnSaveRecipe_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("警告!", "檔案已存在,覆寫檔案?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SaveRecipe(CurrentRecipeFile);
            }
        }

        private void UpdateGridItem()
        {
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                dataGridView3.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void lbRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = RecipeList.HYRecipe[lbRecipe.SelectedIndex].Recipes;
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = RecipeList.HYRecipe[lbRecipe.SelectedIndex].Recipes;
            CurrentRecipe = RecipeList.HYRecipe[lbRecipe.SelectedIndex];

            UpdateGridItem();

        }

        private void lbRecipe_DoubleClick(object sender, EventArgs e)
        {
            //Thread t1 = new Thread(recipe_worker);

            //t1.Start();
            //RecipeToRun.Clear();
            //RecipeStart = -1;
            //RecipeStop = -1;
    
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

        private void SaveRecord()
        {
            if (!string.IsNullOrEmpty(tbWorkingDir.Text))
            {
                string fileName = tbWorkingDir.Text + "\\" + tbDUTSerial.Text;
                fileName += DateTime.Now.ToString("_yyyyMMdd-HHmm");
                fileName += ".rcp";
                RecipeList.DeviceID = tbDUTSerial.Text;
                RecipeList.TimeStamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                if (File.Exists(fileName))
                {
                    if (MessageBox.Show("警告!", "檔案已存在,覆寫檔案?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        // todo: check if file presend
                        SaveRecipe(fileName);
                    }
                }
                else
                {
                    SaveRecipe(fileName);
                }

                MessageBox.Show("存檔完成");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
            //if (!string.IsNullOrEmpty(tbWorkingDir.Text))
            //{
            //    string fileName = tbWorkingDir.Text + "\\" + tbDUTSerial.Text;
            //    fileName += DateTime.Now.ToString("_yyyyMMdd-HHmm");
            //    fileName += ".rcp";
            //    RecipeList.DeviceID = tbDUTSerial.Text;
            //    RecipeList.TimeStamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            //    if (File.Exists(fileName))
            //    {
            //        if (MessageBox.Show("警告!", "檔案已存在,覆寫檔案?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //        {
            //            // todo: check if file presend
            //            SaveRecipe(fileName);
            //        }
            //    }
            //    else
            //    {
            //        SaveRecipe(fileName);
            //    }
            //}
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
//            STLinkAdapter stlink = new STLinkAdapter(SystemConfig.UtilityPath);
            Flash_Device stlink = new Flash_Device(SystemConfig.UtilityPath, "STLINK");
            stlink.SetValue("FLASH", "F407xx");
            msg = stlink.GetValue("FLASH");

            //STLinkReturnCodes ret;
            //string retstr;
            //ret = stlink.FindSTLink(out retstr);
            ////rtFlashMessage.Text = retstr;
            //if (ret == STLinkReturnCodes.Failure)
            //{
            //    MessageBox.Show("NO STLINK Found!");
            //    msg = retstr;
            //    return false;
            //}
            //string target = "F407xx";
            ////var res = await Task.Run(() => stlink.ConnectToTarget(target, out retstr));
            //ret = stlink.ConnectToTarget(target, out retstr);
            ////rtFlashMessage.Text = retstr;
            //if (ret == STLinkReturnCodes.Failure)
            //{
            //    MessageBox.Show("STLINK Error 01 !");
            //    msg = retstr;
            //    return false;
            //}



            //ret = stlink.ProgramTarget(fileName, 0x8000000, out retstr);
            ////rtFlashMessage.Text = retstr;
            //if (ret == STLinkReturnCodes.Failure)
            //{
            //    MessageBox.Show("STLINK Flash Error 02!");
            //    msg = retstr;
            //    return false;
            //}
            //else
            //{
            //    //rtFlashMessage.Text = "燒錄完成,請留意板上的綠色LED灯應該在閃爍!";
            //}
            //msg = retstr;
            return success;
        }
        private void btnImageFlash_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(tbImagePath.Text))
            //{
            //    return;
            //}
            //STLinkAdapter stlink = new STLinkAdapter("");
            //STLinkReturnCodes ret;
            //string retstr;
            //ret = stlink.FindSTLink(out retstr);
            //rtFlashMessage.Text = retstr;
            //if (ret == STLinkReturnCodes.Failure)
            //{
            //    MessageBox.Show("NO STLINK Found!");
            //    return;
            //}
            //string target = "STM32F10xx";
            ////var res = await Task.Run(() => stlink.ConnectToTarget(target, out retstr));
            //ret = stlink.ConnectToTarget(target, out retstr);
            //rtFlashMessage.Text = retstr;
            //if (ret == STLinkReturnCodes.Failure)
            //{
            //    MessageBox.Show("STLINK Error!");
            //    return;
            //}



            //ret = stlink.ProgramTarget(tbImagePath.Text, 0x8000000, out retstr);
            //rtFlashMessage.Text = retstr;
            //if (ret == STLinkReturnCodes.Failure)
            //{
            //    MessageBox.Show("STLINK Flash Error!");
            //    return;
            //}
            //else
            //{
            //    rtFlashMessage.Text = "燒錄完成,請留意板上的綠色LED灯應該在閃爍!";
            //}
        }

        private void btnWorkingDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                SystemConfig.DefaultWorkDir = folderBrowserDialog.SelectedPath;
                tbWorkingDir.Text = folderBrowserDialog.SelectedPath;
                SaveConfig(cfgFilePath);
            }
        }
        private void btnUtilPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FileName = "Select a Exe file",
                Filter = "Executable(*.exe)|*.exe",
                Title = "Open Executable file"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SystemConfig.UtilityPath = dialog.FileName;
                //tbUtilPath.Text = SystemConfig.UtilityPath;
                SaveConfig(cfgFilePath);
            }

        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach(DataGridViewRow r in dataGridView2.Rows)
            {
                r.HeaderCell.Value = r.Index + 1;
            }
        }

        private void UpdateCurrentRecipe(object sender, int index)
        {
            if (InvokeRequired)
            {
                Invoke(new RecipeExecute.ActiveResult(UpdateCurrentRecipe),sender,index);
            }
            else
            {
                dataGridView2.Rows[index].Selected = true;
            }
        }

        private void OnRecipeDone(object sender, int index)
        {
            if (InvokeRequired)
            {
                Invoke(new RecipeExecute.ActiveResult(OnRecipeDone), sender, index);
            }
            else
            {
                dataGridView1.Refresh();
                dataGridView2.Refresh();
                if(index == CurrentRecipe.Recipes.Count)
                {
                    bool valid = true;
                    for (int i = 0; i < CurrentRecipe.Recipes.Count; i++)
                    {
                        Recipedef r = CurrentRecipe.Recipes[i];
                        if (r.Result != null && r.Result.Contains("FAIL"))
                        {
                            string s = "Recipe:" + CurrentRecipe.Name + " ID=" + (i + 1).ToString();
                            lvResult.Items.Add(s);
                            valid = false;
                        }
                    }

                    if (!valid)
                    {
                        MessageBox.Show("Recipe Fail!", "Message");
                        RecipeToRun.Clear();
                    }
                    else if (RecipeToRun.Count > 0)
                    {
                        lbRecipe.SelectedIndex = RecipeToRun.First();
                        RecipeToRun.RemoveAt(0);
                        RunRecipe();
                    }
                    else
                    {
                        MessageBox.Show("Recipe Done!", "Message");
                        lvResult.Clear();
                    }
                    if (lvResult.Items.Count == 0)
                    {
                        lvResult.Items.Add("測試通過");
                    }
                }
                else if(index < 0)
                {
                    MessageBox.Show("Recipe Error!", "Error");
                }
                else
                {
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows[index].Selected = true;
                    dataGridView3.ClearSelection();
                    dataGridView3.Rows[index].Selected = true;

                    if(index > 5)
                    {
                        dataGridView2.FirstDisplayedScrollingRowIndex = index - 5;
                        dataGridView3.FirstDisplayedScrollingRowIndex = index - 5;
                    }
                    else
                    {
                        dataGridView2.FirstDisplayedScrollingRowIndex = 0;
                        dataGridView3.FirstDisplayedScrollingRowIndex = 0;
                    }
                    tbRecipeIndex.Text = (index + 1).ToString();
                }
            }
        }

        private bool RunRecipe()
        {
            //RecipeExecute re = new RecipeExecute();
            //re.OnIndexUpdate += UpdateCurrentRecipe;
            ////re.OnRecipeDone += OnRecipeDone;
            //re.SetInstrument(_testBench.Instruments);
            recipeExecutor.RunRecipe(CurrentRecipe);
            //            if (ThRecipe.IsAlive)
            //            {
            //                if (ThRecipe.ThreadState == ThreadState.Suspended)
            //                {
            //                    ThRecipe.Resume();
            //                }
            //                else
            //                {
            //                    ThRecipe.Suspend();
            //                }
            ////                    mres.Reset();
            //            }
            //            else
            //            {
            //                foreach(Recipedef r in CurrentRecipe.Recipes)
            //                {
            //                    r.Result = "";
            //                    r.Response = "";
            //                    r.Valid = false;

            //                }
            //                ThRecipe = new Thread(recipe_worker);
            //                ThRecipe.Start();

            //            }

            return true;
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            RecipeStart = -1;
            RecipeStop = -1;
        
            RunRecipe();

        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if(dataGridView2.SelectedRows.Count > 1)
            {
                RecipeStart = dataGridView2.SelectedRows[0].Index;
                RecipeStop = dataGridView2.SelectedRows[dataGridView2.SelectedRows.Count - 1].Index;
                if (RecipeStop > RecipeStart)
                {
                    RunRecipe();
                }
            }
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
            //List<Recipe> r = RecipeList.HYRecipe.ToList();
            //r.Insert(id, new Recipe() { Name = "New Recipe" });
            RecipeList.HYRecipe.Insert(id, new Recipe("New Recipe"));
            dataGridView1.DataSource = null;
            //RecipeList.HYRecipe = r.ToArray();
            dataGridView1.DataSource = RecipeList.HYRecipe;

            UpdateRecipeContents();
        }


        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            if(e.ColumnIndex == 10 && e.Value != null)
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

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(dataGridView2.SelectedRows.Count > 0)
            {
                dataGridView2.ContextMenuStrip = contextMenuStrip1;
            }
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (recipeExecutor.IsAlive())
            {
                if(MessageBox.Show("確定停止測試?","警告",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    recipeExecutor.Stop();
                    RecipeToRun.Clear();
                }
            }
        }

        private void btnTestStep4_Click_1(object sender, EventArgs e)
        {
            string input = "abcde";

            //Console.Write(input.StartWithUpper() ? "YES" : "NO");
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_loadInterface_Click(object sender, EventArgs e)
        {
            LoadInterfaceDefinition(interfaceFile);
        }

        private void btn_saveInterface_Click(object sender, EventArgs e)
        {
            SaveInterfaceDefinition(interfaceFile);
        }

        private void btn_addInterface_Click(object sender, EventArgs e)
        {
            switch (cb_interfaceType.SelectedIndex)
            {
                case 0:
                    _instruMentInterface.Add_Interface(new ModbusInterface("COM1,9600,N,8,1", ProtocolInterface.Protocol.MODBUS));
                    dgv_interface.DataSource = null;
                    dgv_interface.DataSource = _instruMentInterface.Interfaces;
                    break;
                case 1: // CAN
                    _instruMentInterface.Add_Interface(new USB_CAN_I7565("COM1,H2,250000,250000",ProtocolInterface.Protocol.CANBUS));
                    dgv_interface.DataSource = null;
                    dgv_interface.DataSource = _instruMentInterface.Interfaces;
                    break;
                case 2: // NATIVE
                    _instruMentInterface.Add_Interface(new NATIVEInterface("COM1,9600", ProtocolInterface.Protocol.NATIVE));
                    dgv_interface.DataSource = null;
                    dgv_interface.DataSource = _instruMentInterface.Interfaces;
                    break;
                default:break;
            }

        }

        private void btn_removeInterface_Click(object sender, EventArgs e)
        {
            int instID = dgv_interface.CurrentCell.RowIndex;
            if (instID < _instruMentInterface.Interfaces.Count)
            {
                dgv_interface.DataSource = null;
                _instruMentInterface.Interfaces.RemoveAt(instID);
                dgv_interface.DataSource = _testBench.Instruments;
                dgv_interface.Refresh();
            }
        }

        private void Add_ModbusDev()
        {
            MODBUS_Device dev = new MODBUS_Device("MODBUS_IF1", "PSU1", 1);
            dataGridView1.DataSource = null;
            _testBench.AddInstrument(dev);
            dataGridView1.DataSource = _testBench.Instruments;

        }
        private void Add_SCPIDev()
        {
            SCPI_DC_LOAD dev = new SCPI_DC_LOAD("COM", "LOAD1");
            dev.AddCommandDefine("IDN", "*IDN?");
            dev.AddCommandDefine("LOAD_A", ":SOUR:LIST:RANG");
            dev.AddCommandDefine("CTRL", ":SOUR:INP_STAT");

            dataGridView1.DataSource = null;
            _testBench.AddInstrument(dev);
            dataGridView1.DataSource = _testBench.Instruments;
        }

        private void Add_FlashDev()
        {
            Flash_Device dev = new Flash_Device(string.Empty, "STLINK");

            dataGridView1.DataSource = null;
            _testBench.AddInstrument(dev);
            dataGridView1.DataSource = _testBench.Instruments;
        }

        private void Add_CANDev()
        {
            CANBUS_Device dev = new CANBUS_Device("USBCAN","CAN");

            dataGridView1.DataSource = null;
            _testBench.AddInstrument(dev);
            dataGridView1.DataSource = _testBench.Instruments;
        }

        private void Add_TimerDev()
        {
            TIMER_Device dev = new TIMER_Device(string.Empty, "DELAY");

            dataGridView1.DataSource = null;
            _testBench.AddInstrument(dev);
            dataGridView1.DataSource = _testBench.Instruments;
        }

        private void Add_DUTComDev()
        {
            SerialCOM_Device dev = new SerialCOM_Device("COM", "DUT_COM");

            dataGridView1.DataSource = null;
            _testBench.AddInstrument(dev);
            dataGridView1.DataSource = _testBench.Instruments;
        }

        private void btn_addModbusDev_Click(object sender, EventArgs e)
        {
            Add_ModbusDev();
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int instID = dataGridView1.CurrentCell.RowIndex;
            if (instID < _testBench.Instruments.Count)
            {
                _testBench.SetCurrent(instID);

                switch (_testBench.CurrentInstrument().InstrumentType)
                {
                    case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_MODBUS:
                        {
                            dgv_instrument_2.DataSource = null;
                            MODBUS_Device mins = (MODBUS_Device)_testBench.CurrentInstrument();
                            dgv_instrument_2.DataSource = mins.Registers;
                            dgv_instrument_2.Refresh();
                            gp_modbusRegs.Visible = true;
                        }
                        break;
                    case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_SCPI:
                        {
                            dgv_instrument_2.DataSource = null;
                            SCPI_DC_LOAD mins = (SCPI_DC_LOAD)_testBench.CurrentInstrument();
                            dgv_instrument_2.DataSource = mins.Commands;
                            dgv_instrument_2.Refresh();
                            gp_modbusRegs.Visible = true;
                        }
                        break;
                    case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_TRANSPRAENT:
                        {
                            dgv_instrument_2.DataSource = null;
                            SerialCOM_Device mins = (SerialCOM_Device)_testBench.CurrentInstrument();
                            dgv_instrument_2.DataSource = mins.Commands;
                            dgv_instrument_2.Refresh();
                            gp_modbusRegs.Visible = true;
                        }
                        break;
                    case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_CANBUS:
                        {
                            dgv_instrument_2.DataSource = null;
                            CANBUS_Device mins = (CANBUS_Device)_testBench.CurrentInstrument();
                            dgv_instrument_2.DataSource = mins.Commands;
                            dgv_instrument_2.Refresh();
                            gp_modbusRegs.Visible = true;
                        }
                        break;
                    default:
                        gp_modbusRegs.Visible = false;
                        break;
                }
            }

        }

        private void btn_addMBReg_Click(object sender, EventArgs e)
        {
            dgv_instrument_2.DataSource = null;
            switch (_testBench.CurrentInstrument().InstrumentType)
            {
                case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_MODBUS:
                    MODBUS_Device mdev = (MODBUS_Device)_testBench.CurrentInstrument();
                    mdev.AddRegisterDefines("REG", 0, MODBUS_REG_Struct._REG_TYPE.REG_S16, 1);
                    dgv_instrument_2.DataSource = mdev.Registers;
                    break;
                case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_SCPI:
                    SCPI_DC_LOAD sdev = (SCPI_DC_LOAD)_testBench.CurrentInstrument();
                    sdev.AddCommandDefine("Command", "Argument");
                    dgv_instrument_2.DataSource = sdev.Commands;
                    break;
                case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_TRANSPRAENT:
                    SerialCOM_Device tdev = (SerialCOM_Device)_testBench.CurrentInstrument();
                    tdev.AddCommandDefine("Command", "Argument");
                    dgv_instrument_2.DataSource = tdev.Commands;
                    break;
                case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_CANBUS:
                    CANBUS_Device cdev = (CANBUS_Device)_testBench.CurrentInstrument();
                    cdev.AddCommandDefine("Command", "1122334455667788");
                    dgv_instrument_2.DataSource = cdev.Commands;
                    break;
                default: break;
            }
        }

        private void btn_removeMBReg_Click(object sender, EventArgs e)
        {
            int instID = dgv_instrument_2.CurrentCell.RowIndex;

            switch (_testBench.CurrentInstrument().InstrumentType)
            {
                case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_MODBUS:
                    MODBUS_Device mdev = (MODBUS_Device)_testBench.CurrentInstrument();
                    if (instID < mdev.Registers.Count)
                    {
                        dgv_instrument_2.DataSource = null;
                        mdev.Registers.RemoveAt(instID);
                        dgv_instrument_2.DataSource = mdev.Registers;
                    }
                    break;
                case Abstract_Instrument._INSTRUMENT_TYPE.TYPE_SCPI:
                    SCPI_DC_LOAD sdev = (SCPI_DC_LOAD)_testBench.CurrentInstrument();
                    if (instID < sdev.Commands.Count)
                    {
                        dgv_instrument_2.DataSource = null;
                        sdev.Commands.RemoveAt(instID);
                        dgv_instrument_2.DataSource = sdev.Commands;
                    }
                    break;
                default: break;
            }
        }

        private void btn_addScpiLoad_Click(object sender, EventArgs e)
        {
            Add_SCPIDev();
        }

        private void btn_addCAN_Click(object sender, EventArgs e)
        {
            Add_CANDev();
        }

        private void btn_addSTLINK_Click(object sender, EventArgs e)
        {
            Add_FlashDev();
        }

        private void btn_addTimer_Click(object sender, EventArgs e)
        {
            Add_TimerDev();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = lbRecipe.SelectedIndex;
            RecipeList.HYRecipe.RemoveAt(id);
            dataGridView1.DataSource = null;
            //RecipeList.HYRecipe = r.ToArray();
            dataGridView1.DataSource = RecipeList.HYRecipe;

            UpdateRecipeContents();
        }

        private void btn_addInstrument_Click(object sender, EventArgs e)
        {
            switch (cb_instruments.SelectedIndex)
            {
                case 0: Add_ModbusDev();break;
                case 1: Add_SCPIDev();break;
                case 2: Add_CANDev();break;
                case 3: Add_FlashDev();break;
                case 4: Add_TimerDev();break;
                case 5: Add_DUTComDev();break;
                default:break;
            }
        }

        private void tbDUTSerial_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SaveRecord();
            }
        }

        private void tbDUTSerial_FontChanged(object sender, EventArgs e)
        {

        }

        private void tbDUTSerial_Enter(object sender, EventArgs e)
        {

        }

        private void tbDUTSerial_MouseUp(object sender, MouseEventArgs e)
        {
            TextBox t = (TextBox)sender;
            t.SelectAll();
        }

        private void cellCopy_Click(object sender, EventArgs e)
        {
            
            DataObject dobj = dataGridView2.GetClipboardContent();
            if(dobj != null)
            {
                Clipboard.SetDataObject(dobj);
            }
        }

        private void cellPaste_Click(object sender, EventArgs e)
        {
            
            

        }

    }
}
