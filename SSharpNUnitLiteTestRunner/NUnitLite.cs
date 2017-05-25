// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Crestron.SimplSharp;
using NUnit.Framework.Interfaces;

namespace NUnitLite.Tests
{
    public static class NUnitLite
    {
        // The main program executes the tests. Output may be routed to
        // various locations, depending on the sub class of TextUI
        // that is created and any arument passed.
        //
        // Examples:
        //
        // Sending output to the console works on desktop Windows and Windows CE.
        //
        //      new ConsoleUI().Execute( args );
        //
        // Debug output works on all platforms.
        //
        //      new DebugUI().Execute( args );
        //
        // Sending output to a file works on all platforms, but care must be taken
        // to write to an accessible location on some platforms.
        //
        //      string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //      string path = System.IO.Path.Combine( myDocs, "TestResult.txt" );
        //      System.IO.TextWriter writer = new System.IO.StreamWriter(output);
        //      new TextUI( writer ).Execute( args );
        //      writer.Close();
        private static readonly ConsoleRun Cr;

        static NUnitLite()
        {
            Cr = new ConsoleRun(null);

            Cr.OnTestCompleted += TestCompletedCallback;
        }

        public static void NUnitLiteSSharpTestsMain(string commandLine)
        {
            const string re = @"\G(""((""""|[^""])+)""|(\S+)) *";
            var ms = Regex.Matches(commandLine, re);
            string[] args = ms.Cast<Match>().Select(m => Regex.Replace(m.Groups[2].Success ? m.Groups[2].Value : m.Groups[4].Value, @"""""", @"""")).ToArray();

            DoTest(args);
        }

        static void DoTest(string[] args)
        {
            //if (Environment.OSVersion.Platform == PlatformID.WinCE)
            //{
            //    string path = System.IO.Path.Combine(NUnit.Env.DocumentFolder, "TestResult.txt");
            //    System.IO.TextWriter writer = new System.IO.StreamWriter(path);
            //    new TextUI(writer).Execute(args);
            //    writer.Close();
            //    //            new TcpUI("ppp_peer", 9000).Execute(args);
            //}
            //else
            //{
            //            new TcpUI("ferrari", 9000).Execute(args);
            if (args.Length == 0)
                args = new[] { "-status" };

            var arg0 = args[0].ToLowerInvariant();

            if (arg0 == "-abort")
                Cr.StopTests(true);
            else if (arg0 == "-stop" || arg0 == "-cancel")
                Cr.StopTests(false);
            else if (arg0 == "-status" || arg0 == "-s")
            {
                if (Cr.AreTestsRunning)
                    CrestronConsole.ConsoleCommandResponse("Current running test is '{0}' and previous test was '{1}'", Cr.CurrentTest, Cr.PreviousTest);
                else
                    CrestronConsole.ConsoleCommandResponse("Inactive.");
            }
            else
            {
                if (Cr.AreTestsRunning)
                {
                    CrestronConsole.ConsoleCommandResponse("Tests are running.  Current test is '{0}'.  Cancel before running additonal tests.", Cr.CurrentTest);
                    return;
                }
                Cr.Execute(args, true);
            }
            //}
        }

        private static void TestCompletedCallback(object sender, ITestResult result)
        {
            CrestronEnvironment.AllowOtherAppsToRun();
        }
    }
}