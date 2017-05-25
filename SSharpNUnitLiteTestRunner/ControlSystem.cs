
using Crestron.SimplSharp;                          // For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       // For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        // For Threading

namespace SSharpNUnitLiteSSharpTestRunner
{
    public class ControlSystem : CrestronControlSystem
    {

        /// <summary>
        /// Constructor of the Control System Class. Make sure the constructor always exists.
        /// If it doesn't exit, the code will not run on your 3-Series processor.
        /// </summary>
        public ControlSystem()
        {

            // Set the number of threads which you want to use in your program - At this point the threads cannot be created but we should
            // define the max number of threads which we will use in the system.
            // the right number depends on your project; do not make this number unnecessarily large
            Thread.MaxNumberOfUserThreads = 100;

            if (!CrestronConsole.AddNewConsoleCommand(NUnitLite.Tests.NUnitLite.NUnitLiteSSharpTestsMain, "nunit", "Run NUnitLite tests", ConsoleAccessLevelEnum.AccessOperator))
                ErrorLog.Error("Cannot add nunit command");
        }

        /// <summary>
        /// Overridden function... Invoked before any traffic starts flowing back and forth between the devices and the 
        /// user program. 
        /// This is used to start all the user threads and create all events / mutexes etc.
        /// This function should exit ... If this function does not exit then the program will not start
        /// </summary>
        public override void InitializeSystem()
        {

        }
    }
}
