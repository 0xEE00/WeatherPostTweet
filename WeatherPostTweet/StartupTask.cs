using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using System.Xml;
using System.Net;
using System.Threading.Tasks;
using System.IO;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace WeatherPostTweet
{
    public sealed class StartupTask : IBackgroundTask
    {
        string WeatherInfo = "";

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //
            MakeWebRequest(@"http://www.hidmet.gov.rs/ciril/osmotreni/kosutnjak.xml");
        }

        async public void MakeWebRequest(string url)
        {
            //var webRequest = WebRequest.Create(@"http://www.hidmet.gov.rs/ciril/osmotreni/kosutnjak.xml");

            //using (var response1 = await webRequest.GetResponseAsync())
            //using (var content = response1.GetResponseStream())
            //using (var reader = new StreamReader(content))
            //{
            //    var strContent = reader.ReadToEnd();
            //}



            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(new Uri(url));
            WeatherInfo = await response.Content.ReadAsStringAsync();
        }
    }
}
