// For Basic SIMPL# Classes
using Crestron.SimplSharp.Reflection;
// For Basic SIMPL#Pro classes
using NUnit.Framework;
using SC.SimplSharp.Utilities;
using SC.SimplSharp.Utilities.JSON;

namespace SCConfigLibTests
{
    [TestFixture]
    public class SettingsReaderTests
    {
        private readonly ISettingsReader _settingsReader = new SettingsReader(@"\USERS\SCConfig.dat", "EnvironmentalControls");

        [Test]
        public void ReturnedTypeShouldMatchObjectGiven()
        {
            var result = _settingsReader.LoadSection<EnvironmentalControls>();

            Assert.IsNotNull(result);
        }
    }
}

