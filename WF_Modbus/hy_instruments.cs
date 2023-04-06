using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using NModbus;
using NModbus.Serial;
using NModbus.Utility;
using NModbus.Extensions.Functions;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Threading;

using System.Diagnostics;

namespace HY_INSTRUMENTS
{
    
    public enum _vType
    {
        //[EnumMember(ValueType = "test")]
        USHORT,
        SHORT,
        UINT,
        INT,
        FLOAT
    };

    public abstract class ProtocolInterface
    {
        public enum Protocol
        {
            NATIVE,
            MODBUS,
            CANBUS,
        };
        [DisplayName("Device Name")]
        public string Name { get; set; }
        public Protocol protocol { get; set; }
        [DisplayName("Connection String")]
        public string ConnectionString { get; set; }


        public abstract void Write(params object[] objtcts);
        public abstract void Read(params object[] objtcts);
        public abstract void InitConfig();

        public ProtocolInterface() { }
        public ProtocolInterface(string connectionString, Protocol protocol)
        {
            this.protocol = protocol;
            this.ConnectionString = connectionString;
            switch (this.protocol)
            {
                case Protocol.NATIVE:
                    this.Name = "NATIVE";
                    break;
                case Protocol.MODBUS:
                    this.Name = "MODBUS_IF";
                    break;
                case Protocol.CANBUS:
                    this.Name = "CANBUS_IF";
                    break;
                default:break;
            }
        }
    }
    public class ModbusInterface: ProtocolInterface
    {
        private string PortName { get; set; }
        private int Baudrate { get; set; }

        public ModbusInterface() : base() { }
        public ModbusInterface(string connectionString, Protocol protocol) 
            : base(connectionString, Protocol.MODBUS)
        {
            // the connection should baud,parity,databits, stop bits, flow control
        }

        public override void InitConfig()
        {
            string[] sl = ConnectionString.Split(',');
            PortName = sl[0];
            Baudrate = int.Parse(sl[1]);
        }

        public void SetInterfaceName(string name)
        {
            Name = name;
        }

        public string InterfaceName()
        {
            return Name;
        }

        public override void Write(params object[] objtcts)
        {

        }

        public bool Write(byte slave_addr, ushort address, ushort[] packet)
        {
            bool success = false;
            string[] sl = ConnectionString.Split(',');

            using (SerialPort port = new SerialPort(PortName, Baudrate))
            {
                port.Open();
                port.WriteTimeout = 200;
                port.ReadTimeout = 200;
                
                if (port.IsOpen)
                {
                    Thread.Sleep(50);
                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateRtuMaster(port);
                    try
                    {
                        master.WriteMultipleRegisters(slave_addr, address, packet);
                        
                    }
                    catch { }
                    port.Close();
                }
            }
            return success;
        }

        public override void Read(params object[] objtcts)
        {

        }

        public bool Read(byte slave_addr, ushort address, ushort n, out ushort[] packet)
        {
            bool success = false;
            packet = null;
            using (SerialPort port = new SerialPort(PortName, Baudrate))
            {
                port.Open();
                port.WriteTimeout = 200;
                port.ReadTimeout = 200;
                if (port.IsOpen)
                {
                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateRtuMaster(port);
                    ushort[] rb = null;
                    try
                    {
                        rb = master.ReadHoldingRegisters(slave_addr, address, n);
                        packet = rb;
                        success = true;
                    }
                    catch { 
                       
                    }
                    port.Close();
                }
            }
            return success;
        }
    }

    public class NATIVEInterface : ProtocolInterface
    {
        private string PortName { get; set; }
        private int Baudrate { get; set; }

        public string Response { get; set; }

        public NATIVEInterface() : base() { }
        public NATIVEInterface(string connectionString, Protocol protocol)
            : base(connectionString, Protocol.NATIVE)
        {
            // the connection should baud,parity,databits, stop bits, flow control
            Response = string.Empty;
        }

        public override void InitConfig()
        {
            string[] sl = ConnectionString.Split(',');
            PortName = sl[0];
            Baudrate = int.Parse(sl[1]);
        }

        public void SetInterfaceName(string name)
        {
            Name = name;
        }

        public string InterfaceName()
        {
            return Name;
        }

        public override void Write(params object[] objects)
        {
            if (objects.Length <1 ) return;
            string cmd = objects[0].ToString();
            Response = string.Empty;
            using (SerialPort port = new SerialPort(PortName, Baudrate))
            {
                port.Open();
                if (port.IsOpen)
                {
                    port.ReadTimeout = 1000;
                    port.Write(cmd);
                    //                    Thread.Sleep(100);
                    try
                    {
                        Response = port.ReadLine();
                    }
                    catch
                    {
                       
                    }

                    port.Close();
                }
            }


        }


        public override void Read(params object[] objtcts)
        {

        }

        public bool Read(out string packet)
        {
            packet = Response;
            return (Response == string.Empty);
        }
    }

    public class CANBUS_Packet
    {
        public string PacketName;
        public int DeviceID;
        public byte portID;
        public uint extID;
        public byte RTR;
        public byte DLC;
        public byte[] Data;

        public CANBUS_Packet()
        {
            PacketName = "Packet";
            Data = new byte[8];
        }
        public CANBUS_Packet(string name)
        {
            PacketName = name;
            Data = new byte[8];
        }
    }
    /* 
     *  connectionString: COMx,H1/H2, bitrate, bitrate
     */
    public class USB_CAN_I7565 : ProtocolInterface
    {
        private string PortName { get; set; }
        private int Baudrate { get; set; }

        private byte PortID { get; set; }
        private string Model { get; set; }
        private uint[] Bitrate { get; set; }
        private bool Connected { get; set; }

        public int LastError { get; set; }
        public string LastErrorString { get; set; }
        public List<CANBUS_Packet> PacketsRevd {get;set;}

        public USB_CAN_I7565() : base() 
        {
            PacketsRevd = new List<CANBUS_Packet>(); 
        }
        public USB_CAN_I7565(string connectionString, Protocol protocol)
            : base(connectionString, Protocol.CANBUS)
        {
            // the connection should baud,parity,databits, stop bits, flow control
            PacketsRevd = new List<CANBUS_Packet>();
        }

        public override void InitConfig()
        {
            string[] sl = ConnectionString.Split(',');
            Bitrate = new uint[] { 250000, 250000 };
            PortName = sl[0];
            int id;
            int.TryParse(PortName.Substring(3, PortName.Length - 3),out id);
            PortID = (byte)id;
            //Baudrate = int.Parse(sl[1]);
            Model = sl[1];
            Bitrate[0] = uint.Parse(sl[2]);
            if(sl.Length == 4)
            {
                Bitrate[1] = uint.Parse(sl[3]);
            }

            Connected = false;
            // open device
        }

        private bool OpenDevice()
        {
            bool success = false;

            byte[] Mod_CfgData = new byte[512];
            Mod_CfgData[0] = 0;
            Mod_CfgData[1] = 0;

            int result;
            result = VCI_CAN_DotNET.VCI_SDK.VCI_OpenCAN_NoStruct(PortID, 0x2, Bitrate[0], Bitrate[1]);

            if (result != 0)
            {
                LastError = -1;
                LastErrorString = "Device Open Error!";
                success = false;
            }
            else
            {
                success = true;
            }

            Connected = true;

            return success;
        }

