using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();

            GetCryptoCurrencyInfo();

            DataContext = this;
        }

        public void GetCryptoCurrencyInfo()
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://api.coinmarketcap.com/v1/ticker/?limit=50"));

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

        //ComboBox Changes

        private void cb1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int rank = cb1.SelectedIndex;
            tb1.Header = $"{Cryptos[rank].name} {Cryptos[rank].price_usd}$";
        }

        private void cb2_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int rank = cb2.SelectedIndex;
            tb2.Header = $"{Cryptos[rank].name} {Cryptos[rank].price_usd}$";
        }

        private void cb3_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int rank = cb3.SelectedIndex;
            tb3.Header = $"{Cryptos[rank].name} {Cryptos[rank].price_usd}$";
        }
    }
}