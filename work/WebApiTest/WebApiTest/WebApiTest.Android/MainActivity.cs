using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace WebApiTest.Droid
{
	[Activity (Label = "WebApiTest", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

            // レイアウトを設定する
            SetContentView(Resource.Layout.layout1);

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

            var button = FindViewById<Button>(Resource.Id.button1);
            var edittext = FindViewById<TextView>(Resource.Id.textView1);   

            button.Click += (s, e) =>
            {

            };


		}
	}
}

