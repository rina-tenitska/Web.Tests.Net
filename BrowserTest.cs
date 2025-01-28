using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NSelene;
using static NSelene.Selene;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using AngleSharp.Text;
using Web.Tests.Common.Selenium.Remote;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Collections;

namespace Web.Tests
{
    [Parallelizable(ParallelScope.All)]
    public class BrowserTest
    {
        protected ProjectSettings Settings { get; set;}

        public BrowserTest()
        {
            IHostEnvironment env = new HostingEnvironment { 
                    EnvironmentName = Environment.GetEnvironmentVariable("Context") 
                    ?? "local" 
                };
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true
                )
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                    optional: false,
                    reloadOnChange: true
                )
                .AddEnvironmentVariables()
                .Build();

            this.Settings = new ProjectSettings(configurationRoot);
        }

        [SetUp]
        public void InitDriver()
        {
            Configuration.BaseUrl = Settings.NSelene.BaseUrl;
            Configuration.Timeout = Settings.NSelene.Timeout;
            Configuration.SetValueByJs = Settings.NSelene.SetValueByJs;

            IWebDriver webDriver;

            if (!Settings.WebDriver.Remote)
            {
                if (Settings.WebDriver.BrowserName == "chrome") 
                {
                    ChromeOptions options = new ChromeOptions();
                    Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                    options.AddArgument($"--user-data-dir={Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}1");
                    if (Settings.WebDriver.Headless) 
                    {
                        options.AddArgument("--headless");
                    }
                    webDriver = new ChromeDriver(options);
                }
                else if (Settings.WebDriver.BrowserName == "firefox")
                {
                    FirefoxOptions options = new FirefoxOptions();
                    if (Settings.WebDriver.Headless)
                    {
                        options.AddArgument("--headless");
                    }
                    webDriver = new FirefoxDriver(options);
                }
                else
                {
                    throw new Exception("This browser is not supported");
                }
                SetWebDriver(webDriver);
            }
            else
            {
                if (Settings.WebDriver.BrowserName == "chrome") 
                {
                    var options = new ChromeOptions()
                        .AddGlobal("enableVNC",
                                this.Settings.WebDriver.EnableVNC)
                        .AddGlobal("enableVideo",
                                this.Settings.WebDriver.EnableVideo);

                    if (Settings.WebDriver.Headless) {
                        options.AddArgument("--headless");
                    }
                    webDriver = new RemoteWebDriver(
                        new Uri(Settings.WebDriver.RemoteUri),
                        options);
                }
                else if (Settings.WebDriver.BrowserName == "firefox")
                {
                    FirefoxOptions  options = new FirefoxOptions()
                        .AddGlobal("enableVNC",
                                this.Settings.WebDriver.EnableVNC)
                        .AddGlobal("enableVideo",
                                this.Settings.WebDriver.EnableVideo);

                    if (Settings.WebDriver.Headless) {
                        options.AddArgument("--headless");
                    }
                    webDriver = new RemoteWebDriver(
                        new Uri(Settings.WebDriver.RemoteUri),
                        options);
                }
                else
                {
                    throw new Exception("This browser is not supported");
                }
                SetWebDriver(webDriver);
            }

        }

        [TearDown]
        public void QuitDriver()
        {
            if (Settings.NSelene.HoldBrowserOpen) return;
            GetWebDriver().Quit();
        }
    }
}