        private void CloseDevice()
        {
            VCI_CAN_DotNET.VCI_SDK.VCI_CloseCAN(PortID);
            Connected = false;
        }

        public void SetInterfaceName(string name)
        {
            Name = name;
        }

        public bool SendPacket(CANBUS_Packet packet)
        {
            if (!OpenDevice()) return false;

            int ret = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(packet.portID, 1, packet.RTR, packet.DLC, packet.extID, packet.Data);

            if (ret == 0)
            {
                Thread.Sleep(200);
                RecvAllPacket();
                CloseDevice();
                return true;
            }
            else
            {
                CloseDevice();
                return false;
            }

        }

        public void RecvAllPacket(int can_no = -1)
        {
            PacketsRevd.Clear();
            bool result = false;
            int Ret;
            byte CAN_No, Mode, RTR, DLC;
            byte[] Data = new byte[8];
            UInt32 CANID, TH, TL, DL, DH;

            Mode = RTR = DLC = 0;
            CANID = TH = TL = 0;
            uint nofMsg = 0;
            if((can_no == 1) || (can_no == -1))
            {
                if (VCI_CAN_DotNET.VCI_SDK.VCI_Get_RxMsgCnt(1, ref nofMsg) == 0)
                {
                    for (int i = 0; i < nofMsg; i++)
                    {
                        if (VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(1, ref Mode, ref RTR, ref DLC, ref CANID, ref TL, ref TH, Data) == 0)
                        {
                            CANBUS_Packet p = new CANBUS_Packet();
                            p.Data = Data;
                            p.extID = CANID;
                            p.RTR = RTR;
                            p.DLC = DLC;
                            PacketsRevd.Add(p);
                        }
                    }
                }
            }
            if ((can_no == 2) || (can_no == -1))
            {
                if (VCI_CAN_DotNET.VCI_SDK.VCI_Get_RxMsgCnt(2, ref nofMsg) == 0)
                {
                    for (int i = 0; i < nofMsg; i++)
                    {
                        if (VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(2, ref Mode, ref RTR, ref DLC, ref CANID, ref TL, ref TH, Data) == 0)
                        {
                            CANBUS_Packet p = new CANBUS_Packet();
                            p.Data = Data;
                            p.extID = CANID;
                            p.RTR = RTR;
                            p.DLC = DLC;
                            PacketsRevd.Add(p);
                        }
                    }
                }
            }
        }

        public string InterfaceName()
        {
            return Name;
        }

        public override void Write(params object[] objects)
        {
            if (objects.Length < 1) return;
            string cmd = objects[0].ToString();
            using (SerialPort port = new SerialPort(PortName, Baudrate))
            {
                port.Open();
                if (port.IsOpen)
                {
                    port.Write(cmd);
                    Thread.Sleep(100);

                    port.Close();
                }
            }


        }


        public override void Read(params object[] objects)
        {

        }

        public bool Read(out CANBUS_Packet packet)
        {
            packet = new CANBUS_Packet();
            //return (Response == string.Empty);
            return true;
        }
    }

    public abstract class Abstract_Instrument
    {
        public enum _INSTRUMENT_TYPE
        {
            TYPE_MODBUS,
            TYPE_CANBUS,
            TYPE_SCPI,
            TYPE_TRANSPRAENT,
            TYPE_FLASH,
            TYPE_TIMER,
        };
        public delegate void commandStream(object sender, ushort[] cmd);
        [DisplayName("Name")]
        public string InstrumentName { get; set; }
        [DisplayName("Interface")]
        public string InterfaceName { get; set; }
        [DisplayName("Number of Channel")]
        public int NumberOfChannels { get; set; }
        public _INSTRUMENT_TYPE InstrumentType { get; set; }

        protected ProtocolInterface _interface;
        protected EventWaitHandle waitHandle;
        protected Thread workingThread;
        protected bool _continueThread;
        protected bool _setValid;

        public Abstract_Instrument() { }
        public Abstract_Instrument(string interface_name, string name)
        {
            //this._interface = interface_name;
            this.InterfaceName = interface_name;
            this.InstrumentName = name;
            InitialConfig();
        }

        public void InitialConfig()
        {
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _continueThread = false;
            _setValid = false;
        }

        public abstract void write();
        public abstract void read();
        public abstract void FeedData<T>(T Data);

        public abstract void SetValue(string function, string value);
        public abstract string GetValue(string function);
        public abstract bool SetValid();
        public abstract void StartProc();
        public abstract void StopProc();
        public abstract void SetInterface(ProtocolInterface _interface);
        public abstract void SetThreshold(string func, string low, string high);
        public abstract bool ValidRange(string func);


    }

    public class Entry
    {
        [DisplayName("Command")]
        public string Key { get; set; }
        [DisplayName("Command String")]
        public string Value { get; set; }
        [XmlIgnore]
        public string Response { get; set; }
        private string _HighThreshold { get; set; }
        private string _LowThreshlod { get; set; }
        public Entry() { }
        public Entry(string key, string value)
        {
            Key = key;
            Value = value;
            _LowThreshlod = string.Empty;
            _HighThreshold = string.Empty;
        }

        public void SetThreahold(string low, string high)
        {
            _LowThreshlod = low;
            _HighThreshold = high;
        }

        public string LowThreshold()
        {
            return _LowThreshlod;
        }

        public string HighThreshold()
        {
            return _HighThreshold;
        }

        public bool InRange()
        {
            double lo = double.Parse(_LowThreshlod);
            double hi = double.Parse(_HighThreshold);
            double v;

            if(double.TryParse(Response,out v))
            {
                if ((v >= lo) && (v <= hi))
                {
                    return true;
                }
            }

            return false;
        }
        public bool InMatch(string input)
        {
            if(input.Trim() == Response.Trim())
            {
                return true;
            }
            return false;
        }
        public bool LowMatch()
        {
            if (_LowThreshlod.Trim() == Response.Trim())
            {
                return true;
            }
            return false;
        }
        public bool ArrayMatch()
        {
            bool match = true;
            double[] val = Array.ConvertAll(Response.Trim().Split(' '),Double.Parse);
            double[] low  = Array.ConvertAll(_LowThreshlod.Split(' ') ,Double.Parse);
            double[] high = Array.ConvertAll(_HighThreshold.Split(' '),Double.Parse);

            if(val.Length == low.Length && val.Length == high.Length)
            {
                for(int i = 0; i < val.Length; i++)
                {
                    double v = val[i];
                    double l = low[i];
                    double h = high[i];
                    if (v < l) match = false;
                    if (v > h) match = false;
                }
            }
            else
            {
                match = false;
            }
            return match;
        }
    }

    /* console comm instrument
     * usually is the DUT running console commands
     */
    public class SCPI_DC_LOAD: Abstract_Instrument
    {
        const int NOF_CHANNEL = 1;
        public delegate void CommandIO(string command);

