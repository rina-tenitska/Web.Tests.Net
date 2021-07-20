using NUnit.Framework;
using Web.Tests.Model;
using NUnit.Allure.Attributes;

namespace Web.Tests
{
    public class EcosiaFailureTest : BrowserTest
    {
        [Test]
        [AllureSuite("EcosiaFailure")]
        public void EcosiaFailure()
        {
            Www.ecosia.Open();

            Www.ecosia.Search("nselene dotnet");
            Www.ecosia.Results.ShouldHaveSizeAtLeast(5)
                .ShouldHaveText(0, "Consise API to Selenium for .Net");

            Www.ecosia.Results.FollowLink(0);
            Www.github.ShouldBeOn("Somewhere else");
        }
    }
}