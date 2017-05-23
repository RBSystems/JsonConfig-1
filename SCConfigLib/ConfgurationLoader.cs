using System;
using System.Linq;
using Newtonsoft.Json;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public delegate void ShowSettingsPageDelegate(ushort value);
    public delegate void ShowSaveButtonDelegate(ushort value);
    public delegate void ShowRebootRequiredWarningDelegate(ushort value);
    public delegate void OnConfigurationLoadedHandler();
    public delegate void OnConfigurationSaveHandler();

    public static class ConfigurationLoader
    {
        private static bool _configChanged;
        private static bool _rebootRequired;
        public static Configuration Config { get; set; }
        public static string FileName{get; set;}

        public static bool ConfigChanged 
        {
            get { return _configChanged; }
            set
            {
                _configChanged = value;

                if (ShowSaveButton == null) return;

                var buttonValue = _configChanged ? (ushort) 1 : (ushort) 0;
                ShowSaveButton(buttonValue);
            }
        }

        public static bool RebootRequired
        {
            private get
            {
                return _rebootRequired;
            }

            set
            {
                _rebootRequired = value;

                if (ShowRebootRequired == null) return;
                var buttonValue = _rebootRequired ? (ushort)1 : (ushort)0;

                ShowRebootRequired(buttonValue);
            }
        }
           

        public static ShowSettingsPageDelegate ShowSettings {get; set;}
        public static ShowSaveButtonDelegate ShowSaveButton { get; set; }
        public static ShowRebootRequiredWarningDelegate ShowRebootRequired { get; set; }

        public static event OnConfigurationLoadedHandler OnConfigurationLoaded;
        public static event OnConfigurationSaveHandler OnConfigurationSaved;

        public static void Initialize(string fileName)
        {
            if (!File.Exists(String.Format(@"\USER\{0}",fileName)))
            {
                if (ShowSettings != null)
                {
                    ShowSettings(1);
                }

                Config = new Configuration();
            }
            else
            {
                FileName = fileName;

                LoadConfigurationFile(FileName);

                if (OnConfigurationLoaded != null && Config != null)
                {
                    OnConfigurationLoaded();
                }
                else if(Config == null)
                {
                    if (ShowSettings != null)
                    {
                        ShowSettings(1);
                    }
                }
            }
        }

        public static void LoadConfigurationFile(string fileName)
        {
            GetConfiguration(fileName);

            ConfigChanged = false;

            if (Config != null) RebootRequired = CheckSwitcherConfig(Config.Switcher);

            if (!RebootRequired) return;

            var response = String.Empty;

            CrestronConsole.SendControlSystemCommand(String.Format("progreset -p:{0}", InitialParametersClass.ApplicationNumber), ref response);
        }

        private static void GetConfiguration(string fileName)
        {
            using (var fileReader = File.OpenText(String.Format(@"\USER\{0}", fileName)))
            {
                try
                {
                    Config = JsonConvert.DeserializeObject<Configuration>(fileReader.ReadToEnd());
                }
                catch (Exception ex)
                {
                    ErrorLog.Exception("Exception loading configuration file", ex);
                    CrestronConsole.PrintLine("Exception loading configuration file: {0}", ex.Message);
                    Config = null;
                }
            }
        }

        public static void SaveConfigurationFile(string fileName)
        {
            if (File.Exists(String.Format(@"\USER\{0}", fileName)))
            {
                File.Delete(String.Format(@"\USER\{0}", fileName));
            }

            using (var fileWriter = File.CreateText(String.Format(@"\USER\{0}", fileName)))
            {
                try
                {
                    fileWriter.Write(JsonConvert.SerializeObject(Config,Formatting.Indented));

                }
                catch (Exception ex)
                {
                    ErrorLog.Exception("Exception saving configuration file", ex);
                    CrestronConsole.PrintLine("Exception saving configuration file: {0}", ex.Message);
                }
            }

            ConfigChanged = false;

            if (ShowSettings != null)
            {
                ShowSettings(0);
            }

            if (OnConfigurationSaved != null)
            {
                OnConfigurationSaved();
            }

            if (!RebootRequired) return;

            RebootRequired = false;
            var consoleResponse = String.Empty;
            CrestronConsole.SendControlSystemCommand(String.Format("progreset -p:{0}", InitialParametersClass.ApplicationNumber), ref consoleResponse);
        }

        private static bool CheckSwitcherConfig(Switcher switchConfig)
        {
            var consoleResponse = String.Empty;

            if (switchConfig.Type != 1) return false;
            var ipAddress = IPAddress.Parse(switchConfig.IpAddress);
            var ipFromSwitch = GetSwitchIpFromIpTable();
            if (Equals(ipAddress, ipFromSwitch)) return false;

            CrestronConsole.SendControlSystemCommand(
                String.Format("addp 50 {0} -p:{1}", switchConfig.IpAddress, InitialParametersClass.ApplicationNumber),
                ref consoleResponse);
            return true;
        }

        private static IPAddress GetSwitchIpFromIpTable()
        {
            var consoleResponse = String.Empty;

            CrestronConsole.SendControlSystemCommand(String.Format("ipt -t -p:{0}",InitialParametersClass.ApplicationNumber),ref consoleResponse);

            return ParseIpTableConsoleResponse(consoleResponse);
        }

        private static IPAddress ParseIpTableConsoleResponse(string response)
        {
            var splitResponse = response.Split('\n');
            var returnValue = IPAddress.Parse("0.0.0.0");

            var entry =
                splitResponse.Select(line => line.Split('|')).FirstOrDefault(splitLine => splitLine[0].Contains("50"));

            if (entry == null) return returnValue;

            CrestronConsole.PrintLine("Parsed IP Address: {0} Response: {1}",
                IPAddress.Parse(entry[5].TrimStart('0').Trim()), entry[5].TrimStart('0').Trim());
            returnValue = IPAddress.Parse(entry[5].TrimStart('0').Trim());

            return returnValue;
        }
    }
}