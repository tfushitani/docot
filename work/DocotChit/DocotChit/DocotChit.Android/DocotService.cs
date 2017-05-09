using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace DocotChit.Droid
{
    [Service]
    public class DocotService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Console.WriteLine("サービス開始しました");
            locator = CrossGeolocator.Current;

            locator.PositionChanged += CrossGeolocator_Current_PositionChanged;


            // if (Application.Current.Properties.ContainsKey("deviceId"))
            {
                //
                // すでに自端末情報が登録されている場合はtrueを返す
                //
                //var id = Application.Current.Properties["deviceId"] as String;
                RegisterLatitudeLongtude("34.742203", "132.886971");



            }




            return StartCommandResult.Sticky;
        }

        void CrossGeolocator_Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var position = e.Position;

            Console.WriteLine(position.Timestamp + ":" + position.Latitude + ":" + position.Longitude + ":" +
                position.Altitude + ":"+ position.AltitudeAccuracy+ ":"+ position.Accuracy+":"+position.Heading+":"+position.Speed);

            UpdateLatitudeLongtude(position.Latitude.ToString(), position.Longitude.ToString());

        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            Console.WriteLine("サービスを終了しました");
        }


        //
        // ユーザー情報登録要求
        //
        async Task<String> RegisterUserInfo(String nickname)
        {
            var httpClient = new HttpClient();

            

            // POSTする内容を生成する
            String jsonobj = "{\"nickname\":\"" + nickname + "\"}";
            StringContent content = new StringContent(jsonobj, Encoding.UTF8, "application/json");

            RegisterUserInfoResponseData data = JsonConvert.DeserializeObject<RegisterUserInfoResponseData>(jsonobj);

            // どこっとサーバーに向けて通知する
            var response = await httpClient.PostAsync("http://182.163.58.118:8080/docot/v1/devices/", content);

            return await response.Content.ReadAsStringAsync();
        }

        IGeolocator locator;

        async void UpdateLatitudeLongtude(String latitude, String longitude)
        {
            var method = new HttpMethod("PATCH");

            locator.DesiredAccuracy = 50; // <- 1. 50mの精度に指定

            String jsonString = "{\"latitude\":" + latitude + ",\"longitude\":" + longitude + "}";


            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(method, "http://182.163.58.118:8080/docot/v1/devices/MYHZKL26OVBSRP5M6ROP7MPYIQ/")
            {
                Content = content
            };

            HttpResponseMessage response = new HttpResponseMessage();
            // In case you want to set a timeout
            //CancellationToken cancellationToken = new CancellationTokenSource(60).Token;

            var client = new HttpClient();

            try
            {
                response = await client.SendAsync(request);



                // If you want to use the timeout you set
                //response = await client.SendRequestAsync(request).AsTask(cancellationToken);
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }

            String resp;
            resp = await response.Content.ReadAsStringAsync();

            Console.WriteLine(resp);

            return;
        }


        async void RegisterLatitudeLongtude(String latitude, String longitude)
        {
            var method = new HttpMethod("PATCH");

            locator.DesiredAccuracy = 50; // <- 1. 50mの精度に指定


            Position position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            latitude = position.Latitude.ToString();
            longitude = position.Longitude.ToString();


            String jsonString = "{\"latitude\":" + latitude + ",\"longitude\":" + longitude + "}";


            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(method, "http://182.163.58.118:8080/docot/v1/devices/MYHZKL26OVBSRP5M6ROP7MPYIQ/")
            {
                Content = content
            };
            
            HttpResponseMessage response = new HttpResponseMessage();
            // In case you want to set a timeout
            //CancellationToken cancellationToken = new CancellationTokenSource(60).Token;

            var client = new HttpClient();

            try
            {
                response = await client.SendAsync(request);



                // If you want to use the timeout you set
                //response = await client.SendRequestAsync(request).AsTask(cancellationToken);
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }

            String resp;
            resp =await response.Content.ReadAsStringAsync();

            Console.WriteLine(resp);

            return;
        }



        class RegisterUserInfoResponseData
        {
            public String deviceId { get; set; }
            public String nickname { get; set; }
            public int? latitude { get; set; }
            public int? longitude { get; set; }
            public int? positionUpdated { get; set; }
            public String address { get; set; }
            public String cityCode { get; set; }
            public String cityName { get; set; }

        }


    }
}