using System.Diagnostics;

namespace ConfigurationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Config.BuildConfigFile().Wait();
        }

        [TearDown]
        public void Teardown()
        {
            // File.Delete(Config.configFilename);
        }

        [Test]
        public void ConfigFileCreationTests()
        {
            Assert.That(File.Exists(Config.configFilename));
        }

        [Test]
        public async Task ParseConfigTest()
        {
            await Config.ParseConfig();
            Assert.That(Config.Configuration.GetType() == typeof(List<ConfigData>));
            Config.ParsedConfiguration.ForEach(x => Debug.WriteLine(x));
            Assert.That(Config.ParsedConfiguration.Count() > 0);
        }

        [Test]
        [TestCase("bacConfig=badvalue")]
        [TestCase("$!@%$@%!^%")]
        [TestCase("@")]
        public async Task InvalidConfigParseTest(string badValues)
        {
           
            await File.AppendAllTextAsync(Config.configFilename,badValues+"\n"+"BADSetting" );
            
            ConfigurationException ex =Assert.ThrowsAsync<ConfigurationException>(()=>Config.ParseConfig());
            TestContext.WriteLine($"Exception for bad values was thrown - {ex.GetType().Name} - {ex.Message}");
        }

    }
}