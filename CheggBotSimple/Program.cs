using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.IO;
using OpenQA.Selenium.Remote;

namespace CheggBotSimple
{

    /// <summary>
    /// Create Simple Login into Chegg Site With .Net 5
    /// Download Source Page 
    /// Take ScreenShot
    /// uisng Selenium For Intarcting With Web Browser in My case (Chrome)  See More Details : https://www.selenium.dev/documentation/en/
    /// </summary>
    /// 
    class Program
    {

        private static Random rnd = new Random();

        static async Task Main(string[] args)
        {
            try
            {
                string email = @"email";
                string passWord = "pass";


                var option = new  ChromeOptions();

                option.AcceptInsecureCertificates = true;
                option.AddArguments("--ignore-certificate-errors", "--ignore-ssl-errors", "enable-logging");

                using IWebDriver _driver = new ChromeDriver(option);

                // Chegg Login Page--->https://www.chegg.com/auth?action=login&redirect=https%3A%2F%2Fwww.chegg.com%2F
                _driver.Navigate().GoToUrl(@"https://www.chegg.com/auth?action=login&redirect=https%3A%2F%2Fwww.chegg.com%2F");

                if (_driver.PageSource == null)
                    Console.WriteLine("Error");
                else
                    Console.WriteLine("Web Page Sucssfuly Loaded");


                if (LoginIntoAccount(email, passWord, _driver))
                    Console.WriteLine("bot Loged in...");
                else
                    Console.WriteLine("Error On Log in..");


                Console.Write("Trying To Download html page ... \t");


                var downloadOperation = await DownloadHtmlDocument(_driver);
                if (downloadOperation.Item1)
                    Console.WriteLine($"File : {downloadOperation.Item2} has Succfuly downloaded");
                else
                    Console.WriteLine("Error While Downloading source page");

                var screenShotOperation =  TakeScreenShot(_driver);
                if (screenShotOperation.Item1)
                    Console.WriteLine($"Screen Shot Scussfuly Taken Image name : {screenShotOperation.Item2} ");
                  else
                    Console.WriteLine($"Error on opearion ");



                Console.WriteLine("press Any Key To Exit ");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




        private static bool LoginIntoAccount(string email, string passWord, IWebDriver driver)
        {
            try
            {
                IWebElement user = driver.FindElement(By.Id("emailForSignIn"));
                IWebElement pass = driver.FindElement(By.Id("passwordForSignIn"));

                IWebElement button = driver.FindElement(By.Name("login"));

                user.SendKeys(email);
                Thread.Sleep(rnd.Next(1000, 3000));

                pass.SendKeys(passWord);
                Thread.Sleep(rnd.Next(1000, 3000));

                button.Click();

                Thread.Sleep(3000);

                if (driver.Url != @"https://www.chegg.com/auth?action=login&redirect=https%3A%2F%2Fwww.chegg.com%2F")
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static async Task<(bool, string)> DownloadHtmlDocument(IWebDriver driver)
        {
            try
            {
                if (driver.PageSource != null)
                {
                    var filename = DateTime.Now.ToString("ddHHmmssfff") + ".html";

                    //F:\ConsoleAPP\CheggBotSimple\CheggBotSimple\bin\Debug\net5.0 ------- Returned Path
                    // await File.WriteAllTextAsync(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location + @"\pageSoures"+DateTime.Now.ToString("dd HH mm ss fff")+".html"), driver.PageSource);

                    //Hard Coded Path
                    await File.WriteAllTextAsync(@"F:\ConsoleAPP\CheggBotSimple\CheggBotSimple\pageSoures\" + filename, driver.PageSource, System.Text.Encoding.UTF8);
                    return (true, filename);
                }
                else
                    return (true, default);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (true, default);
            }
        }

        private static (bool,string) TakeScreenShot(IWebDriver driver)
        {
            try
            {
                var filename = DateTime.Now.ToString("HHmmssfff")+".png";
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();

                 ss.SaveAsFile(@"F:\ConsoleAPP\CheggBotSimple\CheggBotSimple\ScreenShots\"+filename,ScreenshotImageFormat.Png);
                return (true, filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (true, default);
            }
        }

    }
}