        public event CommandIO OnCommandWrite;
        public event CommandIO OnValidResult;
        public string Instruction { get; set; }
        public string WriteCommand { get; set; }
        public string ValidResult { get; set; }
//        public Dictionary<string,string> Commands { get; set; }
        public List<Entry> Commands { get; set; }
        private NATIVEInterface _myInterface { get; set; }
        
        public SCPI_DC_LOAD() { }
        public SCPI_DC_LOAD(string interface_name, string name)
            :base(interface_name,name)
        {
            Commands = new List<Entry>();
            InstrumentType = _INSTRUMENT_TYPE.TYPE_SCPI;
        }

        public void AddCommandDefine(string command, string arg)
        {
            if (command == string.Empty) return;
            if (arg == string.Empty) return;

            Commands.Add(new Entry(command,arg));
        }
        public override void FeedData<T>(T Data)
        {
            throw new NotImplementedException();
        }
        public override void read()
        {
            throw new NotImplementedException();
        }
        public override void write()
        {
            throw new NotImplementedException();
        }

        private Entry findEntry(string name)
        {
            foreach(Entry e in Commands)
            {
                if(e.Key == name)
                {
                    return e;
                }
            }

            return null;
        }

        public override void SetThreshold(string func , string low, string high )
        {
            foreach(Entry e in Commands)
            {
                if(e.Key == func)
                {
                    e.SetThreahold(low, high);
                }
            }
        }

        public override bool ValidRange(string func)
        {
            Entry e = findEntry(func);
            if(e != null)
            {
                //if(e.Key == "VOLT" || e.Key == "CURR")
                //{

                //}
                
                return e.InRange();
            }
            return true;
        }

        public override void SetValue(string function, string value)
        {
            string key;
            foreach (var v in Commands)
            {
                if(v.Key == function)
                {
                    string cmd = string.Format("{0} {1}\r\n", v.Value, value);
                    _myInterface.Write(cmd);
                    //Thread.Sleep(100);
                    //string ret;
                    //_myInterface.Read(out ret);
                    //v.Response = ret;
                }
            }
        }

        public override string GetValue(string function)
        {
            string value = "";
            Entry e = findEntry(function);
            if(e != null && e.Value.Contains("?"))
            {
                
                string cmd = string.Format("{0}\r\n", e.Value);
                _myInterface.Write(cmd);
                Thread.Sleep(100);
                string ret;
                //_myInterface.Read(out ret);
                e.Response = _myInterface.Response;
                value = e.Response;
            }
            return value;
        }

        public override bool SetValid()
        {
            return true;
        }

        public override void StartProc()
        {
            
        }

        public override void StopProc()
        {
            
        }

        public override void SetInterface(ProtocolInterface _interface)
        {
            _myInterface = (NATIVEInterface)_interface;
        }

        public void SetCurrentLoad(int channel, float load)
        {

        }
        public void SetVoltage(int channel, float voltage)
        {

        }

        public void SetPower(int channel, float power)
        {

        }


        public string PacketToWrite()
        {
            return WriteCommand;
        }

        public bool SetResult(string res)
        {

            return (res == ValidResult);
        }
    }

    /* console comm instrument
 * usually is the DUT running console commands
 */
    public class SerialCOM_Device : Abstract_Instrument
    {
        const int NOF_CHANNEL = 1;
        public delegate void CommandIO(string command);

        public event CommandIO OnCommandWrite;
        public event CommandIO OnValidResult;
        public string Instruction { get; set; }
        public string WriteCommand { get; set; }
        public string ValidResult { get; set; }
        private NATIVEInterface _myInterface { get; set; }
        private string LastWrite { get; set; }
        private string Response { get; set; }
        public List<Entry> Commands { get; set; }

        public SerialCOM_Device() { }
        public SerialCOM_Device(string interface_name, string name)
            : base(interface_name, name)
        {
            InstrumentType = _INSTRUMENT_TYPE.TYPE_TRANSPRAENT;
        }
        public void AddCommandDefine(string command, string arg)
        {
            if (command == string.Empty) return;
            if (arg == string.Empty) return;

            Entry e = new Entry(command, arg);
            Commands.Add(e);
        }

        public override void SetThreshold(string func, string low, string high)
        {
            foreach(Entry e in Commands)
            {
                if(e.Key == func)
                {
                    e.SetThreahold(low, high);
                }
            }
        }

        public override bool ValidRange(string func)
        {
            Entry e = findEntry(func);
            if(e != null)
            {
                if(e.Key == "TEST")
                {
                    return e.InMatch(e.Value);
                }
                else if(e.Key == "READ_DI")
                {
                    return e.LowMatch();
                }
                else if(e.Key == "READ_AI")
                {
                    return e.ArrayMatch();
                }
                return e.InRange();
            }
            return false;
        }

        public override void FeedData<T>(T Data)
        {
            throw new NotImplementedException();
        }
        public override void read()
        {
            throw new NotImplementedException();
        }
        public override void write()
        {
            throw new NotImplementedException();
        }

        private Entry findEntry(string name)
        {
            foreach(Entry e in Commands)
            {
                if (e.Key == name)
                    return e;
            }
            return null;
        }

        public override void SetValue(string function, string value)
        {
            //DUT CoM is echo server
            Entry e = findEntry(function);
            if(e != null)
            {
                string cmd = string.Format("{0} {1}\r\n", e.Value, value);
                _myInterface.Write(cmd);
                e.Response = _myInterface.Response;
            }
        }

        public override string GetValue(string function)
        {
            Entry e = findEntry(function);
            if(e != null)
            {
                if(e.Key == "TEST")
                {
                    string cmd = string.Format("{0}\r\n", e.Value);
                    _myInterface.Write(cmd);
                    e.Response = _myInterface.Response;
                }
                else if(e.Key == "READ_DI")
                {
                    string cmd = string.Format("{0}\r\n", e.Value);
                    _myInterface.Write(cmd);
                    e.Response = _myInterface.Response;
                }
                else if (e.Key == "READ_AI")
                {
                    string cmd = string.Format("{0}\r\n", e.Value);
                    _myInterface.Write(cmd);
                    e.Response = _myInterface.Response;
                }

                return e.Response;
            }
            return string.Empty;
        }

        public override bool SetValid()
        {
            return (Response == LastWrite);
        }

        public override void StartProc()
        {

        }

        public override void StopProc()
        {

        }

        public override void SetInterface(ProtocolInterface _interface)
        {
            _myInterface = (NATIVEInterface)_interface;
        }


        public string PacketToWrite()
        {
            return WriteCommand;
        }

        public bool SetResult(string res)
        {

            return (res == ValidResult);
        }
    }

    public class CANBUS_Device : Abstract_Instrument
    {
        const int NOF_CHANNEL = 1;
        public delegate void CommandIO(string command);

        public event CommandIO OnCommandWrite;
        public event CommandIO OnValidResult;
        public string Instruction { get; set; }
        public string WriteCommand { get; set; }
        public string ValidResult { get; set; }
        //        public Dictionary<string,string> Commands { get; set; }
        //public List<CANBUS_Packet> Packets { get; set; }
        public List<Entry> Commands { get; set; }
        private USB_CAN_I7565 _myInterface { get; set; }

