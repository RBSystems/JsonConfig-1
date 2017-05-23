using System;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Ssh;
using Newtonsoft.Json;

namespace SC.SimplSharp.Utilities
{
    public static class SettingsStore
    {
        private static bool _initialized;
        public static Settings ProgramSettings = new Settings();

        public delegate void OnUshortSettingsLoadedEventHandler(ushort index, ushort value);
        public delegate void OnStringSettingsLoadedEventHandler(ushort index, SimplSharpString value);
        public delegate void OnBoolSettingsLoadedEventHandler(ushort index, ushort value);

        public static event OnUshortSettingsLoadedEventHandler OnUshortSettingsLoaded;
        public static event OnStringSettingsLoadedEventHandler OnStringSettingsloaded;
        public static event OnBoolSettingsLoadedEventHandler OnBoolSettingsLoaded;

        public static void Initialize()
        {
            string[] nvramFileList = {};

            if (Directory.Exists(String.Format(@"\NVRAM\{0}\{0} settings.dat", InitialParametersClass.ProgramIDTag)))
            {
                nvramFileList =
                    Directory.GetFiles(String.Format(@"\NVRAM\{0}\{0} settings.dat", InitialParametersClass.ProgramIDTag));
            }
            else
            {
                Directory.Create(String.Format(@"\NVRAM\{0}", InitialParametersClass.ProgramIDTag));
            }

            if (nvramFileList.Length == 0)
            {
                CrestronConsole.PrintLine("No settings file found. Creating settings file.");

                File.Create(String.Format(@"\NVRAM\{0}\{0} settings.dat", InitialParametersClass.ProgramIDTag));
            }
            else if(!_initialized)
            {
                using (var fileReader = File.OpenText(nvramFileList[0]))
                {
                    try
                    {
                        ProgramSettings = JsonConvert.DeserializeObject<Settings>(fileReader.ReadToEnd());

                        FireEvent();
                    }
                    catch (Exception ex)
                    {
                        CrestronConsole.PrintLine("Exception processing {0}: {1}", nvramFileList[0], ex.Message);
                        ProgramSettings = null;
                    }
                }
            }

            _initialized = true;
        }

        public static void AddUshortValue(ushort key, ushort value)
        {
            if (ProgramSettings.UshortDict.ContainsKey(key))
            {
                ProgramSettings.UshortDict[key] = value;
                return;
            }

            ProgramSettings.UshortDict.Add(key, value);
        }

        public static void AddStringValue(ushort key, string value)
        {
            if (ProgramSettings.StringDict.ContainsKey(key))
            {
                ProgramSettings.StringDict[key] = value;
                return;
            }

            ProgramSettings.StringDict.Add(key, value);
        }

        public static void AddBoolValue(ushort key, ushort value)
        {
            if (ProgramSettings.BoolDict.ContainsKey(key))
            {
                ProgramSettings.BoolDict[key] = value == 1;
                return;
            }

            ProgramSettings.BoolDict.Add(key, value == 1);
        }

        public static void SaveFile()
        {
            using (
                var fileWriter =
                    File.OpenWrite(String.Format(@"\NVRAM\{0}\{0} settings.dat", InitialParametersClass.ProgramIDTag)))
            {
                fileWriter.Write(JsonConvert.SerializeObject(ProgramSettings), Encoding.Default);
            }
        }

        private static void FireEvent()
        {
            foreach (var setting in ProgramSettings.UshortDict)
            {
                OnUshortSettingsLoaded(setting.Key, setting.Value);
            }

            foreach (var setting in ProgramSettings.StringDict)
            {
                OnStringSettingsloaded(setting.Key, new SimplSharpString(setting.Value));
            }

            foreach (var setting in ProgramSettings.BoolDict)
            {
                var value = setting.Value ? 1 : 0;

                OnBoolSettingsLoaded(setting.Key, (ushort) value);
            }
        }
    }
}