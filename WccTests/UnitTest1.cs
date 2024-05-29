using SelfSignedPfxGenerator;

namespace WccTests
{
    public class Tests
    {
        crypto c;


        [OneTimeSetUp]
        public void OTsetup()
        {
            c = new crypto();

            var pfxBytes = c.GenerateSelfSignedCertificatePfx("CN=Test", "");

            File.WriteAllBytes("selfsigned.pfx", pfxBytes);


        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {



        }
    }
}