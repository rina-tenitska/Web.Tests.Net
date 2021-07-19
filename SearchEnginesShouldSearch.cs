using NUnit.Framework;
using Web.Tests.Model;
using NUnit.Allure.Steps;

namespace Web.Tests
{
    public class SearchEnginesShouldSearch : BrowserTest
    {
        [Test]
        [AllureStep("Duckduckgo search `nselene dotnet`")]
        public void Duckduckgo()
        {
            Www.duckduckgo.Open();

            Www.duckduckgo.Search("nselene dotnet");
            Www.duckduckgo.Results.ShouldHaveSizeAtLeast(5)
                .ShouldHaveText(0, "Consise API to Selenium for .Net");

            Www.duckduckgo.Results.FollowLink(0);
            Www.github.ShouldBeOn("yashaka/NSelene");
        }

        [Test]
        [AllureStep("Ecosia search `nselene dotnet`")]
        public void Ecosia()
        {
            Www.ecosia.Open();

            Www.ecosia.Search("nselene dotnet");
            Www.ecosia.Results.ShouldHaveSizeAtLeast(5)
                .ShouldHaveText(0, "Consise API to Selenium for .Net");

            Www.ecosia.Results.FollowLink(0);
            Www.github.ShouldBeOn("yashaka/NSelene");
        }

        [Test]
        [AllureStep("Ecosia `nselene dotnet` should have 100 results")]
        public void EcosiaFailure()
        {
            Www.ecosia.Open();

            Www.ecosia.Search("nselene dotnet");
            Www.ecosia.Results.ShouldHaveSizeAtLeast(100)
                .ShouldHaveText(0, "Consise API to Selenium for .Net");

            Www.ecosia.Results.FollowLink(0);
            Www.github.ShouldBeOn("yashaka/NSelene");
        }
    }
}