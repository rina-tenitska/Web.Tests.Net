using System;
using NSelene;
using OpenQA.Selenium;
using Web.Tests.Model.Common;
using static NSelene.Selene;
using NUnit.Allure.Steps;
using NUnit.Framework;

namespace Web.Tests.Model.Pages
{
    internal class Duckduckgo
    {
        public Results Results => new Results(SS(".results_links_deep"));

        [AllureStep("Open")]
        internal void Open()
        {
            Selene.Open("https://duckduckgo.com/");
        }

        [AllureStep("Search `{0}`")]
        internal void Search(string text)
        {
            S(By.Name("q")).Type(text).PressEnter();
        }
    }
}