using System;
using NSelene;
using Web.Tests.Common.Selene;
using NUnit.Allure.Steps;
using NUnit.Framework;

namespace Web.Tests.Model.Pages
{
    internal class Github
    {
        [AllureStep("Title should be `{0}`")]
        internal void ShouldBeOn(string pageTitleText)
        {
            Selene.WaitTo(Match.TitleContaining(pageTitleText));
        }
    }
}