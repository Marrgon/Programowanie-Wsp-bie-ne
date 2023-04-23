using NUnit.Framework;
using TPW.Dane;

namespace DaneTest
{
    public class Tests

    {
        
        [SetUp]
        public void Setup()
        {
            DaneAPI.CreateDataBall();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}