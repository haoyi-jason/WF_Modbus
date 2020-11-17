using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Net.Sockets;
using System.ComponentModel;

namespace iSense
{
    class nodeInterface
    {
        SerialPort mPort;
        TcpClient mClient;        
        public nodeInterface()
        {

        }

        public bool startSerial(SerialPort p)
        {
            if(p != null)
            {
                mPort = p;
                p.DataReceived += P_DataReceived;
            }

            return true;
        }

        public bool startSocket(TcpClient client)
        {
            if(client != null)
            {
                mClient = client;
                
            }
            return true;
        }

        private void P_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CmdHeader
    {
        public byte magic1;
        public byte magic2;
        public byte type;
        public byte pid;
        public ushort len;
        public ushort chksum;
    }
}
