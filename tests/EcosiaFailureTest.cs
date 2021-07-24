using NUnit.Framework;
using Web.Tests.Model;
using NUnit.Allure.Attributes;

namespace Web.Tests
{
    [TestFixture]
    public class EcosiaFailureTest : BrowserTest
    {
        [Test]
        public void EcosiaFailure()
        {
            Www.ecosia.Open();
            Assert.Equals(1, 1);

            Www.ecosia.Search("nselene dotnet");
            Www.ecosia.Results.ShouldHaveSizeAtLeast(5)
                .ShouldHaveText(0, "Consise API to Selenium for .Net");

            Www.ecosia.Results.FollowLink(0);
            Www.github.ShouldBeOn("Somewhere else");
        }
    }
}