        public CANBUS_Device() { }
        public CANBUS_Device(string interface_name, string name)
            : base(interface_name, name)
        {
           // Packets = new List<CANBUS_Packet>();
            Commands = new List<Entry>();
            InstrumentType = _INSTRUMENT_TYPE.TYPE_CANBUS;
        }

        public void AddCommandDefine(string command, string arg)
        {
            if (command == string.Empty) return;
            if (arg == string.Empty) return;
            Commands.Add(new Entry(command, arg));

        }
        public override void FeedData<T>(T Data)
        {
            throw new NotImplementedException();
        }
        public override void read()
        {
            throw new NotImplementedException();
        }
        public override void write()
        {
            throw new NotImplementedException();
        }

        public override void SetThreshold(string func, string low, string high)
        {
            
        }

        public override bool ValidRange(string func)
        {
            Entry e = findEntry(func);
            if(e != null)
            {
                if(e.Key == "TEST")
                {
                    return e.InMatch(e.Value);
                }
            }
            return false;
        }
        private Entry findEntry(string name)
        {
            foreach (Entry e in Commands)
            {
                if (e.Key == name)
                    return e;
            }
            return null;
        }
        public override void SetValue(string function, string value)
        {
            string key;
            foreach (var v in Commands)
            {
                if (v.Key == function)
                {
                    CANBUS_Packet p = new CANBUS_Packet();
                    p.Data = StringToByteArray(v.Value);
                    p.DeviceID = 0x01;
                    p.DLC = (byte)p.Data.Length;
                    p.extID = 0x112233;
                    p.RTR = 1;

                    _myInterface.SendPacket(p);
                    //Thread.Sleep(100);
                    //List<CANBUS_Packet> ret = new List<CANBUS_Packet>();
                    //_myInterface.RecvAllPacket(out ret);
                }
            }
        }

        public override string GetValue(string function)
        {
            string value = "";
            Entry e = findEntry(function);
            if(e != null)
            {
                if(e.Key == "TEST")
                {
                    CANBUS_Packet p = new CANBUS_Packet();
                    p.Data = StringToByteArray(e.Value);
                    p.DeviceID = 0x01;
                    p.DLC = (byte)p.Data.Length;
                    p.extID = 0x112233;
                    p.RTR = 0;

                    _myInterface.SendPacket(p);
                    Thread.Sleep(100);
                    List<CANBUS_Packet> ret = _myInterface.PacketsRevd;
                    if(ret != null && ret.Count > 0)
                    {
                        CANBUS_Packet p1 = ret.Last();
                        e.Response = BitConverter.ToString(p1.Data).Replace("-", "");
                    }
                    else
                    {
                        e.Response = string.Empty;
                    }
                }
            }
            value = e.Response;

            return value;
        }

        public override bool SetValid()
        {
            return true;
        }

        public override void StartProc()
        {
            foreach(var v in Commands)
            {
                CANBUS_Packet p = new CANBUS_Packet();
                p.Data = StringToByteArray(v.Value);
                p.DeviceID = 0x01;
                p.DLC = (byte)p.Data.Length;
                p.extID = 0x112233;
                p.RTR = 1;

                //Packets.Add(p);
            }

        }

        public override void StopProc()
        {

        }

        public override void SetInterface(ProtocolInterface _interface)
        {
            _myInterface = (USB_CAN_I7565)_interface;
        }

        public void SetCurrentLoad(int channel, float load)
        {

        }
        public void SetVoltage(int channel, float voltage)
        {

        }

        public void SetPower(int channel, float power)
        {

        }


        public string PacketToWrite()
        {
            return WriteCommand;
        }

        public bool SetResult(string res)
        {

            return (res == ValidResult);
        }

        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }


