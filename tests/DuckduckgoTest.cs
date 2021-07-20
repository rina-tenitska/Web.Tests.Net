using NUnit.Framework;
using Web.Tests.Model;
using NUnit.Allure.Steps;

namespace Web.Tests
{
    public class DuckduckgoTest : BrowserTest
    {
        [Test]
        public void DuckduckgoSearchTest()
        {
            Www.duckduckgo.Open();

            Www.duckduckgo.Search("nselene dotnet");
            Www.duckduckgo.Results.ShouldHaveSizeAtLeast(5)
                .ShouldHaveText(0, "Consise API to Selenium for .Net");

            Www.duckduckgo.Results.FollowLink(0);
            Www.github.ShouldBeOn("yashaka/NSelene");
        }
    }
}
        