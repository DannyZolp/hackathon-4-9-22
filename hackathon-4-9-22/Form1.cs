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
using hackathon_4_9_22.JsonFormats;
using Newtonsoft.Json;

namespace hackathon_4_9_22
{
    public partial class Form1 : Form
    {
        public int temperature;

        public Form1()
        {
            InitializeComponent();
            grabWeatherDataAsync(43.221600, -88.014832);
        }

        public async Task grabWeatherDataAsync(double latitude, double longitude)
        {
            try
            {
                using (var fc = new FlurlClient().WithHeader("User-Agent", "(dannyzolp.com, danny@zolp.dev)").WithHeader("Accept", "application/geo+json"))
                {
                    var pointsReq = await string.Format("https://api.weather.gov/points/{0},{1}", latitude, longitude)
                        .WithClient(fc)
                        .GetJsonAsync<PointsRequest>();

                    //Console.WriteLine("Request finished");
                    //Console.WriteLine(pointsReq);
                    //Console.WriteLine(pointsReq.properties.forecast);

                    var forecastReq = await pointsReq.properties.forecast.WithClient(fc).GetJsonAsync<ForecastRequest>();

                    this.temperature = forecastReq.properties.periods[0].temperature;

                    Console.WriteLine(string.Format("It is {0}{1}", forecastReq.properties.periods[0].temperature, forecastReq.properties.periods[0].temperatureUnit));
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
