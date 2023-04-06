using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCI_CAN_DotNET;
using System.IO.Ports;


namespace WF_Modbus
{
    class CAN_Protocol
    {
        private System.Threading.Thread WorkingThread;
        private bool ThreadRun;
        private CAN7565_HAL CANDev;
        TransferStateMachine StateMach;
        List<CANBUS_Packet> ReceivedPacket;
        List<CANBUS_Packet> PacketToSend;

        public CAN_Protocol()
        {
            ThreadRun = false;
            ReceivedPacket = new List<CANBUS_Packet>();
            PacketToSend = new List<CANBUS_Packet>();
        }
        public void Start(CAN7565_HAL canif)
        {
            CANDev = canif;
            if(CANDev != null)
            {
                ThreadRun = true;
                StateMach = new TransferStateMachine();
                WorkingThread = new System.Threading.Thread(Poller);
                ReceivedPacket.Clear();
                PacketToSend.Clear();
            }
        }

        public void Stop()
        {
            ThreadRun = false;
            WorkingThread.Join();
            WorkingThread = null;
            StateMach = null;
        }

        public int SupportedDevice(string devName)
        {
            return CANDev.IsCompatable(devName);
        }

        public bool SendPacket(CANBUS_Packet packet)
        {
            PacketToSend.Add(packet);
            StateMach.Request(TransferState.TS_SEND);
            return true;
        }

        public bool RecvPacket(byte CANID, out CANBUS_Packet packet)
        {
            if(ReceivedPacket.Count > 0)
            {
                packet = ReceivedPacket.First();
                ReceivedPacket.RemoveAt(0);
                return true;
            }
            packet = null;
            return false;
        }

        public CANBUS_Packet FindPacketByID(uint id)
        {
            CANBUS_Packet p = null;
            if (ReceivedPacket.Count > 0)
            {
                for(int i = 0; i < ReceivedPacket.Count; i++)
                {
                    if(ReceivedPacket.ElementAt(i).extID == id)
                    {
                        p = ReceivedPacket.ElementAt(i);
                        ReceivedPacket.RemoveAt(i);
                    }
                }
            }
            return p;
        }

        private void Poller()
        {
            int ret;
            byte CAN_No, Mode, RTR, DLC;
            byte[] Data = new byte[8];
            UInt32 CANID, TH, TL, DL, DH;
            CAN_No = 2;
            Mode = RTR = DLC = 0;
            CANID = TH = TL = 0;
            CANBUS_Packet packet = null;
            while (ThreadRun)
            {
                switch (StateMach.State)
                {
                    case TransferState.TS_NOT_INITIALIZE:
                        break;
                    case TransferState.TS_INITIALIZED:
                        break;
                    case TransferState.TS_IDLE:
                        for(byte cn = 1; cn < 3; cn++)
                        {
                            ret = VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(cn, ref Mode, ref RTR, ref DLC, ref CANID, ref TL, ref TH, Data);
                            if (ret == 0) // no error
                            {
                                ReceivedPacket.Add(new CANBUS_Packet());
                                CANBUS_Packet p = ReceivedPacket.Last();
                                p.portID = cn;
                                p.RTR = RTR;
                                p.DLC = DLC;
                                p.Data = (Data);
                            }
                        }
                        break;
                    case TransferState.TS_SEND:
                        if(PacketToSend.Count > 0)
                        {
                            packet = PacketToSend.First();
                            ret = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(packet.portID, 1, packet.RTR, packet.DLC, packet.extID, packet.Data);
                            if(ret == 0)
                            {
                                PacketToSend.RemoveAt(0);
                                if(PacketToSend.Count == 0)
                                {
                                    StateMach.State = TransferState.TS_IDLE;
                                    StateMach.SubState = 0;
                                }
                            }
                        }
                        break;
                    case TransferState.TS_RECEIVE:
                        break;
                    default:break;
                }
                System.Threading.Thread.Sleep(10);
            }
        }
    }


    class CAN7565_HAL
    {
        public string LastErrorString;
        public int LastError;
        public bool isOpen;
        private byte PortID;
        private uint[] baud;
        private readonly string[] Compatible = { "icpdas_i7565_1", "icpdas_i7565_2"};
        private System.Timers.Timer timer = null;
        private System.Timers.Timer polltimer = null;
        private List<CANBUS_Packet> inPackets = new List<CANBUS_Packet>();

