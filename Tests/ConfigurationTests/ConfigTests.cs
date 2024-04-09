namespace ConfigurationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Config.BuildConfigFile().Wait() ;
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
            Assert.That(Config.Configuration.GetType() == typeof(IDictionary<string, string>));
        }

    }
}