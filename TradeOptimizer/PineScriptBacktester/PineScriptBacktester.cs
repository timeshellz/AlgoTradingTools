using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using AlgoTrading.TradeOptimizer.Interfaces;
using AlgoTrading.TradeOptimizer.IndexManagers;

using Keys = OpenQA.Selenium.Keys;

namespace AlgoTrading.TradeOptimizer.Backtesters
{
    public class PineScriptBacktester : IStrategyBacktester
    {
        public ChromeDriver WebDriver { get; private set; }
        public PineScriptManager ScriptManager { get; private set; }
        public string ScriptName { get; set; }
        WebDriverWait Wait { get; set; }

        public bool IsScriptLoaded { get; private set; } = false;

        public PineScriptBacktester(string scriptName)
        {
            ScriptName = scriptName;
            ScriptManager = new PineScriptManager(ScriptName);

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("headless");
            options.AddArgument("--log-level=3");

            WebDriver = new ChromeDriver(@"D:\Documents\Personal\Computer Science\WebDriver\", options);
            WebDriver.Navigate().GoToUrl("https://www.tradingview.com");

            FillCookies();

            Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(1));
            WebDriver.Navigate().Refresh();

            OpenChart();

            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            CloseBacktest();
        }

        private void FillCookies()
        {
            JsonTextReader jsonReader;
            JArray deserializedJson;
            JsonSerializer jsonSerializer;

            using (FileStream stream = new FileStream(@"D:\Documents\Personal\Computer Science\WebDriver\ChromeCookies.json", FileMode.OpenOrCreate))
            {
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                jsonReader = new JsonTextReader(streamReader);

                jsonSerializer = new JsonSerializer();
                //jsonSerializer.Converters.Add(new KeyValuePairConverter());

                deserializedJson = (JArray)jsonSerializer.Deserialize(jsonReader);
            }

            foreach (JToken cookieJ in deserializedJson)
            {
                try
                {
                    WebDriver.Manage().Cookies.AddCookie(new Cookie(cookieJ["name"].ToString(), cookieJ["value"].ToString(), cookieJ["domain"].ToString(), cookieJ["path"].ToString(), DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(cookieJ["expirationDate"])).DateTime));
                }
                catch
                {

                }
            }
        }

        public void OpenChart()
        {
            Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[3]/div[3]/div[2]/div[2]/div[2]/div/ul/li[1]/a")));
            IWebElement chartButton = WebDriver.FindElement(By.XPath("/html/body/div[3]/div[3]/div[2]/div[2]/div[2]/div/ul/li[1]/a"));
            chartButton.Click();          

            IWebElement strategyButton = Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div[6]/div[1]/div/div[1]/div[1]/div[4]")));
            strategyButton.Click();
        }

        public void CloseBacktest()
        {          
            IWebElement closeIndicatorsArrow = WebDriver.FindElement(By.XPath("/html/body/div[2]/div[4]/div/div/div/div/div/div[4]/div/div/div[2]"));
            closeIndicatorsArrow.Click();

            IWebElement removeAllButton = WebDriver.FindElement(By.XPath("//*[@id=\"overlap-manager-root\"]/div/span/div[1]/div/div/div[3]"));
            removeAllButton.Click();
        }


        void ResetBottomPanel()
        {
            IWebElement notesButton = Wait.Until(ExpectedConditions.ElementToBeClickable(WebDriver.FindElement(By.XPath("/html/body/div[2]/div[6]/div[1]/div/div[1]/div[1]/div[2]"))));
            notesButton.Click();
        }

        public void PopBacktest(string script)
        {
            ResetBottomPanel();
            Thread.Sleep(200);

            IWebElement pineScriptButton = Wait.Until(ExpectedConditions.ElementToBeClickable(WebDriver.FindElement(By.XPath("/html/body/div[2]/div[6]/div[1]/div/div[1]/div[1]/div[3]"))));
            pineScriptButton.Click();

            IWebElement editor = WebDriver.FindElement(By.Id("editor"));
            Wait.Until(ExpectedConditions.ElementToBeClickable(editor));

            editor.Click();

            IWebElement textInput = editor.FindElement(By.ClassName("ace_text-input"));

            new Actions(WebDriver).KeyDown(Keys.Control).SendKeys("a").KeyUp(Keys.Control).Perform();
            new Actions(WebDriver).SendKeys(Keys.Backspace).Perform();

            Clipboard.SetText(script);

            new Actions(WebDriver).KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).Perform();
            new Actions(WebDriver).KeyDown(Keys.Control).SendKeys(Keys.Enter).KeyUp(Keys.Control).Perform();

            Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[2]/div[1]/div[2]/div[1]/div/table/tr[3]")));
        }

        public float[] GetStratProfit()
        {
            ResetBottomPanel();
            Thread.Sleep(200);

            IWebElement strategyButton = Wait.Until(ExpectedConditions.ElementToBeClickable(WebDriver.FindElement(By.XPath("/html/body/div[2]/div[6]/div[1]/div/div[1]/div[1]/div[4]"))));
            strategyButton.Click();
         
            float profit;
            float sharpe;

            int attempts = 0;

            while (true)
            {              
                try
                {
                    IWebElement report =  Wait.Until(ExpectedConditions.ElementExists(By.ClassName("reports-content")));

                    IWebElement stratProfit = Wait.Until(ExpectedConditions.ElementToBeClickable(report.FindElement(By.XPath(".//div/div/table/tbody/tr[1]/td[2]"))));
                    string profitString = stratProfit.Text.Replace("$", "");
                    profitString = profitString.Substring(0, profitString.IndexOf(Environment.NewLine));
                    profitString = profitString.Trim();
                    profit = float.Parse(profitString);
                   
                    break;
                }
                catch
                {
                    ResetBottomPanel();
                    Thread.Sleep(500);

                    strategyButton.Click();

                    if(attempts == 3)
                    {
                        return new[] { 0f, 0f };
                    }

                    attempts++;
                }
                
            }

            attempts = 0;
            while (true)
            {
                try
                {
                    IWebElement report = Wait.Until(ExpectedConditions.ElementExists(By.ClassName("reports-content")));

                    IWebElement stratSharpe = Wait.Until(ExpectedConditions.ElementToBeClickable(report.FindElement(By.XPath(".//div/div/table/tbody/tr[6]/td[2]"))));
                    string sharpeString = stratSharpe.Text.Trim();

                    if (sharpeString != "N/A")
                        sharpe = float.Parse(sharpeString);
                    else
                        sharpe = 0;

                    break;
                }
                catch
                {
                    ResetBottomPanel();
                    Thread.Sleep(500);

                    strategyButton.Click();

                    if (attempts == 3)
                    {
                        return new[] { 0f, 0f };
                    }

                    attempts++;
                }
            }

            return new float[] { profit, sharpe };            
        }

        public Dictionary<string, float> RunBacktest(Dictionary<string, float> testIndexes)
        {
            float[] profitSharpe = new float[2] { 0f, 0f };

            string script = ScriptManager.ChangeIndexes(testIndexes, false);

            PopBacktest(script);

            profitSharpe = GetStratProfit();
            CloseBacktest();

            //return profitSharpe;

            return new Dictionary<string, float>();
        }

        public Dictionary<string, float> GetStrategyIndexes()
        {
            return ScriptManager.GetFileIndexes();
        }
    }
}