        public CAN7565_HAL(int portid = 1, uint b1=250000, uint b2=250000)
        {
            LastError = 0;
            LastErrorString = "";
            PortID = (byte)portid;
            baud = new uint[] { b1, b2 };
            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            polltimer = new System.Timers.Timer();
            polltimer.Elapsed += Polltimer_Elapsed; ;


        }

        private void Polltimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            rcvPacket();
        }

        public int IsCompatable(string devName)
        {
            int result = -1;
            for(int i=0;i< Compatible.Length;i++)
            {
                if(Compatible[i] == devName)
                {
                    result = i;
                }
            }

            return result;
        }
        public bool OpenDevice()
        {
            bool ret = false;
            byte[] Mod_CfgData = new byte[512];
            Mod_CfgData[0] = 0;
            Mod_CfgData[1] = 0;

            int result;
            result = VCI_CAN_DotNET.VCI_SDK.VCI_OpenCAN_NoStruct(PortID, 0x2, baud[0], baud[1]);

            if (result != 0)
            {
                LastError = -1;
                LastErrorString = "Device Open Error!";
                ret = false;
            }
            else
            {
                ret = true;
            }

            isOpen = true;
            return ret;
        }

        public bool startHeartBeat(int interval, bool enable)
        {
            if (!isOpen) return false;

            if (enable)
            {
                timer.Interval = interval;
                timer.Start();
                polltimer.Interval = 100;
                polltimer.Start();
            }
            else
            {
                timer.Stop();
                polltimer.Stop();
            }
            return true;

        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CANBUS_Packet p = new CANBUS_Packet();
            p.extID = 0x20;
            p.portID = 1;
            p.RTR = 0;
           
            SendPacket(p);
            p.portID = 2;
            SendPacket(p);


            


        }

        public void CloseDevice()
        {
            VCI_CAN_DotNET.VCI_SDK.VCI_CloseCAN(PortID);
            isOpen = false;
        }

