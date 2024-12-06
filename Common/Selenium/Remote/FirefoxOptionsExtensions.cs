
using OpenQA.Selenium.Firefox;

namespace Web.Tests.Common.Selenium.Remote
{
    public static class FirefoxOptionsExtensions
    {
        public static FirefoxOptions AddGlobal(this FirefoxOptions options, string capabilityName, object capabilityValue)
        {
            options.AddAdditionalOption(capabilityName, capabilityValue); 
            return options;
        }
    }
}