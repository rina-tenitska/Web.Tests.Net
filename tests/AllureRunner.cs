using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using NUnit.Engine;
using NUnit.Engine.Runners;

namespace Web.Tests
{
    public class AllureRunner
    {
        public static void Main(string[] args)
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var package = new TestPackage(path);
            package.AddSetting("WorkDirectory", Environment.CurrentDirectory);

            using (ITestEngine engine = TestEngineActivator.CreateInstance())
            {
                var filterService = engine.Services.GetService<ITestFilterService>();
                var builder = filterService.GetTestFilterBuilder();
                
                TestPlan.readFromEnv()?.Tests.Aggregate(builder, (builder, testCase) => {
                    builder.AddTest(testCase.Selector); 
                    return builder;
                });

                using (ITestRunner runner = engine.GetRunner(package))
                {
                    var result = runner.Run(listener: null, filter: builder.GetFilter());
                }
            }
        }
    }

    class TestPlan
    {
        public string Version { get; set; }
        public List<TestCase> Tests { get; set; }

        internal static TestPlan? readFromEnv()
        {
            var testPlanPath = Environment.GetEnvironmentVariable("ALLURE_TESTPLAN_PATH");
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

            catch (Exception)
            {
                return null;
            }
        }
    }

    class TestCase
    {
        public string Id { get; set; }
        public string Selector { get; set; }
    }
}
