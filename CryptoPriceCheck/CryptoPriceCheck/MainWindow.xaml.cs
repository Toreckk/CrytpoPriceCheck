using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CryptoPriceChecker.Crypto;
using Newtonsoft.Json;

namespace CryptoPriceCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Coin> Cryptos { get; set; }
        public int ActiveCoinRank { get; set; }

        public MainWindow()
        {
            ActiveCoinRank = 0;
            GetCryptoCurrencyInfo();
            InitializeComponent();

            UpdateUI(ActiveCoinRank);//We start showing the data for the #1 coin

            //dispatchtimer will update the UI every 30s
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimeronTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 20);//will update every 1 min
            dispatcherTimer.Start();
        }

        private void DispatcherTimeronTick(object sender, EventArgs eventArgs)
        {
            GetCryptoCurrencyInfo();
            UpdateUI(ActiveCoinRank);
        }

        public void GetCryptoCurrencyInfo()
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://api.coinmarketcap.com/v1/ticker/"));

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            Cryptos = JsonConvert.DeserializeObject<List<Coin>>(jsonString);
        }

        public void UpdateUI(int CoinRank)
        {
            lblName.Content = $"{Cryptos[CoinRank].name}";
            lblPrice.Content = $"${Cryptos[CoinRank].price_usd}";
            lblRank.Content = $"#{Cryptos[CoinRank].rank}";
            SearchBar.Text = "";
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string SearchedTerm = SearchBar.Text.ToLower();
            List<string> Suggestions = new List<string>();

            for (int i = 0; i < Cryptos.Count; i++)
            {
                if (!string.IsNullOrEmpty(SearchBar.Text))
                {
                    if (Cryptos[i].name.ToLower().Contains(SearchedTerm) || Cryptos[i].id.ToLower().Contains(SearchedTerm) ||
                        Cryptos[i].symbol.ToLower().Contains(SearchedTerm))
                    {
                        Suggestions.Add(Cryptos[i].name);
                    }
                }
            }
            if (Suggestions.Count > 0)//if there are available suggestions
            {
                lblSuggestions.ItemsSource = Suggestions;
                lblSuggestions.Visibility = Visibility.Visible;
                lblSuggestions.IsDropDownOpen = true;
            }
            else if (SearchBar.Text.Equals(""))
            {
                lblSuggestions.Visibility = Visibility.Collapsed;
                lblSuggestions.ItemsSource = null;
            }
            else
            {
                lblSuggestions.Visibility = Visibility.Collapsed;
                lblSuggestions.ItemsSource = null;
            }
        }

        private void lblSuggestions_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (lblSuggestions.ItemsSource != null)
            {
                lblSuggestions.Visibility = Visibility.Collapsed;
                //SearchBar.TextChanged += new TextChangedEventHandler(SearchBar_TextChanged);
                SearchBar.Text = lblSuggestions.SelectedItem.ToString();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //find rank of coin
            foreach (Coin cn in Cryptos)
            {
                if (cn.name.ToLower().Equals(SearchBar.Text.ToLower()))
                {
                    ActiveCoinRank = Int32.Parse(cn.rank) - 1;
                }
            }
            UpdateUI(ActiveCoinRank);
        }
    }
}