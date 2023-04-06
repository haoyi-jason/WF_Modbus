using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace STLINK_ADAPTER
{
    public class STLinkAdapter
    {

        //==================================================== 
        #region Data Properties 
        private const string CmdListDevices = "-List";
        private const string CmdConnectToTarget = "-c ID=0 SWD";
        private const string CmdProgramFromFile = "-P";
        private const string CmdFlashErase = "-ME";
        private const string CmdVerifyProgramming = "-V";
        private const string CmdRunFirmware = "-Run";
        private const string CmdNoPrompt = "-NoPrompt";
        private const string STLinkCLIDefaultPath = @"C:\Program Files (x86)\STMicroelectronics\STM32 ST-LINK Utility\ST-LINK Utility\ST-LINK_CLI.exe ";
        public string STLinkCLIAppPath { get; set; }
        #endregion

        //======================================================= 
        #region Constructor 
        public STLinkAdapter(string a_STLinkCLIAppPath)
        {
            if (string.IsNullOrEmpty(a_STLinkCLIAppPath))
            {
                STLinkCLIAppPath = STLinkCLIDefaultPath;
            }
            else
            {
                STLinkCLIAppPath = a_STLinkCLIAppPath;
            }
        }
        #endregion

        //======================================================= 
        #region Procedures 
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

        //----------------------------------- 
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



        //----------------------------------- 
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

        //----------------------------------- 
        /// <summary> 
        /// Executes a shell command synchronously. 
        /// </summary> 
        /// <param name="command">string command</param> 
        /// <returns>string, as output of the command.</returns> 
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
        //-----------------------------------
        #endregion
    }

    public enum STLinkReturnCodes
    {
        Success = 0,
        Failure = 1
    }
}
