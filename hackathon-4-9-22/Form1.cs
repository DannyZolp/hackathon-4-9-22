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
        public string userLocation;
        public List<Period> forecast;

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

                    this.forecast = forecastReq.properties.periods;

                    Console.WriteLine(string.Format("It is {0}{1}", forecastReq.properties.periods[0].temperature, forecastReq.properties.periods[0].temperatureUnit));
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task grabCoordinates(string userInput)
        {
            var locationReq = await "https://geocode-api.arcgis.com/arcgis/rest/services/World/GeocodeServer/findAddressCandidates"
                .SetQueryParam("f", "pjson")
                .SetQueryParam("token", "AAPK7c62543ea6de4eff9591d102e27ef7c6hBGMaqGDrvmLt_yNZ9YBcKFWtiJGBh-qHhjz2uNoaAgmy1zwKWB-yhMciWNcsLUV")
                .SetQueryParam("singleLine", userInput)
                .GetJsonAsync<UserLocationRequest>();

            await grabWeatherDataAsync(locationReq.candidates[0].location.x, locationReq.candidates[0].location.y);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grabCoordinates(userLocation);
        }
    }
}
