using System.IO;
using System.Net;
using System.Web;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;

namespace Wikirace_Desktop
{
    public partial class Game : Window
    {
        private WikiAlgorithm WikiAlgo;
        private WikiRaceGame game;
        private ChromiumWebBrowser Browser;
        private string endPage;
        private string startPage;
        private string endPageEncoded;
        private string startPageEncoded;
        private int clicks = -2;

        public Game()
        {
            InitializeComponent();
            
            CefSettings settings = new CefSettings();

            string subprocessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"runtimes\win-x64\native\CefSharp.BrowserSubprocess.exe");
            settings.BrowserSubprocessPath = subprocessPath;
            
            string cachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CefCache");
            settings.CachePath = cachePath;

            Cef.Initialize(settings);
            
            WikiAlgo = new WikiAlgorithm();
            //Creating Browser Object since i need some stuff from it and can't add it normally to the grid
            Browser = new ChromiumWebBrowser();
            
            SetupGame();
            
            this.KeyDown += (sender, args) =>
            {
                if (args.Key == Key.F12 && Keyboard.IsKeyDown(Key.PageUp))
                {
                    Browser.ShowDevTools();
                }
            };
            
            
            
            Browser.FrameLoadEnd += (sender, args) =>
            {
                // Check if the frame that finished loading is the main frame.
                if(args.Frame.IsMain)
                {
                    
                    // Execute JavaScript code to remove Some stuff.
                    string Wikidel = File.ReadAllText("Scripts/wikidel.js");
                    Browser.ExecuteScriptAsync(Wikidel);
                    
                    this.Dispatcher.Invoke(() =>
                    {
                        if(this.startPage.Replace("Start Page: ", "") == CurrentPage.Text.Replace("Current Page: ", ""))
                        {
                            clicks = 0;
                            LinkClickCounter.Text = "Clicks: " + clicks;
                        } else {
                            clicks += 1;
                            LinkClickCounter.Text = "Clicks: " + clicks;
                        }
                    });
                }
            };
            
            //Event Handler for when the address changes
            Browser.AddressChanged += (sender, args) =>
            {
                //Check if the address is still a Wikipedia page
                if(Browser.Address != null && !Browser.Address.Contains("https://en.wikipedia.org/wiki/"))
                {
                    MessageBox.Show("You lost! Next time stay on Wikipedia!");
                    this.Close();
                    //Goto Main Menu
                    MainWindow mainMenu = new MainWindow(); 
                    mainMenu.Show();
                }

                var curr = "Current Page: " + WebUtility.UrlDecode(Browser.Address.Split("/").Last());
                curr = curr.Replace("%20", " ");
                curr = curr.Replace("_", " ");
                CurrentPage.Text = curr;
                    
                if (curr.Replace("Current Page: ", "") == this.endPage.Replace("End Page: ", ""))
                {
                    MessageBox.Show("You won!");
                    // Close the game window
                    this.Close();
                    //Goto Main Menu
                    MainWindow mainMenu = new MainWindow(); 
                    mainMenu.Show();
                }
            };
        }

        private async Task SetupGame()
        {
            this.startPage = await WikiAlgo.GetRandomWikipediaLink();
            this.endPage = await WikiAlgo.GetRandomWikipediaLink();
            this.startPageEncoded =
                "https://en.wikipedia.org/wiki/" + HttpUtility.UrlEncode(startPage).Replace("+", "%20");
            this.endPageEncoded = "https://en.wikipedia.org/wiki/" + HttpUtility.UrlEncode(endPage).Replace("+", "%20");
            game = new WikiRaceGame(startPageEncoded, endPageEncoded);

            StartPage.Text = "Start Page: " + startPage;
            EndPage.Text = "End Page: " + endPage;
            CurrentPage.Text = "Current Page: " + startPage;

            Browser.Margin = new Thickness(0, 50, 0, 0);
            Browser.Address = startPageEncoded;
            //TODO Add Loading event to the browser

            

            GameGrid.Children.Add(Browser);
        }
        
        private async void RefreshStartPage(object sender, RoutedEventArgs e)
        {
            this.startPage = await WikiAlgo.GetRandomWikipediaLink();
            this.startPageEncoded = "https://en.wikipedia.org/wiki/" + HttpUtility.UrlEncode(startPage).Replace("+", "%20");
            StartPage.Text = "Start Page: " + startPage;
            clicks = 0;
            Browser.Address = startPageEncoded;
        }

        private async void RefreshEndPage(object sender, RoutedEventArgs e)
        {
            this.endPage = await WikiAlgo.GetRandomWikipediaLink();
            this.endPageEncoded = "https://en.wikipedia.org/wiki/" + HttpUtility.UrlEncode(endPage).Replace("+", "%20");
            EndPage.Text = "End Page: " + endPage;
            Browser.Address = this.startPageEncoded;
            clicks = 0;
        }
        
        
        
    }
}