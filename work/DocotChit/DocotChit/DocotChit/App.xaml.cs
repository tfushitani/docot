using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DocotChit
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new DocotChit.MainPage();
		}

        public App(ServiceConnectionStub x)
        {
            InitializeComponent();

            Plugin.Media.CrossMedia.Current.Initialize();
            MainPage = new DocotChit.MainPage(x);
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
