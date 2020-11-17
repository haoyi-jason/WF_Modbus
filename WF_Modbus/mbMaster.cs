using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NModbus;
using NModbus.Serial;
using NModbus.Utility;
using System.IO.Ports;


namespace WF_Modbus
{
    class mbMaster
    {
        private IModbusMaster mMaster;
        public mbMaster(SerialPort port)
        {
            var factory = new ModbusFactory();
            mMaster = factory.CreateRtuMaster(port);
        }

        public void writeHoldingRegister()
        {

        }
    }
}
