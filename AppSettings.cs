using Microsoft.Extensions.Configuration;

namespace Web.Tests
{
    public class ProjectSettings
    {
        public ProjectSettings(IConfiguration configuration)
        {
            configuration.Bind(NSeleneSettings.Key, NSelene);
            configuration.Bind(WebDriverSettings.Key, WebDriver);
        }

        public NSeleneSettings NSelene { get; set; } = new NSeleneSettings();

        public WebDriverSettings WebDriver { get; set; } 
            = new WebDriverSettings();
    }


    public class NSeleneSettings
    {
        public static readonly string Key = "NSelene";

        public double Timeout { get; set; } = 6;
        public string BaseUrl { get; set; } = "https://google.com/ncr";
        public bool SetValueByJs { get; set; }  = true;
        public bool TypeByJs { get; set; } = false;
        public bool HoldBrowserOpen { get; set; } = false;
    }

    public class WebDriverSettings
    {
        public static readonly string Key = "WebDriver";
        
        public string BrowserName { get; set; } = "";
        public bool Remote { get; set; } = false;
        public int WindowWidth { get; set; } = 1920;
        public int WindowHeight { get; set;} = 1080;
        public bool Headless { get; set; } = false;
        public string RemoteUri { get; set; } = "http://localhost:4444/wd/hub";
        public bool EnableVNC { get; set; } = true;
        public bool EnableVideo { get; set; } = false;

    }
}