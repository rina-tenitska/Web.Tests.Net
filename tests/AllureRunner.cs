using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using NUnit.Engine;
using NUnit.Engine.Runners;

namespace WebTests
{
    public class AllureRunner
    {
        public const string TestPlanEnv = "ALLURE_TESTPLAN_PATH";

        public static void Main(string[] args)
        {
            var path = Assembly.GetExecutingAssembly().Location;

            var package = new TestPackage(path);
            package.AddSetting("WorkDirectory", Environment.CurrentDirectory);

            using (ITestEngine engine = TestEngineActivator.CreateInstance())
            {
                var filterService = engine.Services.GetService<ITestFilterService>();
                var builder = filterService.GetTestFilterBuilder();
                
                var testPlan = getTestPlan();
                if (testPlan != null)
                {
                    foreach (var testCase in testPlan.Tests)
                    {
                        builder.AddTest(testCase.Selector);
                    }
                }

                var filter = builder.GetFilter();
                using (ITestRunner runner = engine.GetRunner(package))
                {
                    var result = runner.Run(listener: null, filter: filter);
                }
            }
        }
        
        public static TestPlan? getTestPlan()
        {
            var testPlanPath = getTestPlanPath();
            if (!(testPlanPath != null && File.Exists(testPlanPath)))
            {
                return null;
            }
            
            try
            {
                var testPlanJson = File.ReadAllText(testPlanPath);
                
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                var plan = JsonSerializer.Deserialize<TestPlan>(testPlanJson.Replace("'", "\""), options);
                return plan;
            }

            catch (Exception e)
            {
                return null;
            }
        }
        
        public static string? getTestPlanPath()
        {
            var EnvPath = Environment.GetEnvironmentVariable(TestPlanEnv);
            return EnvPath;
        }

    }

    public class TestPlan
    {
        public string Version { get; set; }
        public List<TestCase> Tests { get; set; }
    }

    public class TestCase
    {
        public string Id { get; set; }
        public string Selector { get; set; }
    }
}
