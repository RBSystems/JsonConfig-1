using System;
using System.Linq;
using System.Text.RegularExpressions;
using Crestron.SimplSharp;
using SC.SimplSharp.Utilities.JSON;

namespace SC.SimplSharp.Utilities
{
    public class IpTable
    {
        public bool CheckIfRestartNeeded(Switcher switchConfig)
        {
            if (switchConfig.Type != 1)
            {
                return false;
            }

            var ipAddress = IPAddress.Parse(switchConfig.IpAddress);

            var ipFromSwitch =
                GetSwitcherIpFromIpTable(
                    SendControlSystemCommand(String.Format("ipt -t -p:{0}", InitialParametersClass.ApplicationNumber)));

            if (Equals(ipAddress, ipFromSwitch))
            {
                return false;
            }

            SendControlSystemCommand(String.Format("addp 50 {0} -p:{1}", switchConfig.IpAddress,
                InitialParametersClass.ApplicationNumber));

            return true;
        }

        private string SendControlSystemCommand(string commandToSend)
        {
            var response = String.Empty;

            CrestronConsole.SendControlSystemCommand(commandToSend, ref response);

            return response;
        }

        private IPAddress GetSwitcherIpFromIpTable(string response)
        {
            var splitResponse = response.Split('\n');
            var returnValue = IPAddress.Parse("0.0.0.0");

            var entry =
                splitResponse.Select(line => line.Split('|')).FirstOrDefault(splitLine => splitLine[0].Contains("50"));

            if (entry == null)
            {
                return returnValue;
            }

            var ipAddressString = entry[5].TrimStart('0').Trim();

            returnValue = IPAddress.Parse(Regex.Replace(ipAddressString, "0*([0-9]+)", "${1}"));

            return returnValue;
        }
    }
}