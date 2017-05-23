using System;
using System.Text.RegularExpressions;
using Crestron.SimplSharp;

namespace SC.SimplSharp.Utilities
{
    public class QscCallProgressDecoding
    {
        private const string MatchIncomingCall = "Incoming Call";

        public delegate void DelCallProgressResult(ushort progress, SimplSharpString string1, SimplSharpString string2);

        public DelCallProgressResult CallProgressResult { get; set; }

        public void CallProgressDecode(string progress)
        {
            var incomingCall = Regex.IsMatch(progress, MatchIncomingCall, RegexOptions.IgnoreCase) ? (ushort) 1 : (ushort) 0;
            var string1 = String.Empty;
            var string2 = String.Empty;
            var incomingCallDecode = new Regex(@"(Incoming Call)\s*(?:-|from:)\s*([^\(]*)(\(([^\)]*)\))*", RegexOptions.IgnoreCase);
                
            var matches = incomingCallDecode.Matches(progress);

            if (matches.Count > 0)
            {

                if (matches[0].Groups.Count >= 4 && matches[0].Groups[4].Length >= 1)
                {
                    string1 = matches[0].Groups[2].Value.Trim();
                    string2 = matches[0].Groups[4].Value.Trim();
                }
                else if (matches[0].Groups.Count >= 2)
                {
                    string2 = matches[0].Groups[2].Value.Trim();
                }
            }

            if (CallProgressResult != null)
            {
                CallProgressResult(incomingCall, string1, string2);
            }
        }
    }
}