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

#region �����o�ϐ�
        /// <summary>
        /// �ʒu���擾���C�u����
        /// </summary>
        IGeolocator locator;
        #endregion

#region �萔
        /// <summary>
        /// �ʒu���Ď��̍ŏ��Ԋu����(�b)
        /// </summary>
        const int MIN_TIME = 1800;

        /// <summary>
        /// �ʒu���Ď��̍ŏ�����
        /// </summary>
        const int MIN_DISTANCE = 0;
#endregion

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        /// �T�[�r�X�N��������
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Console.WriteLine("�yDebug�zOnStartCommand +");

            // �ʒu���擾���C�u����������������
            locator = CrossGeolocator.Current;
            // 1. 50m�̐��x�Ɏw��
            locator.DesiredAccuracy = 50; 

            // �ʒu���ύX���C�x���g�̊Ď����J�n����
            locator.PositionChanged += CrossGeolocator_Current_PositionChanged;
            locator.StartListeningAsync(MIN_TIME, MIN_DISTANCE);

            Console.WriteLine("�yDebug�zOnStartCommand -");
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// �T�[�r�X�I��������
        /// </summary>
        public override void OnDestroy()
        {
            Console.WriteLine("�yDebug�zOnDestroy() +");

            UpdateLatitudeLongtude("0", "0");

            base.OnDestroy();

            Console.WriteLine("�yDebug�zOnDestroy() -");
        }

        /// <summary>
        /// �ʒu���ύX���n���h��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CrossGeolocator_Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Console.WriteLine("�yDebug�zCrossGeolocator_Current_PositionChanged +");
            Console.WriteLine("�yDebug�z" + e.Position.Timestamp + ":" + e.Position.Latitude + ":" + e.Position.Longitude + ":" +
                e.Position.Altitude + ":" + e.Position.AltitudeAccuracy + ":" + e.Position.Accuracy + ":" + e.Position.Heading + ":" + e.Position.Speed);

            // Docot�T�[�o�[�Ɉʒu���𑗐M����
            UpdateLatitudeLongtude(e.Position.Latitude.ToString(), e.Position.Longitude.ToString());

            Console.WriteLine("�yDebug�zCrossGeolocator_Current_PositionChanged -");
        }

        /// <summary>
        /// ���[�U�[���o�^�v�� �iDocot�T�[�o�[�j
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        async Task<String> RegisterUserInfo(String nickname)
        {
            var httpClient = new HttpClient();

            // POST������e�𐶐�����
            String jsonobj = "{\"nickname\":\"" + nickname + "\"}";
            StringContent content = new StringContent(jsonobj, Encoding.UTF8, "application/json");

            RegisterUserInfoResponseData data = JsonConvert.DeserializeObject<RegisterUserInfoResponseData>(jsonobj);

            // �ǂ����ƃT�[�o�[�Ɍ����Ēʒm����
            var response = await httpClient.PostAsync("http://182.163.58.118:8080/docot/v1/devices/", content);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// �ʒu��񑗐M�v���iDocot�T�[�o�[�j
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

            locator.DesiredAccuracy = 50; // <- 1. 50m�̐��x�Ɏw��


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

#region �����N���X
        /// <summary>
        /// �ʒu���f�[�^�N���X
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