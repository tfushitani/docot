﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



using System.Net.Http;
using Newtonsoft.Json;

namespace DocotChit
{
	public partial class MainPage : ContentPage
	{
        private ServiceConnectionStub m_docotService { get; set; }
       

        public MainPage()
		{
			InitializeComponent();
		}

        public MainPage(ServiceConnectionStub x)
        {
            InitializeComponent();

            m_docotService = x;


        }

        async void CameraButton_ClickedAsync(object sender, System.EventArgs e)
        {
            var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                AllowCropping = false,
            });
            if (file == null) { return; }

            cameraImage.Source = ImageSource.FromStream(() => file.GetStream());
        }

        void RemoveButton_ClickedAsync(object sender, System.EventArgs e)
        {
            m_docotService.RemoveUserPreference();
        }

            void RegistButton_ClickedAsync(object sender, System.EventArgs e)
        {
            m_docotService.RegisterLatitudeLongtude();
        }


        //
        // 登録・更新ボタンクリック イベントハンドラ
        //
        async void Button_ClickedAsync(object sender, System.EventArgs e)
        {
            Console.WriteLine("【Debug】MainPage x =" + m_docotService.Getstr());

            if (null == editor1.Text ||
                "" == editor1.Text)
            {
                //
                // ニックネームが入力されていない場合
                //
                await DisplayAlert("失敗", "ニックネームば入力してから押さんかバカたれが", "OK");
            }
            else
            {
                //
                // ニックネームが入力されている場合
                //

                // ユーザー情報を登録す
                String str = "";
                if (true == IsUserInfoRegstered())
                {
                    Console.WriteLine("ユーザー情報がすでに登録されている");

                    await DisplayAlert("失敗", "既に登録されとっばい", "OK");

                }
                else
                {
                    str = await RegisterUserInfo(editor1.Text);

                    RegisterUserInfoResponseData data = JsonConvert.DeserializeObject<RegisterUserInfoResponseData>(str);

                    // 設定情報に保存する
                    SetUserPreferences(data.deviceId, data.nickname);

                    await DisplayAlert("成功", "登録の終わったばい", "OK");

                }
            }
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

        class RegisterUserInfoResponseData
        {
            public String deviceId { get; set; }
            public String nickname {get; set; }
	        public int? latitude {get; set; }
	        public int? longitude {get; set; }
	        public int? positionUpdated {get; set; }
	        public String address {get; set; }
	        public String cityCode {get; set; }
            public String cityName {get; set; }

        }

        bool IsUserInfoRegstered()
        {
            bool response = false;

            if (m_docotService.IsUserInfoRegistered())
            {
                //
                // すでに自端末情報が登録されている場合はtrueを返す
                //
                response = true;
            }

            return response;
        }

        public void SetUserPreferences(string deviceId, string nickname)
        {
            m_docotService.SetUserPreferences(deviceId, nickname);

        }

    }
}
