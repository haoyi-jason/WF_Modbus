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
    public abstract class AInstrument
    {
        public delegate void commandStream(object sender, ushort[] cmd);


        [DisplayName("Interface")]
        public string InterfaceName { get; set; }
        [DisplayName("Paramter")]
        public int parameter { get; set; }
        [DisplayName("Unit")]
        public string Unit { get; set; }
        [DisplayName("Scale")]
        public double Scale { get; set; }
        public byte[] Ushort2Bytes(ushort[] src, bool reverse = false)
        {
            int count = src.Length;
            byte[] dest = new byte[count << 1];
            if (reverse)
            {
                for(int i = 0; i < count; i++)
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
            if(len %2 != 0)
            {
                cnt += 1;
            }

            ushort[] ret = new ushort[cnt];
            if (reverse)
            {
                for(int i = 0; i < cnt; i++)
                {
                    ret[i] = (ushort)(src[i * 2] << 8 | src[i * 2 + 1] & 0xff);
                }
            }
            else
            {
                for (int i = 0; i < cnt; i++)
                {
                    ret[i] = (ushort)(src[i * 2+1] << 8 | src[i * 2] & 0xff);
                }
            }

            return ret;
        }
        public ushort[] Float2Ushorts(float v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            return Byte2UShorts(bytes);
        }

        public float Ushort2Float(ushort[] src,int start = 0)
        {
            ushort[] tmp = new ushort[2];
            for(int i = 0; i < 2; i++)
            {
                tmp[i] = src[i + start];
            }
            byte[] bt = Ushort2Bytes(tmp);
            float ret = BitConverter.ToSingle(bt, 0);
            return ret;
        }

        abstract public void write();
        abstract public void read();
    
    }

    /* console comm instrument
     * usually is the DUT running console commands
     */
    public class ASCInstrument : AInstrument
    {
        public delegate void CommandIO(string command);

        public event CommandIO OnCommandWrite;
        public event CommandIO OnValidResult;
        public string Instruction { get; set; }
        public string WriteCommand { get; set; }
        public string ValidResult { get; set; }

        public ASCInstrument()
        {

        }
        public override void read()
        {
            throw new NotImplementedException();
        }
        public override void write()
        {
            throw new NotImplementedException();
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


    /*
     *  single register instrument
     */
    public class SRInstrument : AInstrument
    {
        public event commandStream WriteCommand;
        public event commandStream ReadCommand;
        public string Name { get; set; }
        public ushort ReadAddress { get; set; }
        public ushort WriteAddress { get; set; }
        public byte ID { get; set; }

        public _vType Type { get; set; }
        public string ValueToWrite { get; set; }
        public string ValueReadBack { get; set; }
        
        public SRInstrument()
        {
            ReadAddress = WriteAddress = 0x0;
            ID = 0x1;
            Name = "Name";
            Type = _vType.USHORT;
            ValueToWrite = "";
            ValueReadBack = "";
        }

        public SRInstrument(string name, ushort readAddr, ushort writeAddr, byte devID)
        {
            Name = name;
            ReadAddress = readAddr;
            WriteAddress = writeAddr;
            ID = devID;
        }

        public ushort[] PacketToWrite()
        {
            ushort[] reg;
            switch (this.Type)
            {
                case _vType.USHORT:
                case _vType.SHORT:
                    reg = new ushort[] { (ushort)(double.Parse(ValueToWrite)*Scale) };
                    break;
                case _vType.FLOAT:
                    reg = Float2Ushorts(Single.Parse(ValueToWrite));
                    break;
                default:
                    reg = new ushort[] { ushort.Parse(ValueToWrite) };
                    break;
            }
            return reg;
        }
        public override void write()
        {
            //if (_port == null) return ;
            //float fv = Convert.ToSingle(value);
            ushort[] reg;
            switch (this.Type)
            {
                case _vType.USHORT:
                case _vType.SHORT:
                    reg = new ushort[]{ushort.Parse(ValueToWrite)};
                    break;
                case _vType.FLOAT:
                    reg = Float2Ushorts(Single.Parse(ValueToWrite));
                    break;
                default:
                    reg = new ushort[] { ushort.Parse(ValueToWrite) };
                    break;
            }
            //var factory = new ModbusFactory();
            //IModbusMaster master = factory.CreateRtuMaster(_port);

            //ushort[] reg = Float2Ushorts(fv);
            //            master.WriteMultipleRegisters(ID, WriteAddress, reg);
            WriteCommand?.Invoke(this,reg);
            
        }
        public void SetValue(ushort[] b)
        {
            switch (this.Type)
            {
                case _vType.USHORT:
                case _vType.SHORT:
                    ValueReadBack = (b[0]/Scale).ToString();
                    break;
                case _vType.FLOAT:
                    ValueReadBack = Ushort2Float(b).ToString();
                    break;
                default:
                    ValueReadBack = b[0].ToString();
                    break;
            }
        }
        public override void read()
        {
            //if (_port == null) return ;
            //SerialPort port = new SerialPort(this.InterfaceName, this.parameter);
            //port.Open();

            //var factory = new ModbusFactory();
            //IModbusMaster master = factory.CreateRtuMaster(port);


            // read back
            ushort[] rb = new ushort[2];
            rb[0] = ReadAddress;
            rb[1] = 2;
            ReadCommand?.Invoke(this,rb);
        }

    }
    [XmlRoot("HY_Test_Bench", Namespace ="http://www.grididea.com.tw",IsNullable = false)]
    public class HY_TestBench
    {
        public string PortName;
        public List<SRInstrument> Instruments { get; private set; }
        [XmlIgnore]
        SerialPort _port;

        public HY_TestBench()
        {
            Instruments = new List<SRInstrument>();
            _port = new SerialPort();
        }

        public void AddInstrument(SRInstrument ins)
        {
            Instruments.Add(ins);
        }

        public void setInstrument<T>(string name,T value)
        {
            foreach (SRInstrument s in Instruments)
            {
                if(s.Name == name)
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
                if (Instruments.ElementAt(i).Name == name)
                    return i;
            }
            return -1;
        }
    }

    public class Recipedef
    {
        public string Process { get; set; }
        public string Instrument { get; set; }
        public string Value { get; set; }
        public string Response { get; set; }
        public string ExpectedResult { get; set; }
        public string RW { get; set; }
        public int Delay { get; set; }
        public bool Valid { get; set; }
        public string LowThreshold { get; set; }
        public string HighThreshold { get; set; }
        public string Result { get; set; }
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
        public Recipe()
        {
            Name = "Recipe Name";
            Recipes = new List<Recipedef>();
        }
        public Recipe(string _name)
        {
            Name = _name;
            Recipes = new List<Recipedef>();
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
        public Recipe[] HYRecipe;
    }


}
