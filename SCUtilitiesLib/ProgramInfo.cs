using System;
using System.Collections.Generic;
using Crestron.SimplSharp;      // For Basic SIMPL# Classes

namespace SC.SimplSharp.Utilities
{
    public static class ProgramInfo
    {
        private static Dictionary<string,Action<String>> _responseDictionary; 

        public static ProgramComments SystemInformation { get; set; }

        public static event EventHandler ProgramInformationUpdated;

        public static void Initialize()
        {
            _responseDictionary = new Dictionary<string, Action<string>>
            {
                {"Program File", s => SystemInformation.ProgFile = s},
                {"Programmer", s => SystemInformation.Programmer = s},
                {"Compiled On", s => SystemInformation.CompileDate = s},
                {"Source Env", s => SystemInformation.CompileVersion = s}
            };

            SystemInformation = new ProgramComments
            {
                BootDir = InitialParametersClass.ProgramDirectory.ToString(),
                Firmware = InitialParametersClass.FirmwareVersion
            };

            var progCommentsResponse = String.Empty;

            CrestronConsole.SendControlSystemCommand(String.Format("progcomments:{0}",
                InitialParametersClass.ApplicationNumber), ref progCommentsResponse);

            ProcessProgComments(progCommentsResponse);
        }

        private static void ProcessProgComments(string progCommentsResponse)
        {
            var splitResponse = progCommentsResponse.Split('\n');

            foreach (var s in splitResponse)
            {
                if (!s.Contains(":")) continue;

                var key = s.Split(':')[0];
                var value = s.Split(':')[1];

                if (_responseDictionary.ContainsKey(key))
                {
                    _responseDictionary[key].Invoke(key == "Compiled On"
                        ? String.Format("{0}:{1}", value.Trim(), s.Split(':')[2].Trim())
                        : value.Trim());
                }
            }

            if (ProgramInformationUpdated != null)
            {
                ProgramInformationUpdated(null, null);
            }
        }
    }
}