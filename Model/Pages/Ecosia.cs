using System;
using NSelene;
using OpenQA.Selenium;
using Web.Tests.Model.Common;
using static NSelene.Selene;
using NUnit.Allure.Steps;
using NUnit.Framework;

namespace Web.Tests.Model.Pages
{
    internal class Ecosia 
    {
        public Results Results => new Results(SS(".js-result"));

        internal void Open()
        {
            Selene.Open("https://www.ecosia.org/");
        }

        internal void Search(string text)
        {
            S(By.Name("q")).Type(text).PressEnter();
        }
    }
}