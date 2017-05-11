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

#region メンバ変数
        /// <summary>
        /// 位置情報取得ライブラリ
        /// </summary>
        IGeolocator locator;
        #endregion

#region 定数
        /// <summary>
        /// 位置情報監視の最小間隔時間(秒)
        /// </summary>
        const int MIN_TIME = 1800;

        /// <summary>
        /// 位置情報監視の最小距離
        /// </summary>
        const int MIN_DISTANCE = 0;
#endregion

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        /// サービス起動時処理
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Console.WriteLine("【Debug】OnStartCommand +");

            // 位置情報取得ライブラリを初期化する
            locator = CrossGeolocator.Current;
            // 1. 50mの精度に指定
            locator.DesiredAccuracy = 50; 

            // 位置情報変更時イベントの監視を開始する
            locator.PositionChanged += CrossGeolocator_Current_PositionChanged;
            locator.StartListeningAsync(MIN_TIME, MIN_DISTANCE);

            Console.WriteLine("【Debug】OnStartCommand -");
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// サービス終了時処理
        /// </summary>
        public override void OnDestroy()
        {
            Console.WriteLine("【Debug】OnDestroy() +");

            UpdateLatitudeLongtude("0", "0");

            base.OnDestroy();

            Console.WriteLine("【Debug】OnDestroy() -");
        }

        /// <summary>
        /// 位置情報変更時ハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CrossGeolocator_Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Console.WriteLine("【Debug】CrossGeolocator_Current_PositionChanged +");
            Console.WriteLine("【Debug】" + e.Position.Timestamp + ":" + e.Position.Latitude + ":" + e.Position.Longitude + ":" +
                e.Position.Altitude + ":" + e.Position.AltitudeAccuracy + ":" + e.Position.Accuracy + ":" + e.Position.Heading + ":" + e.Position.Speed);

            // Docotサーバーに位置情報を送信する
            UpdateLatitudeLongtude(e.Position.Latitude.ToString(), e.Position.Longitude.ToString());

            Console.WriteLine("【Debug】CrossGeolocator_Current_PositionChanged -");
        }

        /// <summary>
        /// ユーザー情報登録要求 （Docotサーバー）
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 位置情報送信要求（Docotサーバー）
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        async void UpdateLatitudeLongtude(String latitude, String longitude)
        {
            var method = new HttpMethod("PATCH");


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

#region 内部クラス
        /// <summary>
        /// 位置情報データクラス
        /// </summary>
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
#endregion

    }
}