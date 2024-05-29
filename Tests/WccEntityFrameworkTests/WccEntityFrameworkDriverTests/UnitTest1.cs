namespace WccEntityFrameworkDriverTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {



        }

        [Test]
        public void Test1()
        {
            CryptoHelper helper = new CryptoHelper();

            long result = helper.TestBcryptSpeed("dwqjndbqwhjbdwiqdqwhduihqwidq", 30);
            TestContext.WriteLine($"elapsed time - {result}ms");
            Assert.Pass();

        }
    }
}