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
        /// 位置情報監視の最小間隔時間(ミリ秒)
        /// </summary>
        const int MIN_TIME = (1800*1000);

        /// <summary>
        /// 位置情報監視の最小距離
        /// </summary>
        const int MIN_DISTANCE = 0;
        #endregion

        public override IBinder OnBind(Intent intent)
        {
            // This method must always be implemented
            this.Binder = new DocotServiceBinder(this);
            return this.Binder;
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

            // 初期値となる位置情報を送信する
            RegisterLatitudeLongtude();

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
            string deviceId = GetDeviceID();

            if("" != deviceId)
            { 
                var method = new HttpMethod("PATCH");


                String jsonString = "{\"latitude\":" + latitude + ",\"longitude\":" + longitude + "}";


                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(method, "http://182.163.58.118:8080/docot/v1/devices/" + deviceId + "/")
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
            }
 
            return;
        }

        public async void RegisterLatitudeLongtude()
        {
            string deviceId = GetDeviceID();

            if ("" != deviceId)
            {

                var method = new HttpMethod("PATCH");

                locator.DesiredAccuracy = 50; // <- 1. 50mの精度に指定

                String latitude;
                String longitude;

                Position position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                latitude = position.Latitude.ToString();
                longitude = position.Longitude.ToString();


                String jsonString = "{\"latitude\":" + latitude + ",\"longitude\":" + longitude + "}";


                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(method, "http://182.163.58.118:8080/docot/v1/devices/" + deviceId + "/")
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
            }

            return;
        }

        public IBinder Binder { get; private set; }

        #region 外部公開API

        public string GetFormattedTimestamp()
        {
            return "せいこう！";
        }

        public bool IsUserInfoRegistered()
        {
            bool result = false;

            ISharedPreferences prefs = this.GetSharedPreferences("DOCOT_SETTINGS", FileCreationMode.Private);

            string deviceId = prefs.GetString("DEVICE_ID", "");

            if("" != deviceId)
            {
                result = true;
            }

            Console.WriteLine("【Debug】GetSharedPreferences[deviceId = " + deviceId + "]");

            return result;
        }

        public void SetUserPreferences(string deviceId, string nickname)
        {
            Console.WriteLine("【Debug】SetUserPreferences[deviceId = " + deviceId + ", nickname = " + nickname);
            ISharedPreferences prefs = this.GetSharedPreferences("DOCOT_SETTINGS", FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("DEVICE_ID", deviceId);
            editor.PutString("NICK_NAME",nickname);
            editor.Apply();
            return;
        }

        public void RemoveUserPreference()
        {
            ISharedPreferences prefs = this.GetSharedPreferences("DOCOT_SETTINGS", FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Clear();
            editor.Apply();
            return;
        }

        public string GetDeviceID()
        {
            ISharedPreferences prefs = this.GetSharedPreferences("DOCOT_SETTINGS", FileCreationMode.Private);

            return prefs.GetString("DEVICE_ID", "");

        }


        #endregion

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