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
using Windows.UI.Popups;
using Tweetinvi;
using Windows.Foundation;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace WeatherPostTweet
{
    public sealed class StartupTask : IBackgroundTask
    {
        string WeatherInfo = "";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            //Linija ispod svaki pozadiniski proces za svaku asinhronu operaciju koja je inicijalizovana
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral(); //Sprečava zatvaranje glavnog procesa dok se u pozadini izvršavaju asinhroni procesi
            WeatherInfo = await MakeWebRequest(@"http://www.hidmet.gov.rs/ciril/osmotreni/kosutnjak.xml");
            Auth.SetUserCredentials("MuEtY8o5bgVF2kbX5RcXDp6rA", "Xw2qiJlj8qeB2pv2W7pe45Y55D9IXPrLShQb6TA8T4ZbhmHpYs", "61542070-19Fwt5IhgVeQufhBiqX7Hi9AGnsoeKLZnos4Fv4Nj", "bNjYwTVwgetZXgo6cZ5jE6tjzsh0XOCa7LLOz661hIkzj");
            Tweet.PublishTweet(WeatherInfo.Substring(0, 140));
            deferral.Complete();
        }

        public IAsyncOperation<string> MakeWebRequest(string uri) //IAsyncOperation je valida Windows Runtime povratna vrednost koja predstavlja jednu asinhronu operaciju
        {

            return this.GetUriContentAsynHelper(uri).AsAsyncOperation();
            //var dialog = new MessageDialog(WeatherInfo);
            //await dialog.ShowAsync();
        }

       

        private async Task<string> GetUriContentAsynHelper(string uri) //S obzirom da je UWP Windows Runtime ne možemo da imamo javnu metodu koja vraća Task
        {
            //Initialize WebClient
            HttpClient httpClient = new HttpClient();
            //Call service
            HttpResponseMessage response = await httpClient.GetAsync(new Uri(uri));
            //Read response
            string responseText = await response.Content.ReadAsStringAsync();
            //Return result
            return responseText;


            //Može i ovako
            /*
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);

            return content;
            */
        }
    }
}
