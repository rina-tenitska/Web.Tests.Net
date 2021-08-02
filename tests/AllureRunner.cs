using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using NUnit.Engine;
using NUnit.Engine.Extensibility;
using NUnit.Engine.Runners;


namespace Web.Tests
{
    public class AllureRunner
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
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
                    var output = runner.Run(listener: null, filter: builder.GetFilter());

                    var overallResult = TestResult.GetOverallResult(output);
                    var testMessage = TestResult.GetDetails(output);
                    string errors = TestResult.GetErrors(output);
                    if (overallResult == "Failed")
                    {
                        throw new Exception($"{errors} \n{testMessage}");    
                    }
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(testMessage);
                    Console.ResetColor();
                }
            }
        }
    }

    class TestResult
    {
        internal static string GetOverallResult(XmlNode output)
        {
            var OverallResult = GetAttribute(output, "result");
            return OverallResult;
        }
        public static string GetDetails(XmlNode output)
        {
            if (output.Name != "test-run")
                throw new Exception("The test-run element was not found.");

            var overallResult = GetAttribute(output, "result");
            var total = GetAttribute(output, "total");
            var passed = GetAttribute(output, "passed");
            var failed = GetAttribute(output, "failed");
            var skipped = GetAttribute(output, "skipped");
            var duration = Math.Round(Convert.ToSingle(GetAttribute(output, "duration"), new CultureInfo("en-US")), 2);

            var testMessage = $"\n{overallResult}! Failed - {failed}, Passed - {passed}, Skipped - {skipped}, Total - {total}, Duration: {duration.ToString()} s";

            return testMessage;
        }
        public static string GetErrors(XmlNode output)
        {
            XmlNodeList nodes = output.SelectNodes(".//test-case");
            List<string> errors = new List<string>();
            foreach (XmlNode node in nodes)
            {
                errors.Add(node.InnerText);
            }

            return string.Join("\n\n", errors);
        }
        public static string GetAttribute(XmlNode output, string name)
        {
            return output.Attributes[name]?.Value;
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