        public bool SendPacket(CANBUS_Packet packet)
        {
            int ret = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(packet.portID, 1, packet.RTR, packet.DLC, packet.extID, packet.Data);

            if(ret == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool RecvAllPacket(out List<CANBUS_Packet> packet)
        {
            packet = new List<CANBUS_Packet>();
            bool result = false;
            int Ret;
            byte CAN_No, Mode, RTR, DLC;
            byte[] Data = new byte[8];
            UInt32 CANID, TH, TL, DL, DH;

            Mode = RTR = DLC = 0;
            CANID = TH = TL = 0;
            uint nofMsg = 0;
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
                        packet.Add(p);
                    }
                }
            }
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
                        packet.Add(p);
                    }
                }
            }


            return result;
        }

        public bool readPacket(UInt32 packetID, out CANBUS_Packet packet)
        {
            foreach(CANBUS_Packet p in inPackets)
            {
                if((p.extID&0xfff) == packetID)
                {
                    packet = p;
                    return true;
                }
            }
            packet = null;

            return false;
        }
        public bool RecvPacket(UInt32 packetID,out CANBUS_Packet packet, byte portID = 1,int timeoutMS = 100)
        {
            bool result = false;
            int Ret;
            byte CAN_No, Mode, RTR, DLC;
            byte[] Data = new byte[8];
            UInt32 CANID, TH, TL, DL, DH;

            Mode = RTR = DLC = 0;
            Mode = 1;
            CANID = TH = TL = 0;
            packet = new CANBUS_Packet();
            uint nofMsg = 0;
            if(VCI_CAN_DotNET.VCI_SDK.VCI_Get_RxMsgCnt(1, ref nofMsg) == 0)
            {
                for(int i = 0; i < nofMsg; i++)
                {
                    if(VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(1, ref Mode, ref packet.RTR, ref packet.DLC, ref CANID, ref TL, ref TH, Data) == 0)
                    {
                        CANID &= 0xFFF;
                        if(CANID == packetID)
                        {
                            packet.Data = (Data);
                            result = true;
                            //break;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            if (VCI_CAN_DotNET.VCI_SDK.VCI_Get_RxMsgCnt(2, ref nofMsg) == 0)
            {
                for (int i = 0; i < nofMsg; i++)
                {
                    if (VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(2, ref Mode, ref packet.RTR, ref packet.DLC, ref CANID, ref TL, ref TH, Data) == 0)
                    {
                        CANID &= 0xFFF;
                        if (CANID == packetID)
                        {
                            packet.Data = (Data);
                            result = true;
                            //break;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }


            return result;
        }

        private void rcvPacket()
        {
            bool result = false;
            int Ret;
            byte CAN_No, Mode, RTR, DLC;
            byte[] Data = new byte[8];
            UInt32 CANID, TH, TL, DL, DH;

            Mode = RTR = DLC = 0;
            Mode = 1;
            CANID = TH = TL = 0;
            uint nofMsg = 0;
            if (VCI_CAN_DotNET.VCI_SDK.VCI_Get_RxMsgCnt(1, ref nofMsg) == 0)
            {
                for (int i = 0; i < nofMsg; i++)
                {
                    if (VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(1, ref Mode, ref RTR, ref DLC, ref CANID, ref TL, ref TH, Data) == 0)
                    {
                        bool found = false;
                        foreach (CANBUS_Packet pp in inPackets)
                        {
                            if(pp.extID == CANID)
                            {
                                Data.CopyTo(pp.Data, 0);
                                pp.RTR = RTR;
                                pp.DLC = DLC;
                                pp.extID = CANID;
                                found = true;
                                //inPackets.Remove(pp);
                            }
                        }
                        if (!found)
                        {
                            CANBUS_Packet p = new CANBUS_Packet();
                            Data.CopyTo(p.Data, 0);
                            p.RTR = RTR;
                            p.DLC = DLC;
                            p.extID = CANID;
                            inPackets.Add(p);
                        }
                    }
                }
            }
            if (VCI_CAN_DotNET.VCI_SDK.VCI_Get_RxMsgCnt(2, ref nofMsg) == 0)
            {
                for (int i = 0; i < nofMsg; i++)
                {
                    if (VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(2, ref Mode, ref RTR, ref DLC, ref CANID, ref TL, ref TH, Data) == 0)
                    {
                        bool found = false;
                        foreach (CANBUS_Packet pp in inPackets)
                        {
                            if (pp.extID == CANID)
                            {
                                Data.CopyTo(pp.Data, 0);
                                //                                pp.Data = Data;
                                pp.RTR = RTR;
                                pp.DLC = DLC;
                                pp.extID = CANID;
                                found = true;
                                if((Data[3] == 0x04) && (CANID == 0x21110))
                                {
                                    found = true;
                                }
                            }
                        }
                        if (!found)
                        {
                            CANBUS_Packet p = new CANBUS_Packet(); ;
                            Data.CopyTo(p.Data, 0);
                            //p.Data = Data;
                            p.RTR = RTR;
                            p.DLC = DLC;
                            p.extID = CANID;
                            inPackets.Add(p);
                        }
                    }
                }
            }
        }
    }

 
public class CANBUS_Packet
    {
        public int DeviceID;
        public byte portID;
        public uint extID;
        public byte RTR;
        public byte DLC;
        public byte[] Data = new byte[8];
    }

    public enum TransferState
    {
        TS_NOT_INITIALIZE,
        TS_INITIALIZED,
        TS_IDLE,
        TS_SEND,
        TS_RECEIVE,
        TS_NOFSTATE
    }
    class TransferStateMachine
    {
        public TransferState State { get; set; }
        public TransferState RequestState { get; set; }
        public int SubState { get; set; }

        public bool Request(TransferState state)
        {
            if(State == TransferState.TS_IDLE)
            {
                this.RequestState = state;
                return true;
            }
            else if (!this.HasRequest())
            {
                this.RequestState = state;
                return true;
            }
            return false;
        }

        public bool HasRequest()
        {
            return this.RequestState == TransferState.TS_IDLE ? false : true;
        }

        public void Valid()
        {
            if (this.HasRequest())
            {
                this.State = this.RequestState;
                this.SubState = 0;
                this.RequestState = TransferState.TS_IDLE;
            }
            else
            {
                this.State = TransferState.TS_IDLE;
                this.SubState = 0;
            }
        }

    }
}

