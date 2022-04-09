using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace hackathon_4_9_22
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            grabDataAsync(43.221600, -88.014832);
        }

        public async Task grabDataAsync(double latitude, double longitude)
        {
            try
            {
                using (var fc = new FlurlClient().WithHeader("User-Agent", "(dannyzolp.com, danny@zolp.dev)").WithHeader("Accept", "application/geo+json"))
                {
                    var userModel = await string.Format("https://api.weather.gov/points/{0},{1}", latitude, longitude)
                        .WithClient(fc)
                        .GetJsonAsync();

                    Console.WriteLine(JsonConvert.SerializeObject(userModel));
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
