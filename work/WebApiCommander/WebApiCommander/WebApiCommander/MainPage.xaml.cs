using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;

namespace WebApiCommander
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        async void Button_ClickedAsync(object sender, System.EventArgs e)
        {
            // ログ出力する
            System.Diagnostics.Debug.WriteLine("Button Clicked");

            //非同期でダウンロード
            var str = await Download(entry1.Text);

            // ラベルに表示する
            label1.Text = str;

        }

        async Task<String> Download(String url)
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync(url);
        }
    }
}
