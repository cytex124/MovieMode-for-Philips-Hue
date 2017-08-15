using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Litmus;
using RestSharp;
using RestSharp.Deserializers;
using Q42.HueApi.ColorConverters;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ColorLightChanger
{ 
    class ColorLightChanger
    {
        private string apikey;

        private Boolean isOn = false;
        private String ip;
        public List<int> usingLights { get; set; }

        public void MovieModeOn()
        {
            this.isOn = true;
            this.StartAsync();
        }

        public async void MovieModeOff()
        {
            this.isOn = false;
            await Task.Run(() =>
            {
                Thread.Sleep(400);
                RGBColor rgb = new RGBColor("#" + Config.defaultcolor);
                Color c = Color.FromArgb(Convert.ToInt32(rgb.R), Convert.ToInt32(rgb.G), Convert.ToInt32(rgb.B));
                LightColorAttributes obj = this.calcColor(c);
                foreach (var i in this.usingLights)
                {
                    var client = this.GetClient();
                    var request = new RestRequest(String.Format("lights/{0}/state", i), Method.PUT);
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(obj);
                    var response = client.Execute(request);
                }
            });
        }

        public Color getAverageColor()
        {
            using (var bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                   Screen.PrimaryScreen.Bounds.Height,
                                   PixelFormat.Format32bppArgb))
            {
                using (var gfxScreenshot = Graphics.FromImage(bmp))
                {
                    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                    Screen.PrimaryScreen.Bounds.Y,
                                    0,
                                    0,
                                    Screen.PrimaryScreen.Bounds.Size,
                                    CopyPixelOperation.SourceCopy);
                    //Used for tally
                    int r = 0;
                    int g = 0;
                    int b = 0;

                    int total = 0;

                    for (int x = 0; x < Convert.ToInt32(bmp.Width / 50); x++)
                    {
                        for (int y = 0; y < Convert.ToInt32(bmp.Height / 50); y++)
                        {
                            Color clr = bmp.GetPixel(x * 50, y * 50);

                            r += clr.R;
                            g += clr.G;
                            b += clr.B;

                            total++;
                        }
                    }
                    //Calculate average
                    r /= total;
                    g /= total;
                    b /= total;
                    return Color.FromArgb(r, g, b);
                }
                    
            }
        }

        public LightColorAttributes calcColor(Color rgb)
        {
            float r = ((float)(rgb.R));
            float g = ((float)(rgb.G));
            float b = ((float)(rgb.B));
            float red = (r > 0.04045f) ? (float)(Math.Pow((r + 0.055f) / (1.0f + 0.055f), 2.4f)) : (r / 12.92f);
            float green = (g > 0.04045f) ? (float)(Math.Pow((g + 0.055f) / (1.0f + 0.055f), 2.4f)) : (g / 12.92f);
            float blue = (b > 0.04045f) ? (float)(Math.Pow((b + 0.055f) / (1.0f + 0.055f), 2.4f)) : (b / 12.92f);
            float X = red * 0.664511f + green * 0.154324f + blue * 0.162028f;
            float Y = red * 0.283881f + green * 0.668433f + blue * 0.047685f;
            float Z = red * 0.000088f + green * 0.072310f + blue * 0.986039f;
            float cx = X / (X + Y + Z);
            float cy = Y / (X + Y + Z);
            var obj = new LightColorAttributes();
            obj.xy = new List<float> { cx, cy };
            obj.bri = Convert.ToInt32(Y * 300);
            return obj;
        }

        private async void StartAsync()
        {
            await Task.Run(() =>
            {
                while (this.isOn)
                {
                    LightColorAttributes obj = this.calcColor(this.getAverageColor());
                    foreach (var i in this.usingLights)
                    {
                        var client = this.GetClient();
                        var request = new RestRequest(String.Format("lights/{0}/state", i), Method.PUT);
                        request.RequestFormat = DataFormat.Json;
                        request.AddBody(obj);
                        var response = client.Execute(request);
                    }
                    
                    Thread.Sleep(100);
                }
            });
        }

        public Dictionary<int, Light> SetUp(string ip, string apikey)
        {
            this.apikey = apikey;
            this.ip = ip;

            var client = this.GetClient();
            var request = new RestRequest("/lights", Method.GET);
            IRestResponse response = client.Execute(request);
            JsonDeserializer deserializer = new JsonDeserializer();
            Dictionary<int, Light> lights = deserializer.Deserialize<Dictionary<int, Light>>(response);
            return lights;
        }

        private RestClient GetClient()
        {
            return new RestClient(String.Format("http://{0}/api/{1}", this.ip, this.apikey));
        }
    }

    public class LightRow
    {
        public string name { get; set; }
        public int id { get; set; }
        public bool isOn { get; set; }
        public bool isUsed { get; set; }

        public LightRow(string name, int id, bool ison, bool isused)
        {
            this.name = name;
            this.id = id;
            this.isOn = ison;
            this.isUsed = isused;
        }
    }

    public class Light
    {

        public LightState state { get; set; }
        public LightSWUpdate swupdate { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public string manufacturename { get; set; }
        public string uniqueid { get; set; }
        public string swversion { get; set; }
    }

    public class LightState
    {
        public bool on { get; set; }
        public int bri { get; set; }
        public int hue { get; set; }
        public int sat { get; set; }
        public string effect { get; set; }
        public List<int> xy { get; set; }
        public string alert { get; set; }
        public string colormode { get; set; }
        public bool reachable { get; set; }
    }

    public class LightSWUpdate
    {
        public string state { get; set; }
        public string lastinstall { get; set; }
    }

    public class LightColorAttributes
    {
        public List<float> xy { get; set; }
        public int bri { get; set; }
    }
}
