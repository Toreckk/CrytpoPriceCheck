using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoPriceChecker.Crypto
{
    public class Coin
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string rank { get; set; }

        public string price_usd { get; set; }
        public string price_btc { get; set; }

        [JsonProperty(PropertyName = "24h_volume_usd")]
        public string volume_usd_24h { get; set; }//Cant start with a number

        public string market_cap_usd { get; set; }
        public string available_supply { get; set; }
        public string total_supply { get; set; }
        public string percent_change_1h { get; set; }
        public string percent_change_24h { get; set; }
        public string percent_change_7d { get; set; }
        public string last_updated { get; set; }
    }
}