    /*
     *  MODBUS based instrument
     */
    public class MODBUS_REG_Struct
    {
        public enum _REG_TYPE
        {
            REG_S16,
            REG_U16,
            REG_S32,
            REG_U32,
            REG_F32,
        };
        public string RegName { get; set; }
        public ushort RegAddr { get; set; }
        public _REG_TYPE RegType { get; set; } // 16/32-bit INT/UNIT, or float
        public float Scale { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public long write { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public long readback { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public string write_string { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public string string_read { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public bool Modified { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public bool Valid { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public double _lowThreshold  { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public double _highThreshold { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public double _readScaled { get; set; }

        // todo: add conversion function here
        public MODBUS_REG_Struct()
        {
            Valid = false;
            Modified = false;
        }

        public bool InRange()
        {
            if ((_readScaled >= _lowThreshold) && (_readScaled <= _highThreshold))
                return true;
            return false;
        }

        public ushort[] PacketArray()
        {
            ushort[] packet = null;

            switch (RegType)
            {
                case _REG_TYPE.REG_S16:
                case _REG_TYPE.REG_U16:
                    //                    packet = new ushort[] { ushort.Parse(write_string) };
                    packet = new ushort[1];
                    packet[0] = (ushort)write;
                    break;
                case _REG_TYPE.REG_S32:
//                    packet = new ushort[] {ushort.Parse(write_string)};
                    packet = new ushort[2];
                    packet[0] = (ushort)write;
                    packet[1] = (ushort)(write >> 16);
                    break;
                case _REG_TYPE.REG_U32:
                    packet = new ushort[] { ushort.Parse(write_string) };
                    packet = new ushort[2];
                    packet[0] = (ushort)write;
                    packet[1] = (ushort)(write >> 16);
                    break;
                case _REG_TYPE.REG_F32:
                    packet = Float2Ushorts(Single.Parse(write_string));
                    break;
            }

            return packet.ToArray();
        }

        public void SetData(ushort[] data)
        {
            switch (RegType)
            {
                case _REG_TYPE.REG_S16:
                case _REG_TYPE.REG_U16:
                    readback = (long)data[0];
                    _readScaled = readback / Scale;
                    string_read = _readScaled.ToString();
                    break;
                case _REG_TYPE.REG_S32:
                    readback = (Int32)data[1] << 16 | (Int32)data[0];
                    _readScaled = readback / Scale;
                    string_read = _readScaled.ToString();
                    break;
                case _REG_TYPE.REG_U32:
                    readback = (Int32)data[1] << 16 | (Int32)data[0];
                    _readScaled = readback / Scale;
                    string_read = _readScaled.ToString();
                    break;
                case _REG_TYPE.REG_F32:
                    readback = (Int32)data[1] << 16 | (Int32)data[0];
                    _readScaled = readback / Scale;
                    string_read = _readScaled.ToString();
                    break;
            }
        }

        public ushort NofRegister()
        {
            switch (RegType)
            {
                case _REG_TYPE.REG_S16:
                case _REG_TYPE.REG_U16:
                    return 1;
                case _REG_TYPE.REG_S32:
                    return 2;
                case _REG_TYPE.REG_U32:
                    return 2;
                case _REG_TYPE.REG_F32:
                    return 2;
                default: return 0;
            }
        }

        public bool IsRWMatch()
        {
            bool match = (write == readback);
            return match;
        }

        public byte[] Ushort2Bytes(ushort[] src, bool reverse = false)
        {
            int count = src.Length;
            byte[] dest = new byte[count << 1];
            if (reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2] = (byte)(src[i] >> 8);
                    dest[i * 2 + 1] = (byte)(src[i] >> 0);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2 + 1] = (byte)(src[i] >> 8);
                    dest[i * 2] = (byte)(src[i] >> 0);
                }
            }
            return dest;

        }
        public ushort[] Byte2UShorts(byte[] b, bool reverse = false)
        {
            int len = b.Length;
            byte[] src = new byte[len + 1];
            b.CopyTo(src, 0);
            int cnt = len >> 1;
            if (len % 2 != 0)
            {
                cnt += 1;
            }

            ushort[] ret = new ushort[cnt];
            if (reverse)
            {
                for (int i = 0; i < cnt; i++)
                {
                    ret[i] = (ushort)(src[i * 2] << 8 | src[i * 2 + 1] & 0xff);
                }
            }
            else
            {
                for (int i = 0; i < cnt; i++)
                {
                    ret[i] = (ushort)(src[i * 2 + 1] << 8 | src[i * 2] & 0xff);
                }
            }

            return ret;
        }
        public ushort[] Float2Ushorts(float v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            return Byte2UShorts(bytes,true);
        }

        public float Ushort2Float(ushort[] src, int start = 0)
        {
            ushort[] tmp = new ushort[2];
            for (int i = 0; i < 2; i++)
            {
                tmp[i] = src[i + start];
            }
            byte[] bt = Ushort2Bytes(tmp);
            float ret = BitConverter.ToSingle(bt, 0);
            return ret;
        }
    }

    
    public class MODBUS_Device : Abstract_Instrument
    {
        const ushort MODE_CV = 0x0001;
        const ushort MODE_CC = 0x0002;
        const ushort MODE_OVP = 0x0004;
        const ushort MODE_OPP = 0x0008;
        const ushort MODE_OTP = 0x0010;


        public event commandStream WriteCommand;
        public event commandStream ReadCommand;
       // public string Name { get; set; }
        [DisplayName("Station ID")]
        public byte ID { get; set; }

        private ModbusInterface _myInterface;

        public List<MODBUS_REG_Struct> Registers { get; set; }

        public MODBUS_Device() { }
        public MODBUS_Device(string interface_name, string name, byte id)
            :base(interface_name,name)
        {
            //this._myInterface = (ModbusInterface)interface_name;
            ID = id; // station ID
            InstrumentName = name;
            Registers = new List<MODBUS_REG_Struct>();
            InstrumentType = _INSTRUMENT_TYPE.TYPE_MODBUS;
            _myInterface = null;
        }

        public override void StartProc()
        {
            string[] sl = InterfaceName.Split(',');
            this.ID = 1;
            if(sl.Length == 2)
            {
                int id = 0;
                if(int.TryParse(sl[1], out id))
                {
                    this.ID = (byte)id;
                }
            }
        }

        public override void StopProc()
        {
            _continueThread = false;
            waitHandle.Set();
            workingThread.Join();
            workingThread = null;
        }

        public override void SetInterface(ProtocolInterface _interface)
        {
            _myInterface = (ModbusInterface)_interface;
            
        }
        /*
         *  threadProc: check register state to write to interface
         */
        private void threadProc()
        {
            while (_continueThread)
            {
                foreach(MODBUS_REG_Struct r in Registers)
                {
                    if (r!=null  && r.Modified)
                    {
                        // create write packet
                        _myInterface.Write(this.ID, r.RegAddr, r.PacketArray());
                        Thread.Sleep(100); // short delay
                        ushort[] rb = null;
                        
                        _myInterface.Read(this.ID, r.RegAddr, r.NofRegister(), rb);
                        r.SetData(rb);
//                        waitHandle.Reset();
//                        waitHandle.WaitOne(500);
                        _setValid = r.IsRWMatch();
                        r.Modified = false;
                    }
                }
            }
        }

        public void FeedData(byte id, ushort address, ushort[] Data)
        {
            if(id == this.ID)
            {
                MODBUS_REG_Struct r;
                if((r = FindReg(address)) != null)
                {
                    
                }
            }
        }

        public override void FeedData<T>(T Data)
        {
            throw new NotImplementedException();
        }

        public void AddRegisterDefines(MODBUS_REG_Struct reg) {
            if(FindReg(reg.RegName) == null)
            {
                Registers.Add(reg);
            }
        }

        public void AddRegisterDefines(string name, ushort addr, MODBUS_REG_Struct._REG_TYPE type, float scale)
        {
            MODBUS_REG_Struct r = new MODBUS_REG_Struct();
            r.RegName = name;
            r.RegAddr = addr;
            r.Scale = scale;
            r.RegType = type;
            Registers.Add(r);
        }
        public string ReadData(string name)
        {
            int vv = 123;
            string value = "";
            MODBUS_REG_Struct r;
            if ((r = FindReg(name)) != null)
            {
                ushort[] rb = null;

                if (_myInterface.Read(this.ID, r.RegAddr, r.NofRegister(), out rb))
                {
                    r.SetData(rb);
                    value = r.string_read;
                }
                //switch (r.RegType)
                //{
                //    case MODBUS_REG_Struct._REG_TYPE.REG_S16:
                //        {
                //            short v = (short)(r.readback / r.Scale);
                //            value = v.ToString();
                //        }
                //        break;
                //    case MODBUS_REG_Struct._REG_TYPE.REG_U16:
                //        {
                //            ushort v = (ushort)(r.readback / r.Scale);
                //            value = v.ToString();
                //        }
                //        break;
                //    case MODBUS_REG_Struct._REG_TYPE.REG_S32:
                //        {
                //            Int32 v = (Int32)(r.readback / r.Scale);
                //            value = v.ToString();
                //        }
                //        break;
                //    case MODBUS_REG_Struct._REG_TYPE.REG_U32:
                //        {
                //            UInt32 v = (UInt32)(r.readback / r.Scale);
                //            value = v.ToString();
                //        }
                //        break;
                //    case MODBUS_REG_Struct._REG_TYPE.REG_F32:
                //        {
                //            float v = (float)(r.readback / r.Scale);
                //            value = v.ToString();
                //        }
                //        break;
                //}

            }
            return value;
        }

        public bool WriteData(string name, string value)
        {
            MODBUS_REG_Struct r;
            if ((r = FindReg(name)) != null)
            {
                // set register to write
                float fv = Single.Parse(value)*r.Scale;
                switch (r.RegType)
                {
                    case MODBUS_REG_Struct._REG_TYPE.REG_S16:
                        {
                            short v = (short)fv;
                            r.write = (long)(v);
                            r.write_string = v.ToString("D");
                            r.Modified = true;
                        }
                        break;
                    case MODBUS_REG_Struct._REG_TYPE.REG_U16:
                        {
                            ushort v = (ushort)fv;
                            r.write = (long)(v);
                            r.write_string = v.ToString("D");
                            r.Modified = true;
                        }
                        break;
                    case MODBUS_REG_Struct._REG_TYPE.REG_S32:
                        {
                            Int32 v = (Int32)fv;
                            r.write = (long)(v);
                            r.write_string = v.ToString("D");
                            r.Modified = true;
                        }
                        break;
                    case MODBUS_REG_Struct._REG_TYPE.REG_U32:
                        {
                            UInt32 v = (UInt32)fv;
                            r.write = (long)(v);
                            r.write_string = v.ToString("D");
                            r.Modified = true;
                        }
                        break;
                    case MODBUS_REG_Struct._REG_TYPE.REG_F32:
                        {
                            float v = fv;
                            r.write = (long)(v);
                            r.write_string = v.ToString("F");
                            r.Modified = true;
                        }
                        break;
                }
                // create write packet
                _myInterface.Write(this.ID, r.RegAddr, r.PacketArray());
                //Thread.Sleep(100); // short delay
                //ushort[] rb = null;

                //if(_myInterface.Read(this.ID, r.RegAddr, r.NofRegister(),out rb))
                //{
                //    r.SetData(rb);
                //}
                ////                        waitHandle.Reset();
                ////                        waitHandle.WaitOne(500);
                //_setValid = r.IsRWMatch();
                r.Modified = false; 
                return true;
            }

            return false;
        }

        public MODBUS_REG_Struct FindReg(string name)
        {
            foreach(MODBUS_REG_Struct r in Registers)
            {
                if (r.RegName == name)
                    return r;
            }
            return null;
        }

        public MODBUS_REG_Struct FindReg(ushort address)
        {
            foreach (MODBUS_REG_Struct r in Registers)
            {
                if (r.RegAddr == address)
                    return r;
            }
            return null;
        }

        public override void SetThreshold(string func, string low, string high)
        {
            MODBUS_REG_Struct r = FindReg(func);
            if(r != null)
            {
                double v;
                if(double.TryParse(low, out v))
                {
                    r._lowThreshold = v;
                }
                else
                {
                    r._lowThreshold = 0;
                }
                if (double.TryParse(high, out v))
                {
                    r._highThreshold = v;
                }
                else
                {
                    r._highThreshold = 0;
                }

            }

        }
        public override bool ValidRange(string func)
        {
            MODBUS_REG_Struct r = FindReg(func);
            if (r != null)
            {
                return r.InRange();
            }
            return false;
        }
        public override void SetValue(string function, string value)
        {
            _setValid = WriteData(function, value);
        }

        public override string GetValue(string function)
        {
            //          int vv = 123;
            string value = ReadData(function);

//            ReadData<T>(function);

            return value;
        }
        public override bool SetValid()
        {
            return _setValid;
        }

        public override void write()
        {

        }
        public void SetValue(ushort[] b)
        {

        }
        public override void read()
        {
        }
    }


    public enum STLinkReturnCodes
    {
        Success = 0,
        Failure = 1
    }
    public class Flash_Device : Abstract_Instrument
    {
        private const string CmdListDevices = "-List";
        private const string CmdConnectToTarget = "-c ID=0 SWD";
        private const string CmdProgramFromFile = "-P";
        private const string CmdFlashErase = "-ME";
        private const string CmdVerifyProgramming = "-V";
        private const string CmdRunFirmware = "-Run";
        private const string CmdNoPrompt = "-NoPrompt";
        private const string STLinkCLIDefaultPath = @"C:\Program Files (x86)\STMicroelectronics\STM32 ST-LINK Utility\ST-LINK Utility\ST-LINK_CLI.exe ";
        public string STLinkCLIAppPath { get; set; }


        public event commandStream WriteCommand;
        public event commandStream ReadCommand;
        // public string Name { get; set; }

        private int ErrorCode { get; set; }
        private string ErrorString { get; set; }

        public Flash_Device() { }
        public Flash_Device(string interface_name, string name)
            : base(interface_name, name)
        {
            //this._myInterface = (ModbusInterface)interface_name;
            InstrumentName = name;
            InstrumentType = _INSTRUMENT_TYPE.TYPE_FLASH;
            ErrorString = string.Empty;

            if (string.IsNullOrEmpty(interface_name))
            {
                STLinkCLIAppPath = STLinkCLIDefaultPath;
            }
            else
            {
                STLinkCLIAppPath = interface_name;
            }
        }

        public override void StartProc()
        {
            //_continueThread = true;
            //waitHandle.Reset();
            //workingThread = new Thread(threadProc);
            //workingThread.Start();
        }

        public override void StopProc()
        {
            _continueThread = false;
            waitHandle.Set();
            workingThread.Join();
            workingThread = null;
        }

        public override void SetInterface(ProtocolInterface _interface)
        {
            //_myInterface = (ModbusInterface)_interface;

        }
        /*
         *  threadProc: check register state to write to interface
         */
        private void threadProc()
        {
            while (_continueThread)
            {
            }
        }

        public override void FeedData<T>(T Data)
        {
            throw new NotImplementedException();
        }

        public string ReadData(string name)
        {
            int vv = 123;
            string value = "";
            return value;
        }

        public bool WriteData(string name, string value)
        {

            return false;
        }

        public override void SetThreshold(string func, string low, string high)
        {
            
        }

        public override bool ValidRange(string func)
        {
            if(func == "FLASH")
            {
                return (ErrorCode == 0);
            }
            return false;
        }

        public override void SetValue(string function, string value) // FLASH , hex file
        {
            if(function == "FLASH")
            {
                STLinkReturnCodes ret;
                string retstr;
                ret = FindSTLink(out retstr);
                //rtFlashMessage.Text = retstr;
                if (ret == STLinkReturnCodes.Failure)
                {
                    //MessageBox.Show("NO STLINK Found!");
                    ErrorString = retstr;
                    ErrorCode = (int)ret;
                    return ;
                }
                //string target = "F407xx";
                //var res = await Task.Run(() => stlink.ConnectToTarget(target, out retstr));
                ret = ConnectToTarget(this.InterfaceName, out retstr);
                //rtFlashMessage.Text = retstr;
                if (ret == STLinkReturnCodes.Failure)
                {
                    //MessageBox.Show("STLINK Error 01 !");
                    ErrorString = retstr;
                    ErrorCode = (int)ret;
                    return;
                }

                ret = ProgramTarget(value, 0x8000000, out retstr);
                //rtFlashMessage.Text = retstr;
                if (ret == STLinkReturnCodes.Failure)
                {
                    //MessageBox.Show("STLINK Flash Error 02!");
                    ErrorString = retstr;
                    ErrorCode = (int)ret;
                    return;
                }
                else
                {
                    //rtFlashMessage.Text = "燒錄完成,請留意板上的綠色LED灯應該在閃爍!";
                    ErrorString = "PASS";
                    ErrorCode = 0;
                }
            }
        }

        public override string GetValue(string function)
        {
            string value = string.Empty;
            
            if(function == "FLASH")
            {
                value = ErrorString;
            }
            else if(function == "CODE")
            {
                value = ErrorCode.ToString();
            }

            return value;
        }
        public override bool SetValid()
        {
            return _setValid;
        }

        public override void write()
        {

        }
        public void SetValue(ushort[] b)
        {

        }
        public override void read()
        {
        }

        public STLinkReturnCodes FindSTLink(out string a_ResultOut)
        {
            STLinkReturnCodes l_STLinkReturnCode = STLinkReturnCodes.Success;
            l_STLinkReturnCode = ExecuteCommandSync(STLinkCLIAppPath, CmdListDevices, out a_ResultOut);
            if (l_STLinkReturnCode == STLinkReturnCodes.Success)
            {
                if (a_ResultOut.Contains("No ST-LINK detected"))
                {
                    a_ResultOut = "No ST-Link found";
                    l_STLinkReturnCode = STLinkReturnCodes.Failure;
                }
                else
                {
                    a_ResultOut = "ST-Link detected";
                }
            }
            return l_STLinkReturnCode;
        }
        public STLinkReturnCodes ConnectToTarget(string a_TargetProcessor, out string a_ResultOut)
        {
            STLinkReturnCodes l_STLinkReturnCode = STLinkReturnCodes.Success;

            l_STLinkReturnCode = ExecuteCommandSync(STLinkCLIAppPath, CmdConnectToTarget, out a_ResultOut);

            if (l_STLinkReturnCode == STLinkReturnCodes.Success)
            {
                if (a_ResultOut.Contains(a_TargetProcessor))
                {
                    a_ResultOut = "Target processor found: " + a_TargetProcessor;
                    l_STLinkReturnCode = STLinkReturnCodes.Success;
                }
                else
                {
                    a_ResultOut = "Target processor not found.";
                    l_STLinkReturnCode = STLinkReturnCodes.Failure;
                }
            }
            return l_STLinkReturnCode;
        }

        public STLinkReturnCodes ProgramTarget(string a_BinFilePath, UInt32 a_FlashAddress, out string a_ResultOut)
        {
            STLinkReturnCodes l_STLinkReturnCode = STLinkReturnCodes.Success;
            a_ResultOut = string.Empty;
            if (File.Exists(a_BinFilePath) == false)
            {
                l_STLinkReturnCode = STLinkReturnCodes.Failure;
                a_ResultOut = "Bin file doesn't exist.";
            }

            if (l_STLinkReturnCode == STLinkReturnCodes.Success)
            {
                string l_CommandArguments = string.Format("{0} {1} {2} \"{3}\" 0x{4:X8} {5} {6} {7}", CmdConnectToTarget, CmdFlashErase, CmdProgramFromFile, a_BinFilePath, a_FlashAddress, CmdVerifyProgramming, CmdNoPrompt, CmdRunFirmware);

                l_STLinkReturnCode = ExecuteCommandSync(STLinkCLIAppPath, l_CommandArguments, out a_ResultOut);
                if (l_STLinkReturnCode == STLinkReturnCodes.Success)
                {
                    if (a_ResultOut.Contains("Verification...OK"))
                    {
                        a_ResultOut = "Target successfully programmed.";
                        l_STLinkReturnCode = STLinkReturnCodes.Success;
                    }
                    else
                    {
                        a_ResultOut += "Programming failed!";
                        l_STLinkReturnCode = STLinkReturnCodes.Failure;
                    }
                }
            }
            return l_STLinkReturnCode;
        }

        private STLinkReturnCodes ExecuteCommandSync(string a_AppPath, string a_CommandParameters, out string a_ResultOut)
        {
            STLinkReturnCodes l_STLinkReturnCode = STLinkReturnCodes.Success;
            a_ResultOut = string.Empty;

            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters. 
                // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit. 
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo();
                procStartInfo.FileName = a_AppPath;
                procStartInfo.Arguments = a_CommandParameters;
                // The following commands are needed to redirect the standard output.  
                //This means that it will be redirected to the Process.StandardOutput StreamReader. 
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;

                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;

                // Now we create a process, assign its ProcessStartInfo and start it
                using (Process l_process = Process.Start(procStartInfo))
                {
                    l_process.WaitForExit();

                    // Get the output into a string
                    using (StreamReader reader = l_process.StandardOutput)
                    {
                        a_ResultOut = reader.ReadToEnd();
                    }
                }
                l_STLinkReturnCode = STLinkReturnCodes.Success;
            }
            catch (Exception objException)
            {
                l_STLinkReturnCode = STLinkReturnCodes.Failure;
                a_ResultOut = "Failure";
            }
            return l_STLinkReturnCode;
        }

    }

    public class TIMER_Device : Abstract_Instrument
    {
        public string Instruction { get; set; }
        public string WriteCommand { get; set; }
        public string ValidResult { get; set; }

        public TIMER_Device() { }
        public TIMER_Device(string interface_name, string name)
            : base(interface_name, name)
        {
            InstrumentType = _INSTRUMENT_TYPE.TYPE_TIMER;
        }

        public override void FeedData<T>(T Data)
        {
            throw new NotImplementedException();
        }
        public override void read()
        {
            throw new NotImplementedException();
        }
        public override void write()
        {
            throw new NotImplementedException();
        }

        public override void SetThreshold(string func, string low, string high)
        {
            
        }

        public override bool ValidRange(string func)
        {
            return false;
        }

        public override void SetValue(string function, string value)
        {
            if(function == "DELAY")
            {
                int ms;
                if(int.TryParse(value, out ms))
                {
                    Thread.Sleep(ms);
                }
            }
        }

        public override string GetValue(string function)
        {
            return string.Empty;
        }

        public override bool SetValid()
        {
            return true;
        }

        public override void StartProc()
        {

        }

        public override void StopProc()
        {

        }

        public override void SetInterface(ProtocolInterface _interface)
        {
            //_myInterface = (USB_CAN_I7565)_interface;
        }

    }



    [XmlRoot("HY_TB_Interfface", Namespace = "http://www.grididea.com.tw", IsNullable = false)]
    public class HY_TB_Interfface
    {
        [XmlArrayItem(Type = typeof(ProtocolInterface))]
        [XmlArrayItem(Type = typeof(ModbusInterface))]
        [XmlArrayItem(Type = typeof(NATIVEInterface))]
        [XmlArrayItem(Type = typeof(USB_CAN_I7565))]
        public List<ProtocolInterface> Interfaces { get; private set; }
        public HY_TB_Interfface()
        {
            Interfaces = new List<ProtocolInterface>();
        }
        public void Add_Interface(ProtocolInterface value)
        {
            Interfaces.Add(value);
        }
        public ProtocolInterface FindInterfaceByName(string name) // name maybe consist of ##interface,##id
        {
            string[] sl = name.Split(',');
            if (sl.Length == 0) return null;

            foreach(ProtocolInterface v in Interfaces)
            {
                if (v.Name == sl[0])
                    return v;
            }
            return null;
        }
        public void InitialConfig()
        {
            foreach(ProtocolInterface p in Interfaces)
            {
                p.InitConfig();
            }
        }
    }
    [XmlRoot("HY_Test_Bench", Namespace ="http://www.grididea.com.tw",IsNullable = false)]
    public class HY_TestBench
    {
        [XmlArrayItem(Type =typeof(Abstract_Instrument))]
        [XmlArrayItem(Type =typeof(MODBUS_Device))]
        [XmlArrayItem(Type = typeof(SCPI_DC_LOAD))]
        [XmlArrayItem(Type = typeof(Flash_Device))]
        [XmlArrayItem(Type = typeof(TIMER_Device))]
        [XmlArrayItem(Type = typeof(SerialCOM_Device))]
        [XmlArrayItem(Type = typeof(CANBUS_Device))]
        public List<Abstract_Instrument> Instruments { get; private set; }
        private Abstract_Instrument _currentInstrument { get; set; }

        public HY_TestBench()
        {
            Instruments = new List<Abstract_Instrument>();
            //_port = new SerialPort();
        }

        public void InitialConfig()
        {
            foreach(Abstract_Instrument ins in Instruments)
            {
                ins.InitialConfig();
            }
        }

        public void AddInstrument(Abstract_Instrument ins)
        {
            Instruments.Add(ins);
        }

        public void SetCurrent(int id)
        {
            if(id < Instruments.Count){
                _currentInstrument = Instruments[id];
            }
            else
            {
                _currentInstrument = null;
            }
        }

        public Abstract_Instrument CurrentInstrument()
        {
            return _currentInstrument;
        }

        public void setInstrument<T>(string name,T value)
        {
            foreach (Abstract_Instrument s in Instruments)
            {
                if(s.InstrumentName == name)
                {
                    s.write();
                }
            }
        }

        public void InstrumentWrite(int id)
        {
            if(id < Instruments.Count)
            {
                Instruments.ElementAt(id).write();
            }
        }
        public void InstrumentRead(int id)
        {
            if (id < Instruments.Count)
            {
                Instruments.ElementAt(id).read();
            }
        }

        public void SetPort(SerialPort port)
        {
        }

        public int FindInstrument(string name)
        {
            for(int i = 0; i < Instruments.Count; i++)
            {
                if (Instruments.ElementAt(i).InstrumentName == name)
                    return i;
            }
            return -1;
        }
    }

    public class Recipedef
    {
        public string Process { get; set; }
        public string Instrument { get; set; } // instrument' name
        public string Function { get; set; }    // ex: VOUT, AOUT, ETC
        public string RW { get; set; }
        public string Value { get; set; }
        public string Response { get; set; }
        //public string ExpectedResult { get; set; }
        public int Delay { get; set; }
        public bool Valid { get; set; }
        public string Result { get; set; }
        public string LowThreshold { get; set; }
        public string HighThreshold { get; set; }
        public string Comment { get; set; }
        public Recipedef(string insName)
        {
            Instrument = insName;
        }
        public Recipedef()
        {

        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Recipedef> Recipes { get; set; }
        public int runIndex;
        public Recipe()
        {
            Name = "Recipe Name";
            Recipes = new List<Recipedef>();
            runIndex = 0;
        }
        public Recipe(string _name)
        {
            Name = _name;
            Recipes = new List<Recipedef>();
            runIndex = 0;
        }



    }

    [XmlRoot("Recipes", Namespace = "http://www.grididea.com.tw", IsNullable = false)]
    public class Recipes
    {
        public string DeviceID { get; set; }
        public string Name { get; set; }
        public string TimeStamp { get; set; }
        [XmlArray("RecipeList")]
        [XmlArrayItem("Recipe")]
//        public Recipe[] HYRecipe;
        public List<Recipe> HYRecipe;

        public Recipes()
        {
            HYRecipe = new List<Recipe>();
        }
    }


    [XmlRoot("WF_Modbus", Namespace = "http://www.grididea.com.tw", IsNullable = false)]
    public class WF_MODBUS_System_Config
    {
        public string DefaultWorkDir;
        public string UtilityPath;
        public WF_MODBUS_System_Config()
        {
            DefaultWorkDir = @"C:\";
            UtilityPath = "";
        }
    }

    public class RecipeExecute
    {
        public delegate void RecipeResult(object sender, Recipe r);
        public delegate void ActiveResult(object sender, int index);

        public event RecipeResult OnRecipeDone;
        public event RecipeResult OnRecipeError;
        public event ActiveResult OnIndexUpdate;

        private List<ProtocolInterface> _interface;
        private List<Abstract_Instrument> _instruments;
        private List<Recipedef> _recipes;
        private Recipe _recipeToRun;

        private EventWaitHandle _waitHandle;
        private Thread _thread;
        private int _recipeID;
        private bool _stopThread;
        private int _waitTimeout;
        public RecipeExecute()
        {
            _interface = null;
            _instruments = null;
            _recipes = null;

            _waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _thread = null;
            _waitTimeout = 200;
            _recipeToRun = null;
        }

        public void SetInterface(List<ProtocolInterface> value)
        {
            _interface = value;
        }

        public void SetInstrument(List<Abstract_Instrument> value)
        {
            _instruments = value;
        }

        public bool RunRecipe(Recipe recipe)
        {
            bool success = true;
            _recipeToRun = recipe;

            success |= (recipe != null);
           // success |= (_interface != null);
            success |= (_instruments != null);

            if (success)
            {
                _waitHandle.Reset();
                _recipeID = 0;
                _stopThread = false;
                _recipeToRun.runIndex = 0;
                foreach(Recipedef rd in _recipeToRun.Recipes)
                {
                    rd.Response = "";
                    rd.Result = "";
                }
                _thread = new Thread(procRecipe);
                _thread.Start();
            }

            return success;
        }

        public void Stop()
        {
            _stopThread = true;
            _thread.Join();
            _thread = null;
        }

        public bool IsAlive()
        {
            return (_thread != null);
        }

        private ProtocolInterface findInterface(string name)
        {
            foreach(ProtocolInterface p in _interface)
            {
                if (p.Name == name)
                    return p;
            }
            return null;
        }

        private Abstract_Instrument findInstrument(string name)
        {
            foreach(Abstract_Instrument i in _instruments)
            {
                if (i.InstrumentName == name)
                    return i;
            }
            return null;
        }

        public void ExecuteRecipe(Recipedef r)
        {
            Abstract_Instrument ins = findInstrument(r.Instrument);
            if (ins != null)
            {
                if(r.RW == "W")
                {
                    ins.SetValue(r.Function, r.Value);
                    _waitHandle.WaitOne(_waitTimeout);
                    r.Response = ins.GetValue(r.Function);
                    if (r.Valid)
                    {
                        r.Result = (ins.ValidRange(r.Function)) ? "PASS" : "FAIL";
                    }
                }
                else if(r.RW == "R")
                {
                    ins.SetThreshold(r.Function, r.LowThreshold, r.HighThreshold);
                    //ins.GetValue(r.Function);
                    //ins.SetValue(r.Function, r.Value);
                    r.Response = ins.GetValue(r.Function);
                    if (r.Valid)
                    {
                        r.Result = (ins.ValidRange(r.Function)) ? "PASS" : "FAIL";                        
                    }
                }
            }
        }

        private void procRecipe()
        {
            int i;
            for(i=0;i< _recipeToRun.Recipes.Count;i++)
            {
                OnIndexUpdate?.Invoke(this, i);
                _recipeToRun.runIndex = i;
                Recipedef r = _recipeToRun.Recipes[i];
                ExecuteRecipe(r);

                OnIndexUpdate?.Invoke(this, i);
                if (_stopThread)
                    break;

                Thread.Sleep(r.Delay);
            }


            OnIndexUpdate?.Invoke(this, i);
        }
    }


}
