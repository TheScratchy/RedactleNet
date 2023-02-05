using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedactleNet.Model
{
    public class WebPage
    {
        public string WebData { get; private set; } 
        public WebPage(string webAddress)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            WebData = wc.DownloadString(webAddress);
        }
    }
}
